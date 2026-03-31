using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace StarkCNC.Utilities;

public class Cryptography
{
    private static byte[] _key;
    private static byte[] _vector;

    static Cryptography()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.v2.json", optional: false, reloadOnChange: true)
            .Build();

        _key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Settings:PrivateKey") ?? string.Empty));
        _vector = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Settings:InitVector") ?? string.Empty));
    }

    public static string Encrypt<T>(T data) where T : class
    {
        var json = JsonSerializer.Serialize(data);

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _vector;

        using var notEncrypted = new MemoryStream(Encoding.UTF8.GetBytes(json));
        using var encrypted = new MemoryStream();
        using (var cs = new CryptoStream(encrypted, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            byte[] bin = new byte[100];
            long readTotal = 0;
            while (readTotal < notEncrypted.Length)
            {
                int len = notEncrypted.Read(bin, 0, 100);
                cs.Write(bin, 0, len);
                readTotal += len;
            }
        }

        return Convert.ToBase64String(encrypted.ToArray());
    }

    public static async Task<string> EncryptAsync<T>(T data) where T : class
    {
        var json = JsonSerializer.Serialize(data);

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _vector;

        await using var notEncrypted = new MemoryStream(Encoding.UTF8.GetBytes(json));
        await using var ecnrypted = new MemoryStream();

        await using (var cs = new CryptoStream(ecnrypted, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            byte[] bin = new byte[100];
            long readTotal = 0;
            while (readTotal < notEncrypted.Length)
            {
                int len = notEncrypted.Read(bin, 0, 100);
                await cs.WriteAsync(bin, 0, len).ConfigureAwait(false);
                readTotal += len;
            }
        }        

        return Convert.ToBase64String(ecnrypted.ToArray());
    }

    public static T? Decrypt<T>(string data) where T : class
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _vector;

        using var encrypted = new MemoryStream(Convert.FromBase64String(data));

        using var cs = new CryptoStream(encrypted, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using var sr = new StreamReader(cs, Encoding.UTF8);
        var result = sr.ReadToEnd();

        return JsonSerializer.Deserialize<T>(result);
    }

    public static async Task<T?> DecryptAsync<T>(string data) where T : class
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _vector;

        await using var encrypted = new MemoryStream(Convert.FromBase64String(data));

        await using var cs = new CryptoStream(encrypted, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using var sr = new StreamReader(cs, Encoding.UTF8);

        var result = await sr.ReadToEndAsync().ConfigureAwait(false);

        return JsonSerializer.Deserialize<T>(result);
    }
}
