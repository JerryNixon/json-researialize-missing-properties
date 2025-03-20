using System.Text.Json.Nodes;

namespace Library;

// Interface for models that can track their original JSON structure
public interface ITrackableModel
{
    JsonNode? OriginalJsonStructure { get; set; }
    bool IsSpecialCase { get; set; }
}

// Interface for models that can track which properties were present in the original JSON
public interface IPropertyTrackingModel
{
    void InitializeFromPropertyMap(Dictionary<string, bool> propertyMap);
}