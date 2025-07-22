using Microsoft.Extensions.Configuration;

namespace StarkCNC.Models
{
    internal class OutputsParametersSwitch
    {
        public string RequestString { get; private set; } = string.Empty;

        public static OutputsParametersSwitch InitializeParameters(IConfigurationSection configurationSection, string sectionName)
        {
            var section = configurationSection.GetSection(sectionName);

            return new OutputsParametersSwitch()
            {
                RequestString = section.GetValue<string>(nameof(RequestString)) ?? string.Empty
            };
        }
    }
}
