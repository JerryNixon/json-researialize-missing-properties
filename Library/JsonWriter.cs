using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Library;

public class JsonWriter
{
    public static string Write<T>(T model) where T : ITrackableModel
    {
        // If we have the original JSON structure, use it as a template
        if (model.OriginalJsonStructure != null)
        {
            // Simply return the original structure - it's already been validated
            // and contains only the properties that were present in the original JSON
            return model.OriginalJsonStructure.ToJsonString();
        }
        
        // Fallback to standard serialization with default options
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            WriteIndented = false
        };
        
        return JsonSerializer.Serialize(model, options);
    }
    
    public static string WriteToFile<T>(T model, string filePath) where T : ITrackableModel
    {
        string json = Write(model);
        File.WriteAllText(filePath, json);
        return json;
    }
}