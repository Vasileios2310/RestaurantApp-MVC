using System.Text.Json;

namespace RestaurantApp.Models;
/// <summary>
/// Saves objects ino a session
/// Reads objects of a session
/// </summary>
public static class SessionExtensions
{
    /// <summary>
    /// Saves any object (T), into the session under a given key.
    /// Converts the object (e.g., cart) into a JSON string.
    /// Saves that string in session under the key.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    /// <summary>
    ///  Gets the JSON string from session.
    ///  Converts the JSON string back into the object.
    /// </summary>
    /// <param name="session"></param>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Get<T>(this ISession session, string key)
    {
        var json = session.GetString(key);
        if (string.IsNullOrEmpty(json))
        {
            return default(T);
        }
            return JsonSerializer.Deserialize<T>(json);
    }
}