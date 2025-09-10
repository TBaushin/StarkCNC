using StarkCNC.Core.Models;
using System.ComponentModel;

namespace StarkCNC.Core.Repository;

public class AdjustmentRepository : IAdjustmentRepository
{
    private readonly List<AdjustmentType> _types = new List<AdjustmentType>();
    private readonly List<AdjustmentParameters> _adjustments = new List<AdjustmentParameters>();

    public event PropertyChangedEventHandler? PropertyChanged;

    public AdjustmentRepository()
    {
#if DEBUG
        _types = new List<AdjustmentType>()
        {
            new AdjustmentType("Намоткой"),
            new AdjustmentType("Прокатная")
        };
        _adjustments = new List<AdjustmentParameters>()
        {
            new AdjustmentParameters("D25", _types.First()) { InstalledLevel = 1 },
            new AdjustmentParameters("D11", _types.Last()) { InstalledLevel = 2 },
            new AdjustmentParameters("D50", _types.First()) { InstalledLevel = 3 },
            new AdjustmentParameters("Test", _types.Last()),
            new AdjustmentParameters("TestTest", _types.First()),
            new AdjustmentParameters("Program", _types.Last()),
            new AdjustmentParameters("Abcde", _types.First()),
            new AdjustmentParameters("StarkCNC", _types.Last()),
            new AdjustmentParameters("ListView", _types.First()),
            new AdjustmentParameters("ListViewItem", _types.Last()),
            new AdjustmentParameters("AdjustmentParameters", _types.First()),
            new AdjustmentParameters("D26", _types.Last()),
            new AdjustmentParameters("D12", _types.First()),
            new AdjustmentParameters("D51", _types.Last()),
            new AdjustmentParameters("Testi", _types.First()),
            new AdjustmentParameters("TestiTesti", _types.Last()),
            new AdjustmentParameters("Programi", _types.First()),
            new AdjustmentParameters("Abcdef", _types.Last()),
            new AdjustmentParameters("StarkCNC.Core", _types.First()),
            new AdjustmentParameters("TreeView", _types.Last()),
            new AdjustmentParameters("TreeViewItem", _types.First()),
            new AdjustmentParameters("AdjustmentType", _types.Last())
        };
#endif
    }

    public AdjustmentParameters AddElement(string name, AdjustmentType type)
    {
        _adjustments.Add(new AdjustmentParameters(name, type));
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

    public IEnumerable<AdjustmentType> GetTypes() => _types;

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
