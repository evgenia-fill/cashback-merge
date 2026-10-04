using System.Text.Json;

namespace CashBackMerge.App;

public class JsonDeserializer
{
    public int MovesLimit { get; init; }
    public IReadOnlyList<SpawnSettings> SpawnRules { get; init; } = [];

    public static JsonDeserializer Deserialize(string filename)
    {
        var path = Path.Combine(AppContext.BaseDirectory, filename);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("JSON file not found");
        }

        var json = File.ReadAllText(path);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        return JsonSerializer.Deserialize<JsonDeserializer>(json, options) ??
               throw new InvalidOperationException("Invalid JSON file");
    }
}