namespace StarkCNC.Helpers;

public static class OnlyNumberEnterHelper
{
    public static bool IsTextAllowed(string text) =>
        text.All(c => char.IsNumber(c) || c == '.' || c == ',');
}