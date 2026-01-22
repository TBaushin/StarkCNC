namespace StarkCNC.Core.Models;

public enum StatusType
{
    Error,
    Warning,
    Success,
    Information
}

public enum StatusPage
{
    Unknown,
    Manual,
    Visualization,
    Program,
    Automatic,
    Adjustment,
    User,
    Settings
}
