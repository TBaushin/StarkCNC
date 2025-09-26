using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using System.ComponentModel;

namespace StarkCNC.Core.Repository;

public class AdjustmentRepository : IAdjustmentRepository
{
    private readonly IConfiguration _configuration;
    private readonly List<AdjustmentParameters> _adjustments = new List<AdjustmentParameters>();

    public event PropertyChangedEventHandler? PropertyChanged;

    public AdjustmentRepository(IConfiguration configuration)
    {
        _configuration = configuration;
#if DEBUG
        _adjustments = new List<AdjustmentParameters>()
        {
            new AdjustmentParameters(_configuration, "D25") { InstalledLevel = 1, Type = AdjustmentType.Winding },
            new AdjustmentParameters(_configuration, "D11") { InstalledLevel = 2, Type = AdjustmentType.Rolling },
            new AdjustmentParameters(_configuration, "D50") { InstalledLevel = 3, Type = AdjustmentType.Winding },
            new AdjustmentParameters(_configuration, "Test"),
            new AdjustmentParameters(_configuration, "TestTest"),
            new AdjustmentParameters(_configuration, "Program"),
            new AdjustmentParameters(_configuration, "Abcde"),
            new AdjustmentParameters(_configuration, "StarkCNC"),
            new AdjustmentParameters(_configuration, "ListView"),
            new AdjustmentParameters(_configuration, "ListViewItem"),
            new AdjustmentParameters(_configuration, "AdjustmentParameters"),
            new AdjustmentParameters(_configuration, "D26"),
            new AdjustmentParameters(_configuration, "D12"),
            new AdjustmentParameters(_configuration, "D51"),
            new AdjustmentParameters(_configuration, "Testi"),
            new AdjustmentParameters(_configuration, "TestiTesti"),
            new AdjustmentParameters(_configuration, "Programi"),
            new AdjustmentParameters(_configuration, "Abcdef"),
            new AdjustmentParameters(_configuration, "StarkCNC.Core"),
            new AdjustmentParameters(_configuration, "TreeView"),
            new AdjustmentParameters(_configuration, "TreeViewItem"),
            new AdjustmentParameters(_configuration, "AdjustmentType")
        };
#endif
    }

    public AdjustmentParameters AddElement(string name)
    {
        _adjustments.Add(new AdjustmentParameters(_configuration, name));
        return _adjustments.Last();
    }

    public AdjustmentParameters AddElement(AdjustmentParameters adjustment)
    {
        _adjustments.Add(adjustment);
        return _adjustments.Last();
    }

    public void RemoveElement(string name)
    {
        var item = _adjustments.FirstOrDefault(a => a.Name == name);
        if (item is not null)
            _adjustments.Remove(item);
    }

    public void RemoveElement(AdjustmentParameters adjustment) => _adjustments.Remove(adjustment);

    public IEnumerable<AdjustmentParameters> GetTenElements(int startPosition = 0) =>
        _adjustments.Take(10).Skip(startPosition);

    public IEnumerable<AdjustmentParameters> FindByName(string name) =>
        _adjustments.Where(a => a.Name.Contains(name));

    public int Count() => _adjustments.Count;

    public IEnumerable<AdjustmentParameters> GetAll() => _adjustments;

    public void SetLevel(AdjustmentParameters adjustment, int level)
    {
        adjustment.InstalledLevel = level;
        if (!_adjustments.Contains(adjustment))
        {
            _adjustments.Add(adjustment);
        }
        else
        {
            var currentLevelAdjustments = _adjustments
                .Where(a => a.InstalledLevel == level && !a.Equals(adjustment))
                .ToList();
            currentLevelAdjustments.ForEach(a => a.InstalledLevel = 0);
        }
        PropertyChanged?.Invoke(adjustment, new PropertyChangedEventArgs(nameof(adjustment.InstalledLevel)));
    }

    public AdjustmentParameters? GetAdjustmentWithLevel(int level) =>
        _adjustments.FirstOrDefault(a => a.InstalledLevel == level);

    public IEnumerable<AdjustmentParameters> GetAdjustmentsWithLevel() =>
        _adjustments.Where(a => a.InstalledLevel > 0);
}
