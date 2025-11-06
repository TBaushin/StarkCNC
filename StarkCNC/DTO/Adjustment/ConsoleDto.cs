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
    private string _speedCoefficientRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private double _bendPosition;

    [ObservableProperty]
    private double _secondFloorPosition;

    [ObservableProperty]
    private double _secondFloorIntermediatePosition;

    [ObservableProperty]
    private double _thirdFloorPosition;

    [ObservableProperty]
    private double _pipeRotationDepartureDistance;

    [ObservableProperty]
    private double _speedCoefficient;

    public ConsoleDto(
        string bendPositionRequestString,
        string secondFloorPositionRequestString,
        string secondFloorIntermediatePositionRequestString,
        string thirdFloorPositionRequestString,
        string pipeRotationDepartureDistanceRequestString,
        string speedCoefficientRequestString)
    {
        _bendPositionRequestString = bendPositionRequestString;
        _secondFloorPositionRequestString = secondFloorPositionRequestString;
        _secondFloorIntermediatePositionRequestString = secondFloorIntermediatePositionRequestString;
        _thirdFloorPositionRequestString = thirdFloorPositionRequestString;
        _pipeRotationDepartureDistanceRequestString = pipeRotationDepartureDistanceRequestString;
        _speedCoefficientRequestString = speedCoefficientRequestString;
    }

    public object Clone() => MemberwiseClone();

    public StarkCNC.Core.Models.Adjustment.Console Parse(Guid? id) =>
        new StarkCNC.Core.Models.Adjustment.Console(
            BendPosition,
            SecondFloorPosition,
            SecondFloorIntermediatePosition,
            ThirdFloorPosition,
            PipeRotationDepartureDistance,
            SpeedCoefficient) { Id = DtoParseHelper.GetId(Id, id) };

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
            PipeRotationDepartureDistance == other.PipeRotationDepartureDistance &&
            SpeedCoefficient == other.SpeedCoefficient;
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
                _pipeRotationDepartureDistanceRequestString,
                SpeedCoefficient,
                _speedCoefficientRequestString));

    public static ConsoleDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var consoleSection = section.GetSection("Console");

        var bendPositionSection = consoleSection.GetSection("BendPosition");
        var bendPositionDefault = bendPositionSection.GetSection("Default").Get<double>();
        var bendPositionRequestString = bendPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var secondFloorPositionSection = consoleSection.GetSection("SecondFloorPosition");
        var secondFloorPositionDefault = secondFloorPositionSection.GetSection("Default").Get<double>();
        var secondFloorPositionRequestString = secondFloorPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var secondFloorIntermediatePositionSection = consoleSection.GetSection("SecondFloorIntermediatePosition");
        var secondFloorIntermediatePositionDefault = secondFloorIntermediatePositionSection.GetSection("Default").Get<double>();
        var secondFloorIntermediatePositionRequestString = secondFloorIntermediatePositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var thirdFloorPositionSection = consoleSection.GetSection("ThirdFloorPosition");
        var thirdFloorPositionDefault = thirdFloorPositionSection.GetSection("Default").Get<double>();
        var thirdFloorPositionRequestString = thirdFloorPositionSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var pipeRotationDepartureDistanceSection = consoleSection.GetSection("PipeRotationDepartureDistance");
        var pipeRotationDepartureDistanceDefault = pipeRotationDepartureDistanceSection.GetSection("Default").Get<double>();
        var pipeRotationDepartureDistanceRequestString = pipeRotationDepartureDistanceSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        var speedCoefficientSection = consoleSection.GetSection("SpeedCoefficient");
        var speedCoefficientDefault = speedCoefficientSection.GetSection("Default").Get<double>();
        var speedCoefficientRequestString = speedCoefficientSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new ConsoleDto(
            bendPositionRequestString,
            secondFloorPositionRequestString,
            secondFloorIntermediatePositionRequestString,
            thirdFloorPositionRequestString,
            pipeRotationDepartureDistanceRequestString,
            speedCoefficientRequestString)
        {
            BendPosition = bendPositionDefault,
            SecondFloorPosition = secondFloorPositionDefault,
            SecondFloorIntermediatePosition = secondFloorIntermediatePositionDefault,
            ThirdFloorPosition = thirdFloorPositionDefault,
            PipeRotationDepartureDistance = pipeRotationDepartureDistanceDefault,
            SpeedCoefficient = speedCoefficientDefault
        };
    }
}