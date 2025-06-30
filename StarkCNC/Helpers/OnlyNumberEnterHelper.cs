namespace StarkCNC.Helpers
{
    public static class OnlyNumberEnterHelper
    {
        public static bool IsTextAllowed(string text)
        {
            return text.All(IsCharAllowed);
        }

        public static bool IsCharAllowed(char ch)
        {
            return char.IsDigit(ch) || ch == '.' || ch == ',';
        }
    }
}
