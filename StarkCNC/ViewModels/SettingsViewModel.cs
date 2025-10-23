using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.DTO;
using StarkCNC.Helpers;
using System.Globalization;

namespace StarkCNC.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private ISettingsRepository _settingsRepository;
    private static FloorTypeToStringConverter _floorTypeToStringConverter = new FloorTypeToStringConverter();

    public IReadOnlyCollection<string> Types { get; } = new List<string>()
    {
        (string)_floorTypeToStringConverter.Convert(FloorType.SingleLevel, typeof(string), null, CultureInfo.CurrentCulture),
        (string)_floorTypeToStringConverter.Convert(FloorType.TwoLevel, typeof(string), null, CultureInfo.CurrentCulture),
        (string)_floorTypeToStringConverter.Convert(FloorType.ThreeLevel, typeof(string), null, CultureInfo.CurrentCulture)
    };

    [ObservableProperty]
    private SettingsDto _settings;

    [ObservableProperty]
    private string _selectedType = string.Empty;


    public SettingsViewModel(ISettingsRepository settingsRepository, IConfiguration configuration)
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        _settingsRepository = settingsRepository;

        var settings = _settingsRepository.GetAsync().Result;
        SettingsDto? settingsDto = null;
        if (settings is not null)
            settingsDto = settings.ToDto(configuration);

        if (settingsDto is null)
            settingsDto = SettingsDto.CreateFromConfiguration(configuration);

        if (settingsDto is not null)
            Settings = settingsDto;

        SelectedType = (string)_floorTypeToStringConverter.Convert(Settings.FloorType, typeof(string), null, CultureInfo.CurrentCulture);

        Settings.PropertyChanged += Settings_PropertyChanged;

        PropertyChanged += SettingsViewModel_PropertyChanged;
    }

    private async void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Settings.FloorType))
            SelectedType = (string)_floorTypeToStringConverter.Convert(Settings.FloorType, typeof(string), null, CultureInfo.CurrentCulture);

        Settings? item;
        if (Settings.Id is not Guid id)
            item = Settings.Parse(Settings.Id);
        else
            item = await _settingsRepository.FindByIdAsync(id).ConfigureAwait(false);

        if (item is not null)
        {
            if (_settingsRepository.Count() == 0)
                await _settingsRepository.AddElementAsync(item).ConfigureAwait(false);
            else
                await _settingsRepository.UpdateElementAsync(item).ConfigureAwait(false);
        }
    }

    private void SettingsViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SelectedType))
            Settings.FloorType = (FloorType)_floorTypeToStringConverter.ConvertBack(SelectedType, typeof(FloorType), null, CultureInfo.CurrentCulture);
    }
}