using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace StarkCNC.Core;

public enum EntityState
{
    Unchanged = 0,
    Deleted = 1,
    Modified = 2,
    Added = 3
}

public class EntityEntry<T> where T : class
{
    public T Value { get; set; }
    public EntityState State { get; set; }

    public EntityEntry(T value)
    {
        Value = value;
        State = EntityState.Unchanged;
    }
}

public class JsonSet<T> : System.Collections.Generic.IEnumerable<T> where T : class
{
    private readonly List<T> _entries = new List<T>();

    public void Add(T item) => _entries.Add(item);

    public void Remove(T item) => _entries.Remove(item);

    public IEnumerator<T> GetEnumerator() => _entries.GetEnumerator();

    public void ForEach(Action<T> action)
    {
        foreach (var item in _entries)
            action(item);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class JsonContext
{
    protected string _savePath;
    protected string _defaultFileName;
    protected readonly Dictionary<Type, object> _sets = new Dictionary<Type, object>();
    protected readonly Dictionary<Type, JsonSet<object>> _entries = new Dictionary<Type, JsonSet<object>>();

    public JsonContext(IConfiguration configuration)
    {
        _savePath = GetSavePath(configuration) ?? AppDomain.CurrentDomain.BaseDirectory;
        _defaultFileName = GetDefaultFileName(configuration) ?? "settings.json";
    }

    public JsonSet<T> Set<T>() where T : class
    {
        var type = typeof(T);
        JsonSet<T> set;

        if (!_sets.ContainsKey(type))
        {
            set = new JsonSet<T>();
            _sets[type] = set;
        }
        else
        {
            var selectedSet = _sets[type] as JsonSet<T>;
            if (selectedSet is null)
                set = new JsonSet<T>();
            else
                set = selectedSet;
        }

        return set;
    }

    public T Add<T>(T value) where T : class
    {
        var type = typeof(T);

        if (!_entries.ContainsKey(type))
            _entries[type] = new JsonSet<object>();

        var entry = new EntityEntry<T>(value) { State = EntityState.Added };
        _entries[type].Add(entry);

        var set = Set<T>();
        set.Add(value);

        return set.Last();
    }

    public void Remove<T>(T value) where T : class
    {
        var type = typeof(T);
        if (!_entries.ContainsKey(type))
            _entries[type] = new JsonSet<object>();

        var entry = new EntityEntry<T>(value) { State = EntityState.Deleted };
        _entries[type].Add(entry);

        Set<T>().Remove(value);
    }

    public EntityEntry<T> Entry<T>(T entity) where T : class
    {
        var type = typeof(T);
        if (!_entries.ContainsKey(type))
            _entries[type] = new JsonSet<object>();

        EntityEntry<T>? result;
        if (_entries.TryGetValue(type, out var entry))
        {
            result = entry.FirstOrDefault(e =>
            {
                var item = e as EntityEntry<T>;
                if (item is not null && item.Value == entity)
                    return true;
                else
                    return false;
            }) as EntityEntry<T>;

            if (result is not null)
                return result;
        }

        result = new EntityEntry<T>(entity) { State = EntityState.Added };
        _entries[type].Add(result);
        return result;
    }

    public virtual async Task SaveChangesAsync()
    {
        var toSave = GetToSaveElements();

        var json = JsonSerializer.Serialize(toSave, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_savePath + "\\" + _defaultFileName, json).ConfigureAwait(false);

        UpdateStatesAfterSave();
    }

    protected Dictionary<string, IEnumerable<object>> GetToSaveElements()
    {
        var toSave = new Dictionary<string, IEnumerable<object>>();

        foreach (var kvp in _sets)
        {
            var type = kvp.Key;
            var set = Convert.ChangeType(kvp.Value, typeof(JsonSet<EntityEntry<object>>), CultureInfo.CurrentCulture) as JsonSet<EntityEntry<object>>;
            if (set is null)
                throw new ArgumentNullException(nameof(set));

            var all = set
                .Where(e => e.State != EntityState.Deleted)
                .Cast<object>()
                .ToList();

            toSave[type.AssemblyQualifiedName ?? type.FullName!] = all;
        }

        return toSave;
    }

    protected void UpdateStatesAfterSave()
    {
        foreach (var list in _entries.Values)
        {
            if (list is null)
                continue;

            list.ForEach(e =>
            {
                var prop = e.GetType().GetProperty("State");
                if (prop is not null)
                {
                    var state = prop.GetValue(e) as EntityState? ?? EntityState.Unchanged;
                    if (state == EntityState.Added || state == EntityState.Modified)
                        prop.SetValue(e, EntityState.Unchanged);
                }
            });
        }
    }

    private string? GetSavePath(IConfiguration configuration)
    {
        var section = configuration.GetSection("SaveParameters");
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        return section.GetSection("Path").Get<string>();
    }

    private string? GetDefaultFileName(IConfiguration configuration)
    {
        var section = configuration.GetSection("SaveParameters");
        if (section is null)
            throw new ArgumentNullException(nameof(section));

        return section.GetSection("DefaultFileName").Get<string>();
    }
}
