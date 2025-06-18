using StarkCNC.Core.Models;

namespace StarkCNC.Core.Services
{
    public interface IGCodeService
    {
        public async Task SaveAsync(string path, ICollection<BendingData> data) { }

        public async Task<ICollection<BendingData>> ReadAsync(string path)
        {
            return new List<BendingData>();
        }
    }
}
