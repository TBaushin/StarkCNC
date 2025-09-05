using Microsoft.Extensions.Configuration;

namespace StarkCNC.Services;

public static class ConfigurationReaderService
{
    public static string GetRequestStringFromConfiguration(IConfigurationSection? section, string configKey)
    {
        if (section is null)
            return string.Empty;

        return section.GetValue<string>(configKey) ?? string.Empty;
    }
}
