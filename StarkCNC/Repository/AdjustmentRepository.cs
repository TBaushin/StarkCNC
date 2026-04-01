using Microsoft.EntityFrameworkCore;
using StarkCNC.Core.Models;
using StarkCNC.Core.Repository;
using StarkCNC.Database;
using StarkCNC.Database.Helpers;

namespace StarkCNC.Repository;

public class AdjustmentRepository : IAdjustmentRepository
{
    private IDbContextFactory<AppJsonContext> _contextFactory;

    public AdjustmentRepository(IDbContextFactory<AppJsonContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<AdjustmentParameters?> AddElementAsync(AdjustmentParameters adjustment)
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        await context.AddAsync(adjustment).ConfigureAwait(false);
        await context.SaveChangesAsync().ConfigureAwait(false);

        return adjustment;
    }

    public async Task RemoveElementAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        var item = context.Adjustments.FirstOrDefault(a => a.Id == id);
        if (item is not null)
        {
            context.Remove(item);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    public async Task RemoveElementAsync(AdjustmentParameters adjustment)
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        context.Remove(adjustment);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateElementAsync(AdjustmentParameters adjustment)
    {
        if (adjustment is null)
            throw new ArgumentNullException(nameof(adjustment));

        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);

        var local = await FindByIdAsync(adjustment.Id).ConfigureAwait(false);
        if (local is not null)
        {
            context.Entry(local).CurrentValues.SetValues(adjustment);
            await UpdateLocalEntry(adjustment, local).ConfigureAwait(false);
        }
        else
            context.Entry(adjustment).State = EntityState.Modified;

        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<AdjustmentParameters>> FindByNameAsync(string name)
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        return await context.Adjustments
            .AsNoTracking()
            .IncludeAll(context)
            .Where(a => a.Name.Contains(name, StringComparison.CurrentCulture))
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<AdjustmentParameters?> FindByIdAsync(Guid id)
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        return await context.Adjustments
            .AsNoTracking()
            .IncludeAll(context)
            .FirstOrDefaultAsync(a => a.Id == id)
            .ConfigureAwait(false);
    }

    public int Count()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Adjustments.Count();
    }

    public async Task<IEnumerable<AdjustmentParameters>> GetAllAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        return await context.Adjustments
            .AsNoTracking()
            .IncludeAll(context)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task SetLevelAsync(Guid id, int level)
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        var item = await context.Adjustments.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
        if (item is null)
            return;

        item.InstalledLevel = level;
        context.Entry(item).State = EntityState.Modified;
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<AdjustmentParameters?> GetAdjustmentWithLevelAsync(int level)
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        return await context.Adjustments
            .AsNoTracking()
            .IncludeAll(context)
            .FirstOrDefaultAsync(a => a.InstalledLevel == level)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<AdjustmentParameters>> GetAdjustmentsWithLevelAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        return await context.Adjustments
            .AsNoTracking()
            .IncludeAll(context)
            .Where(a => a.InstalledLevel > 0)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    private async Task UpdateLocalEntry(AdjustmentParameters item, AdjustmentParameters local)
    {
        using var context = await _contextFactory.CreateDbContextAsync().ConfigureAwait(false);
        context.Entry(local.Bend).CurrentValues.SetValues(item.Bend);
        context.Entry(local.BendRoller).CurrentValues.SetValues(item.BendRoller);
        context.Entry(local.Clamp).CurrentValues.SetValues(item.Clamp);
        context.Entry(local.ClampRoller).CurrentValues.SetValues(item.ClampRoller);
        context.Entry(local.Console).CurrentValues.SetValues(item.Console);
        context.Entry(local.Dorn).CurrentValues.SetValues(item.Dorn);
        context.Entry(local.Lift).CurrentValues.SetValues(item.Lift);
        context.Entry(local.Press).CurrentValues.SetValues(item.Press);
        context.Entry(local.Rotation).CurrentValues.SetValues(item.Rotation);
        context.Entry(local.Squeeze).CurrentValues.SetValues(item.Squeeze);
        context.Entry(local.Supply).CurrentValues.SetValues(item.Supply);
    }
}