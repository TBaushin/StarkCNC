using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;

namespace StarkCNC.Core.Services
{
    public class SettingsService : ISettingsService
    {
        public IReadOnlyCollection<FloorType> FloorTypes { get; }

        public FloorType? SelectedFloorType { get; set; }

        public bool IsElectricBendingDrive { get; set; } = false;

        public bool IsPunchingCylinder { get; set; } = false;

        public bool IsElectricMachine { get; set; } = false;

        public SettingsService(IConfiguration configuration)
        {
            FloorTypes = ReadLevels(configuration).ToList();
        }

        private static ICollection<FloorType> ReadLevels(IConfiguration configuration)
        {
            var floors = configuration.GetSection("Settings").GetSection("Levels").Get<ICollection<FloorType>>();
            if (floors is null)
                floors = new List<FloorType>()
                {
                    new FloorType("Одноуровневый", 1), // TODO: Вынести в Localization
                    new FloorType("Двухуровневый", 2), // TODO: Вынести в Localization
                    new FloorType("Трёхуровневый", 3) // TODO: Вынести в Localization
                };

            return floors;
        }
    }
}
