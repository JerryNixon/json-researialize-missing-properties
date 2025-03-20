using System.Text.Json.Nodes;

namespace Library.Models;

public class ModelState
{
    public JsonNode? OriginalJsonStructure { get; set; }
    public Dictionary<string, bool> PropertyMap { get; set; } = [];
}
