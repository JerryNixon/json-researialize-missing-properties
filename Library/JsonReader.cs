using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Validation;

namespace Library;

public class JsonReader
{
    private static readonly JsonSerializerOptions DeserializeOptions = new()
    {
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };

    private static readonly JsonSchema Schema;

    static JsonReader()
    {
        string schemaJson = File.ReadAllText("schema.json");
        Schema = JsonSchema.FromJsonAsync(schemaJson).GetAwaiter().GetResult();
    }

    public static bool TryRead<T>(string json, out T? model) where T : class, new()
    {
        model = null;
        
        if (string.IsNullOrWhiteSpace(json))
        {
            return false;
        }

        if (!TryParseJson(json, out JsonNode jsonNode))
        {
            return false;
        }

        if (!TryValidateAgainstSchema(jsonNode, out _))
        {
            return false;
        }

        if (!TryDeserializeJson(json, out T resultModel))
        {
            return false;
        }

        TryStoreModelState(resultModel, jsonNode);
        model = resultModel;
        return true;
    }

    public static bool TryReadFromFile<T>(string filePath, out T? model) where T : class, new()
    {
        model = null;
        
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return false;
        }

        if (!File.Exists(filePath))
        {
            return false;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            return TryRead(json, out model);
        }
        catch
        {
            return false;
        }
    }
    
    private static string BuildPropertyPath(string prefix, string propertyName)
    {
        return string.IsNullOrEmpty(prefix) ? propertyName : $"{prefix}.{propertyName}";
    }
    
    private static Dictionary<string, bool> GetFlattenedPropertyPaths(JsonNode node, string prefix = "")
    {
        var result = new Dictionary<string, bool>();
        
        if (node is JsonObject obj)
        {
            TryProcessJsonObject(obj, prefix, result);
        }
        else if (node is JsonArray arr)
        {
            TryProcessJsonArray(arr, prefix, result);
        }
        
        return result;
    }

    private static bool TryDeserializeJson<T>(string json, out T model) where T : class
    {
        model = null!;
        try
        {
            var result = JsonSerializer.Deserialize<T>(json, DeserializeOptions);
            if (result == null)
            {
                return false;
            }

            model = result;
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool TryParseJson(string json, out JsonNode jsonNode)
    {
        jsonNode = null!;
        try
        {
            var result = JsonNode.Parse(json);
            if (result == null)
            {
                return false;
            }

            jsonNode = result;
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static void TryProcessJsonArray(JsonArray arr, string prefix, Dictionary<string, bool> result)
    {
        for (int i = 0; i < arr.Count; i++)
        {
            string path = $"{prefix}[{i}]";
            if (arr[i] != null)
            {
                foreach (var nested in GetFlattenedPropertyPaths(arr[i]!, path))
                {
                    result[nested.Key] = nested.Value;
                }
            }
        }
    }

    private static void TryProcessJsonObject(JsonObject obj, string prefix, Dictionary<string, bool> result)
    {
        foreach (var prop in obj)
        {
            string path = BuildPropertyPath(prefix, prop.Key);
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
    
    private static bool TryValidateAgainstSchema(JsonNode jsonNode, out ICollection<ValidationError> errors)
    {
        var validator = new JsonSchemaValidator();
        errors = validator.Validate(jsonNode.ToJsonString(), Schema);
        return errors.Count == 0;
    }
}