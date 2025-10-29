using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models;
using StarkCNC.Core.Models.SettingsParameters;
using StarkCNC.DTO.SettingsParameters;

namespace StarkCNC.DTO;

internal static class SettingsDtoConversions
{
    public static SettingsDto? ToDto(this Settings settings, IConfiguration configuration)
    {
        if (settings is null)
            throw new ArgumentNullException(nameof(settings));

        var settingsDto = SettingsDto.CreateFromConfiguration(configuration);
        if (settingsDto is null)
            throw new ArgumentNullException(nameof(configuration), $"Section \"Settings\" or inner sections not found in {nameof(configuration)}");

        settingsDto.Id = settings.Id;
        settingsDto.FloorType = settings.FloorType;
        settingsDto.IsElectricBendingDrive = settings.IsElectricBendingDrive;
        settingsDto.IsPunchingCylinder = settings.IsPunchingCylinder;
        settingsDto.IsElectricMachine = settings.IsElectricMachine;
        settingsDto.Speed = settings.Speed;
        settingsDto.SynchronizationCoefficient = settings.SynchronizationCoefficient;
        settingsDto.InterceptionMode = settings.InterceptionMode;
        settingsDto.Bend = settings.Bend.ToDto(configuration);
        settingsDto.BendId = settings.BendId;
        settingsDto.Dorn = settings.Dorn.ToDto(configuration);
        settingsDto.DornId = settings.DornId;
        settingsDto.Rotation = settings.Rotation.ToDto(configuration);
        settingsDto.RotationId = settings.RotationId;
        settingsDto.Support = settings.Support.ToDto(configuration);
        settingsDto.SupportId = settings.SupportId;
        settingsDto.Supply = settings.Supply.ToDto(configuration);
        settingsDto.SupplyId = settings.SupplyId;
        settingsDto.Console = settings.Console.ToDto(configuration);
        settingsDto.ConsoleId = settings.ConsoleId;
        settingsDto.Pipe = settings.Pipe.ToDto(configuration);
        settingsDto.PipeId = settings.PipeId;

        return settingsDto;
    }

    public static BendDto? ToDto(this Bend bend, IConfiguration configuration)
    {
        if (bend is null)
            throw new ArgumentNullException(nameof(bend));

        var bendDto = BendDto.CreateFromConfiguration(configuration.GetSection("Settings"));
        if (bendDto is null)
            return bendDto;

        bendDto.Id = bend.Id;
        bendDto.Coefficient = bend.Coefficient;
        bendDto.Synchronization = bend.Synchronization;

        return bendDto;
    }

    public static DornDto? ToDto(this Dorn dorn, IConfiguration configuration)
    {
        if (dorn is null)
            throw new ArgumentNullException(nameof(dorn));

        var dornDto = DornDto.CreateFromConfiguration(configuration.GetSection("Settings"));
        if (dornDto is null)
            return dornDto;

        dornDto.Id = dorn.Id;
        dornDto.Automatic = dorn.Automatic;
        dornDto.LubricantTurnOn = dorn.LubricantTurnOn;
        dornDto.LeadWithdrawalBeforeBend = dorn.LeadWithdrawalBeforeBend;

        return dornDto;
    }

    public static RotationDto? ToDto(this Rotation rotation, IConfiguration configuration)
    {
        if (rotation is null)
            throw new ArgumentNullException(nameof(rotation));

        var rotationDto = RotationDto.CreateFromConfiguration(configuration.GetSection("Settings"));
        if (rotationDto is null)
            return rotationDto;

        rotationDto.Id = rotation.Id;
        rotationDto.Offset = rotation.Offset;
        rotationDto.Coefficient = rotation.Coefficient;
        
        return rotationDto;
    }

    public static SupportDto? ToDto(this Support support, IConfiguration configuration)
    {
        if (support is null)
            throw new ArgumentNullException(nameof(support));

        var supportDto = SupportDto.CreateFromConfiguration(configuration.GetSection("Settings"));
        if (supportDto is null)
            return supportDto;

        supportDto.Id = support.Id;
        supportDto.FrontLiftBan = support.FrontLiftBan;
        supportDto.MiddleLiftBan = support.MiddleLiftBan;
        supportDto.BackLiftBan = support.BackLiftBan;

        return supportDto;
    }

    public static SupplyDto? ToDto(this Supply supply, IConfiguration configuration)
    {
        if (supply is null)
            throw new ArgumentNullException(nameof(supply));

        var supplyDto = SupplyDto.CreateFromConfiguration(configuration.GetSection("Settings"));
        if (supplyDto is null)
            return supplyDto;

        supplyDto.Id = supply.Id;
        supplyDto.ResetedOffset = supply.ResetedOffset;
        supplyDto.StartRollingSpeed = supply.StartRollingSpeed;
        supplyDto.Coefficient = supply.Coefficient;

        return supplyDto;
    }

    public static ConsoleDto? ToDto(this StarkCNC.Core.Models.SettingsParameters.Console console, IConfiguration configuration)
    {
        if (console is null)
            throw new ArgumentNullException(nameof(console));

        var consoleDto = ConsoleDto.CreateFromConfiguration(configuration.GetSection("Settings"));
        if (consoleDto is null)
            return consoleDto;

        consoleDto.Id = console.Id;
        consoleDto.Coefficient = console.Coefficient;

        return consoleDto;
    }

    public static PipeDto? ToDto(this Pipe pipe, IConfiguration configuration)
    {
        if (pipe is null)
            throw new ArgumentNullException(nameof(pipe));

        var pipeDto = PipeDto.CreateFromConfiguration(configuration.GetSection("Settings"));
        if (pipeDto is null)
            return pipeDto;

        pipeDto.Id = pipe.Id;
        pipeDto.OutletCoordinate = pipe.OutletCoordinate;

        return pipeDto;
    }
}
