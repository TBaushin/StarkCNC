using Microsoft.Extensions.Configuration;

namespace StarkCNC.Models
{
    public class OutputsParametersTwoButtons
    {
        public string ForwardRequestString { get; private set; } = string.Empty;

        public string BackwardRequestString { get; private set; } = string.Empty;


        public static OutputsParametersTwoButtons InitializeParameters(IConfigurationSection configurationSection, string sectionName)
        {
            var section = configurationSection.GetSection(sectionName);

            return new OutputsParametersTwoButtons()
            {
                ForwardRequestString = section.GetValue<string>(nameof(ForwardRequestString)) ?? string.Empty,
                BackwardRequestString = section.GetValue<string>(nameof(BackwardRequestString)) ?? string.Empty
            };
        }
    }
}
