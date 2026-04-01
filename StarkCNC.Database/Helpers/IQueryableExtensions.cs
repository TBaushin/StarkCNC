using Microsoft.EntityFrameworkCore;

namespace StarkCNC.Database.Helpers;

public static class IQueryableExtensions
{
    public static IQueryable<T> IncludeAll<T>(this IQueryable<T> query, DbContext context) where T : class
    {
        var entityType = context.Model.FindEntityType(typeof(T));
        if (entityType is null)
            return query;

        foreach (var nav in entityType.GetNavigations())
        {
            query = query.Include(nav.Name);

            foreach (var nested in nav.TargetEntityType.GetNavigations())
            {
                query = query.Include($"{nav.Name}.{nested.Name}");
            }
        }

        return query;
    }
}
