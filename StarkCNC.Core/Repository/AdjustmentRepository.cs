using StarkCNC.Core.Models;

namespace StarkCNC.Core.Repository
{
    public class AdjustmentRepository : IAdjustmentRepository
    {
        private readonly List<AdjustmentParameters> _adjustments = new List<AdjustmentParameters>();

        public AdjustmentParameters AddElement(string name)
        {
            _adjustments.Add(new AdjustmentParameters(name));
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
    }
}
