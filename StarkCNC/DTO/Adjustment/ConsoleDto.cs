using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace StarkCNC.DTO.Adjustment;

public partial class ConsoleDto : ObservableObject, ICloneable
{
    private string _bendPositionRequestString = string.Empty;
    private string _secondFloorPositionRequestString = string.Empty;
    private string _secondFloorIntermediatePositionRequestString = string.Empty;
    private string _thirdFloorPositionRequestString = string.Empty;
    private string _pipeRotationDepartureDistanceRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private float _bendPosition;

    [ObservableProperty]
    private float _secondFloorPosition;

    [ObservableProperty]
    private float _secondFloorIntermediatePosition;

    [ObservableProperty]
    private float _thirdFloorPosition;

    [ObservableProperty]
    private float _pipeRotationDepartureDistance;

    public ConsoleDto(
        string bendPositionRequestString,
        string secondFloorPositionRequestString,
        string secondFloorIntermediatePositionRequestString,
        string thirdFloorPositionRequestString,
        string pipeRotationDepartureDistanceRequestString)
    {
        _bendPositionRequestString = bendPositionRequestString;
        _secondFloorPositionRequestString = secondFloorPositionRequestString;
        _secondFloorIntermediatePositionRequestString = secondFloorIntermediatePositionRequestString;
        _thirdFloorPositionRequestString = thirdFloorPositionRequestString;
        _pipeRotationDepartureDistanceRequestString = pipeRotationDepartureDistanceRequestString;
    }

    public object Clone() => MemberwiseClone();

    public StarkCNC.Core.Models.Adjustment.Console Parse(Guid? id) =>
        new StarkCNC.Core.Models.Adjustment.Console(
            BendPosition,
            SecondFloorPosition,
            SecondFloorIntermediatePosition,
            ThirdFloorPosition,
            PipeRotationDepartureDistance) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not ConsoleDto other)
            return false;

        return
            Id == other.Id &&
            BendPosition == other.BendPosition &&
            SecondFloorPosition == other.SecondFloorPosition &&
            SecondFloorIntermediatePosition == other.SecondFloorIntermediatePosition &&
            ThirdFloorPosition == other.ThirdFloorPosition &&
            PipeRotationDepartureDistance == other.PipeRotationDepartureDistance;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            BendPosition,
            _bendPositionRequestString,
            SecondFloorPosition,
            _secondFloorPositionRequestString,
            SecondFloorIntermediatePosition,
            _secondFloorIntermediatePositionRequestString,
            HashCode.Combine(
                ThirdFloorPosition,
                _thirdFloorPositionRequestString,
                PipeRotationDepartureDistance,
                _pipeRotationDepartureDistanceRequestString));

    public static ConsoleDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var consoleSection = section.GetSection("Console");

        var bendPositionSection = consoleSection.GetSection("BendPosition");
        var bendPositionDefault = bendPositionSection.GetSection("Default").Get<float>();
        var bendPositionRequestString = bendPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var secondFloorPositionSection = consoleSection.GetSection("SecondFloorPosition");
        var secondFloorPositionDefault = secondFloorPositionSection.GetSection("Default").Get<float>();
        var secondFloorPositionRequestString = secondFloorPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var secondFloorIntermediatePositionSection = consoleSection.GetSection("SecondFloorIntermediatePosition");
        var secondFloorIntermediatePositionDefault = secondFloorIntermediatePositionSection.GetSection("Default").Get<float>();
        var secondFloorIntermediatePositionRequestString = secondFloorIntermediatePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var thirdFloorPositionSection = consoleSection.GetSection("ThirdFloorPosition");
        var thirdFloorPositionDefault = thirdFloorPositionSection.GetSection("Default").Get<float>();
        var thirdFloorPositionRequestString = thirdFloorPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var pipeRotationDepartureDistanceSection = consoleSection.GetSection("PipeRotationDepartureDistance");
        var pipeRotationDepartureDistanceDefault = pipeRotationDepartureDistanceSection.GetSection("Default").Get<float>();
        var pipeRotationDepartureDistanceRequestString = pipeRotationDepartureDistanceSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new ConsoleDto(
            bendPositionRequestString,
            secondFloorPositionRequestString,
            secondFloorIntermediatePositionRequestString,
            thirdFloorPositionRequestString,
            pipeRotationDepartureDistanceRequestString)
        {
            BendPosition = bendPositionDefault,
            SecondFloorPosition = secondFloorPositionDefault,
            SecondFloorIntermediatePosition = secondFloorIntermediatePositionDefault,
            ThirdFloorPosition = thirdFloorPositionDefault,
            PipeRotationDepartureDistance = pipeRotationDepartureDistanceDefault
        };
    }
}