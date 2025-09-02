using StarkCNC.Core.Models;

namespace StarkCNC.Core.Repository
{
    public class AdjustmentRepository : IAdjustmentRepository
    {
        private readonly List<AdjustmentType> _types = new List<AdjustmentType>();
        private readonly List<AdjustmentParameters> _adjustments = new List<AdjustmentParameters>();

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
                new AdjustmentParameters("D25", _types.First()),
                new AdjustmentParameters("D11", _types.Last()),
                new AdjustmentParameters("D50", _types.First()),
                new AdjustmentParameters("Test", _types.Last()),
                new AdjustmentParameters("TestTest", _types.First()),
                new AdjustmentParameters("Program", _types.Last()),
                new AdjustmentParameters("Abcde", _types.First()),
                new AdjustmentParameters("StarkCNC", _types.Last()),
                new AdjustmentParameters("ListView", _types.First()),
                new AdjustmentParameters("ListViewItem", _types.Last()),
                new AdjustmentParameters("AdjustmentParameters", _types.First()),
                new AdjustmentParameters("D25", _types.Last()),
                new AdjustmentParameters("D11", _types.First()),
                new AdjustmentParameters("D50", _types.Last()),
                new AdjustmentParameters("Test", _types.First()),
                new AdjustmentParameters("TestTest", _types.Last()),
                new AdjustmentParameters("Program", _types.First()),
                new AdjustmentParameters("Abcde", _types.Last()),
                new AdjustmentParameters("StarkCNC", _types.First()),
                new AdjustmentParameters("ListView", _types.Last()),
                new AdjustmentParameters("ListViewItem", _types.First()),
                new AdjustmentParameters("AdjustmentParameters", _types.Last())
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

        public IEnumerable<AdjustmentParameters> GetTenElements(int startPosition = 0) => _adjustments.Take(10).Skip(startPosition);

        public IEnumerable<AdjustmentParameters> FindByName(string name) => _adjustments.Where(a => a.Name.Contains(name));

        public int Count() => _adjustments.Count;

        public IEnumerable<AdjustmentParameters> GetAll() => _adjustments;

        public IEnumerable<AdjustmentType> GetTypes() => _types;
    }
}
