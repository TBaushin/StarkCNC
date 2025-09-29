namespace StarkCNC.Models;

public class TitleValue
{
    public string PropertyName { get; set; }

    public string DisplayedTitle { get; set; }

    public object Value { get; set; }

    public TitleValue(string propertyName, string displayedTitle, object value)
    {
        PropertyName = propertyName;
        DisplayedTitle = displayedTitle;
        Value = value;
    }
}
