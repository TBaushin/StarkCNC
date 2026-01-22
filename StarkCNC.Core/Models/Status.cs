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
}
