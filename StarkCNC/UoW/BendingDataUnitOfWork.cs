using Microsoft.Win32;
using StarkCNC.Core.Models;
using StarkCNC.Core.Services;
using StarkCNC.Core.UoW;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace StarkCNC.UoW;

public class BendingDataUnitOfWork : IBendingDataUnitOfWork
{
    private static readonly string _tempFolder = Path.GetTempPath() + "\\StarkCNC";
    private const string _tempFileName = "last_used_file.txt";
    private const string _gcodeExtension = ".gcode";
    private const string _gcodeFilter = "GCode (.gc, .g, .gcode, .txt)|*.gc;*.g;*.gcode;*.txt;";


    private IGCodeService _gCodeService;

    public string CurrentFilePath { get; private set; } = string.Empty;

    public string ProgramName { get; private set; } = string.Empty;

    public ICollection<BendingData> BendingDatas { get; } = new List<BendingData>();

    public double PipeLength { get; set; }

    public double SetUpPoint { get; set; }

    public double EstimatedRemainingLength { get; set; }

    public bool HasUnsavedData { get; set; }

    public BendingDataUnitOfWork(IGCodeService gCodeService)
    {
        _gCodeService = gCodeService;

        Initialize();
    }

    public double CalculateEstimatedRemainingLength()
    {
        double result = PipeLength;
        foreach (var data in BendingDatas)
        {
            result -= data.Supply + (2 * Math.PI * data.BendingRadius / 360 * data.BendingAngle);
        }

        EstimatedRemainingLength = result;
        return EstimatedRemainingLength;
    }

    public double CalculatePipeLength()
    {
        double result = EstimatedRemainingLength;
        foreach (var data in BendingDatas)
        {
            result += data.Supply - (2 * Math.PI * data.BendingRadius / 360 * data.BendingAngle);
        }

        PipeLength = result;
        return PipeLength;
    }

    public async Task CreateNewFile()
    {
        if (!await SaveFile().ConfigureAwait(true))
            return;

        var dialog = new SaveFileDialog();
        dialog.DefaultExt = _gcodeExtension;
        dialog.Filter = _gcodeFilter;

        bool? result = dialog.ShowDialog();

        if (result == true)
        {
            CurrentFilePath = dialog.FileName;
            await SaveFile().ConfigureAwait(false);
        }
        else
            return;
    }

    public async Task OpenFile()
    {
        var dialog = new OpenFileDialog();
        dialog.DefaultExt = _gcodeExtension;
        dialog.Filter = _gcodeFilter;

        bool? result = dialog.ShowDialog();

        if (result == true)
        {
            BendingDatas.Clear();
            CurrentFilePath = dialog.FileName;
            await ReadFileAsync(CurrentFilePath).ConfigureAwait(false);

            var first = BendingDatas.FirstOrDefault();

            PipeLength = first?.PipeLength ?? 0;
            SetUpPoint = first?.YSetup ?? 0;
        }
        else
            return;
    }

    public async Task<bool> SaveFile()
    {
        if (string.IsNullOrEmpty(CurrentFilePath))
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = _gcodeExtension;
            dialog.Filter = _gcodeFilter;

            bool? result = dialog.ShowDialog();

            if (result == true)
                CurrentFilePath = dialog.FileName;
            else
                return false;
        }

        foreach(var data in BendingDatas)
        {
            data.PipeLength = PipeLength;
            data.YSetup = SetUpPoint;
        }

        await WriteFileAsync(CurrentFilePath).ConfigureAwait(false);
        HasUnsavedData = false;
        return true;
    }

    public async Task ReadFileAsync(string filePath)
    {
        BendingDatas.Clear();

        CurrentFilePath = filePath;
        ProgramName = Path.GetFileNameWithoutExtension(filePath);
        var data = await _gCodeService.ReadAsync(filePath).ConfigureAwait(false);
        if (data is null)
            return;

        foreach(var d in data)
        {
            BendingDatas.Add(d);
        }

        await SaveLastPathToProgramGCodeFile(filePath).ConfigureAwait(false);
    }

    public async Task WriteFileAsync(string filePath)
    {
        await _gCodeService.SaveAsync(filePath, BendingDatas).ConfigureAwait(false);
        CurrentFilePath = filePath;
        ProgramName = Path.GetFileNameWithoutExtension(filePath);

        await SaveLastPathToProgramGCodeFile(filePath).ConfigureAwait(false);
    }

    private async void Initialize()
    {
        var filePath = await ReadLastPathToProgramGCodeFile().ConfigureAwait(false);
        if (string.IsNullOrEmpty(filePath))
            return;

        CurrentFilePath = filePath;

        if (File.Exists(CurrentFilePath))
            await ReadFileAsync(filePath).ConfigureAwait(false);
        else
            CurrentFilePath = string.Empty;

        var first = BendingDatas.FirstOrDefault();

        PipeLength = first?.PipeLength ?? 0;
        SetUpPoint = first?.YSetup ?? 0;
    }

    private static async Task SaveLastPathToProgramGCodeFile(string filePath)
    {
        if (!Directory.Exists(_tempFolder))
            Directory.CreateDirectory(_tempFolder);

        var tempFullPath = Path.Combine(_tempFolder, _tempFileName);

        var bytes = Encoding.UTF8.GetBytes(filePath);
        var data = Convert.ToBase64String(bytes);
        await File.WriteAllTextAsync(tempFullPath, data).ConfigureAwait(false);
    }

    private static async Task<string?> ReadLastPathToProgramGCodeFile()
    {
        if (!Directory.Exists(_tempFolder))
            return null;

        var tempFullPath = Path.Combine(_tempFolder, _tempFileName);
        if (!File.Exists(tempFullPath))
            return null;

        var data = await File.ReadAllTextAsync(tempFullPath).ConfigureAwait(false);
        try
        {
            var bytes = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(bytes);
        }
        catch (ArgumentNullException)
        {
            Debug.WriteLine($"При чтении последнего используемого файла в {nameof(BendingDataUnitOfWork)} из папки {tempFullPath} произошла ошибка {nameof(ArgumentNullException)}");
            return null;
        }
        catch (EncoderFallbackException)
        {
            Debug.WriteLine($"При чтении последнего используемого файла в {nameof(BendingDataUnitOfWork)} из папки {tempFullPath} произошла ошибка {nameof(EncoderFallbackException)}");
            return null;
        }
    }
}
