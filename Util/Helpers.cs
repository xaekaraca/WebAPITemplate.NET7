using System.Security.Cryptography;

namespace Util;

public static class Helpers
{
    public static dynamic Cast(dynamic source, Type dest)
    {
        return Convert.ChangeType(source, dest);
    }

    public static object? GetProperty(object obj, string name)
    {
        return obj.GetType().GetProperty(name);
    }

    public static object GetPropertyValue(object obj, string name)
    {
        return obj.GetType().GetProperty(name)?.GetValue(obj, null)!;
    }

    public static void SetProperty(object obj, string name, object? value)
    {
        obj.GetType().GetProperty(name)?.SetValue(obj, value);
    }

    public static string Base64Encode(string text)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static string GenerateRandomSecret()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}