using System.Text.Json.Nodes;
using System.Text.Json;

namespace Library;

public class JsonReader
{
    public static bool TryRead<T>(string json, out T? model) where T : class, new()
    {
        model = null;

        if (string.IsNullOrWhiteSpace(json) ||
            !TryParseJson(json, out JsonNode jsonNode) ||
            !jsonNode.CompliesWithSchema() ||
            !TryDeserializeJson(json, out T resultModel))
        {
            return false;
        }

        TryStoreModelState(resultModel, jsonNode);
        model = resultModel;
        return true;
    }

    private static Dictionary<string, bool> GetFlattenedPropertyPaths(JsonNode node, string prefix = "")
    {
        var result = new Dictionary<string, bool>();

        if (node is JsonObject obj)
        {
            TryProcessJsonObject(obj, prefix, result);
        }

        return result;
    }

    private static bool TryDeserializeJson<T>(string json, out T model) where T : class
    {
        model = JsonSerializer.Deserialize<T>(json)!;
        return model != null;
    }

    private static bool TryParseJson(string json, out JsonNode jsonNode)
    {
        jsonNode = JsonNode.Parse(json)!;
        return jsonNode != null;
    }

    private static void TryProcessJsonObject(JsonObject obj, string prefix, Dictionary<string, bool> result)
    {
        foreach (var prop in obj)
        {
            string path = string.IsNullOrEmpty(prefix) ? prop.Key : $"{prefix}.{prop.Key}";
            result[path] = true;

            if (prop.Value is JsonObject or JsonArray)
            {
                foreach (var nested in GetFlattenedPropertyPaths(prop.Value!, path))
                {
                    result[nested.Key] = nested.Value;
                }
            }
        }
    }

    private static void TryStoreModelState<T>(T model, JsonNode jsonNode) where T : class
    {
        var modelState = new ModelState
        {
            OriginalJsonStructure = jsonNode,
            PropertyMap = GetFlattenedPropertyPaths(jsonNode)
        };

        ModelStateTracker<T>.SetState(model, modelState);
    }
}