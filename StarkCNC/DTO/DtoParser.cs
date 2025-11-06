namespace StarkCNC.DTO;

internal static class DtoParser
{
    public static T RequireNotNull<T>(T? value, string parameterName) where T : class
    {
        if (value is null)
            throw new ArgumentNullException(parameterName);

        return value;
    }

    public static T RequireNotNull<T>(T? value, string parameterName) where T : struct
    {
        if (value is null)
            throw new ArgumentNullException(parameterName);

        return value.Value;
    }
}