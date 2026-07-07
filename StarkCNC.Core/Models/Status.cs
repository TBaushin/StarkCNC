namespace StarkCNC.Core.Models;

public class Status
{
    public DateTime EventTime { get; }

    public string Text { get; }

    public StatusType Type { get; }

    public StatusPage Page { get; }

    public Status(string text, StatusType type = StatusType.Information, StatusPage page = StatusPage.Unknown)
    {
        EventTime = DateTime.Now;
        Text = text;
        Type = type;
        Page = page;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Status other)
            return false;

        return Text.Equals(other.Text) &&
            Type == other.Type &&
            Page == other.Page;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Text, Type, Page, EventTime);
    }
}
