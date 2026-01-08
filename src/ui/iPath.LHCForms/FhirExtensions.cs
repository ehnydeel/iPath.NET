using System.Text.Json;

namespace iPath.LHCForms;

public static class FhirExtensions
{
    public static string JsonPrettify(this string json)
    {
        using var jDoc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
    }
}
