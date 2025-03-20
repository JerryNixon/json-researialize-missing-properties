using System.Text.Json.Nodes;
using NJsonSchema;
using NJsonSchema.Validation;

namespace Library.Json;

public static class JsonExtensions
{
   private static readonly JsonSchema Schema;

   static JsonExtensions()
   {
       string schemaJson = File.ReadAllText("schema.json");
       Schema = JsonSchema.FromJsonAsync(schemaJson).GetAwaiter().GetResult();
   }

   public static bool CompliesWithSchema(this JsonNode json)
   {
       var validator = new JsonSchemaValidator();
       var results = validator.Validate(json.ToJsonString(), Schema);
       return results.Count == 0;
   }

   public static bool CompliesWithSchema(this string json)
   {
       var validator = new JsonSchemaValidator();
       var results = validator.Validate(json, Schema);
       return results.Count == 0;
   }
}