using System.Text.Json;

namespace CashBackMerge.App;

public static class JsonDeserializer
{
    public static GameConfig DeserializeFromFile(string filename)
    {
        var json = ReadFile(filename);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        return JsonSerializer.Deserialize<GameConfig>(json, options) ??
               throw new InvalidOperationException("Invalid JSON file");
    }

    private static string ReadFile(string filename)
    {
        var path = Path.Combine(AppContext.BaseDirectory, filename);
        return !File.Exists(path) ? throw new FileNotFoundException("JSON file not found") : File.ReadAllText(path);
    }
}