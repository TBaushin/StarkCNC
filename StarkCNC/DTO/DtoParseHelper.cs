namespace StarkCNC.DTO;

public static class DtoParseHelper
{
    public static Guid GetId(Guid? propertyId, Guid? outerId)
    {
        var newId = Guid.NewGuid();
        if (propertyId is not null && propertyId != Guid.Empty)
            return (Guid)propertyId;

        if (outerId is not null && outerId != Guid.Empty)
            return (Guid)outerId;

        return newId;
    }
}
