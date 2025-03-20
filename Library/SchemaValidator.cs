using System.Text.Json.Nodes;
using NJsonSchema;
using NJsonSchema.Validation;

namespace Library;

public static class SchemaValidator
{
    private static readonly JsonSchema Schema;

    static SchemaValidator()
    {
        string schemaJson = File.ReadAllText("schema.json");
        Schema = JsonSchema.FromJsonAsync(schemaJson).GetAwaiter().GetResult();
    }

    public static bool Validate(JsonNode json)
    {
        // No special handling - if required property is missing, validation should fail
        
        var validator = new JsonSchemaValidator();
        var results = validator.Validate(json.ToJsonString(), Schema);

        return results.Count == 0; // If no errors, it's valid
    }
}