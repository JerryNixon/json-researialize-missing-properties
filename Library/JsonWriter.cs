using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Library;

public class JsonWriter
{
    private static readonly JsonSerializerOptions SerializeOptions = new()
    {
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        WriteIndented = false
    };

    public static bool TryWrite<T>(T model, out string json) where T : class
    {
        json = string.Empty;
        
        if (model == null)
        {
            return false;
        }

        try
        {
            if (TryGetOriginalJson(model, out string originalJson))
            {
                json = originalJson;
                return true;
            }
            
            if (TrySerializeModel(model, out string serializedJson))
            {
                json = serializedJson;
                return true;
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
    
    public static bool TryWriteToFile<T>(T model, string filePath) where T : class
    {
        if (model == null || string.IsNullOrWhiteSpace(filePath))
        {
            return false;
        }

        try
        {
            if (!TryWrite(model, out string json))
            {
                return false;
            }

            File.WriteAllText(filePath, json);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private static bool TryGetOriginalJson<T>(T model, out string json) where T : class
    {
        json = string.Empty;
        
        var state = ModelStateTracker<T>.GetState(model);
        if (state.OriginalJsonStructure == null)
        {
            return false;
        }

        try
        {
            json = state.OriginalJsonStructure.ToJsonString();
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private static bool TrySerializeModel<T>(T model, out string json) where T : class
    {
        json = string.Empty;
        
        try
        {
            json = JsonSerializer.Serialize(model, SerializeOptions);
            return !string.IsNullOrWhiteSpace(json);
        }
        catch
        {
            return false;
        }
    }
}