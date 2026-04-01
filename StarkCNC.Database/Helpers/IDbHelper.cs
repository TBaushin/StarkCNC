using System.Text.Json;

namespace StarkCNC.Database.Helpers;

public interface IDbHelper
{
    public static IEnumerable<T> Read<T>(string path) where T : class
    {
        var elements = new List<T>();

        if (!Directory.Exists(path))
            return elements;

        var files = Directory.GetFiles(path);
        try
        {
            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var obj = JsonSerializer.Deserialize<T>(json);
                if (obj is null)
                    continue;

                elements.Add(obj);
            }
        }
        catch (Exception)
        {
            // Ignore
        }

        return elements;
    }

    private static Dictionary<string, T> ReadWithPathes<T>(string path) where T : class
    {
        var elements = new Dictionary<string, T>();

        if (!Directory.Exists(path))
            return elements;

        var files = Directory.GetFiles(path);
        try
        {
            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var obj = JsonSerializer.Deserialize<T>(json);
                if (obj is null)
                    continue;

                elements.Add(file, obj);
            }
        }
        catch (Exception)
        {
            // Ignore
        }

        return elements;
    }

    public static async Task Save<T>(string path, IEnumerable<T> elements) where T : class
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var opt = new JsonSerializerOptions() { WriteIndented = true };

        foreach (var element in elements)
        {
            var nameProperty = element.GetType().GetProperty("Name");
            var name = nameProperty?.GetValue(element) as string;
            if (!string.IsNullOrEmpty(name))
            {
                var filepath = $"{path}\\{name}.json";
                var json = JsonSerializer.Serialize(element, opt);
                await File.WriteAllTextAsync(filepath, json).ConfigureAwait(false);
                continue;
            }

            var idProperty = element.GetType().GetProperty("Id");
            var id = idProperty?.GetValue(element);
            if (id is Guid guid)
            {
                var filepath = $"{path}\\{guid}.json";
                var json = JsonSerializer.Serialize(element, opt);
                await File.WriteAllTextAsync(filepath, json).ConfigureAwait(false);
                continue;
            }
        }
    }

    public static async Task Delete<T>(
        string path,
        IEnumerable<T> elements,
        Func<T, Guid> keySelector) where T : class
    {
        if (!Directory.Exists(path))
            return;

        var contents = ReadWithPathes<T>(path);
        if (!contents.Any())
            return;

        var elementIds = new HashSet<Guid>(elements.Select(keySelector));

        var forDelete = contents.Where(c => !elementIds.Contains(keySelector(c.Value)));
        foreach (var del in forDelete)
        {
            File.Delete(del.Key);
        }
    }
}