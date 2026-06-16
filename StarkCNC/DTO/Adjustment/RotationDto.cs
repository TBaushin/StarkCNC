using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Core.Models.Adjustment;

namespace StarkCNC.DTO.Adjustment;

public partial class RotationDto : ObservableObject, ICloneable
{
    private string _offsetAfterZeroSearchRequestString = string.Empty;

    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private float _offsetAfterZeroSearch;

    public RotationDto(string offsetAfterZeroSearchRequestString)
    {
        _offsetAfterZeroSearchRequestString = offsetAfterZeroSearchRequestString;
    }

    public object Clone() => MemberwiseClone();

    public Rotation Parse(Guid? id) =>
        new Rotation(OffsetAfterZeroSearch) { Id = DtoParseHelper.GetId(Id, id) };

    public override bool Equals(object? obj)
    {
        if (obj is not RotationDto other)
            return false;

        return
            other.Id == Id &&
            other.OffsetAfterZeroSearch == OffsetAfterZeroSearch;
    }

    public override int GetHashCode() =>
        HashCode.Combine(
            Id,
            OffsetAfterZeroSearch,
            _offsetAfterZeroSearchRequestString);

    public static RotationDto? CreateFromConfiguration(IConfigurationSection section)
    {
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        var rotationSection = section.GetSection("Rotation");

        var offsetAfterZeroSearchSection = rotationSection.GetSection("OffsetAfterZeroSearch");
        var offsetAfterZeroSearchDefault = offsetAfterZeroSearchSection.GetSection("Default").Get<float>();
        var offsetAfterZeroSearchRequestString = offsetAfterZeroSearchSection.GetSection("RequestString").Get<string>() ?? string.Empty;

        return new RotationDto(offsetAfterZeroSearchRequestString)
        {
            OffsetAfterZeroSearch = offsetAfterZeroSearchDefault
        };
    }
}