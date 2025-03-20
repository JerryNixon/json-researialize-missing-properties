using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Library.Models;

namespace Library.Json;

public class JsonWriter
{
    public static bool TryWrite<T>(T model, out string json) where T : class
    {
        json = string.Empty;

        if (model is null)
        {
            return false;
        }

        if (TryGetOriginalJson(model, out string originalJson))
        {
            json = originalJson;
        }

        if (TrySerializeModel(model, out string serializedJson))
        {
            json = serializedJson;
        }

        if (json.CompliesWithSchema())
        {
            return true;
        }

        return false;
    }

    private static bool TryGetOriginalJson<T>(T model, out string json) where T : class
    {
        json = string.Empty;

        var state = model.GetState();
        if (state.OriginalJsonStructure == null)
        {
            return false;
        }

        json = state.OriginalJsonStructure.ToJsonString();
        return true;
    }

    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
    };

    private static bool TrySerializeModel<T>(T model, out string json) where T : class
    {
        var state = model.GetState();
        json = JsonSerializer.Serialize(model, jsonSerializerOptions);
        return !string.IsNullOrWhiteSpace(json) &&
               CompareToOriginalPropertyMap(state, json);
    }

    private static bool CompareToOriginalPropertyMap(ModelState state, string serializedJson)
    {
        var serializedNode = JsonNode.Parse(serializedJson);
        var serializedProperties = new HashSet<string>();

        if (serializedNode is JsonObject jsonObject)
        {
            serializedProperties = [.. jsonObject.Select(p => p.Key)];
        }

        return state.PropertyMap.Keys.All(k => serializedProperties.Contains(k.Split('.').Last()));
    }
}