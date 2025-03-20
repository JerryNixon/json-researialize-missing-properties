using System.Text.Json.Serialization;

namespace Library.Models;

public class Configuration
{
   [JsonPropertyName("required-property")]
   public RequiredPropertyModel RequiredProperty { get; init; } = new();

   [JsonPropertyName("optional-property")]
   [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
   public bool OptionalProperty { get; init; } = true;
}

public class RequiredPropertyModel
{
   [JsonPropertyName("value")]
   public bool Value { get; init; }
   
   [JsonPropertyName("nested-property")]
   [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
   public int NestedProperty { get; init; } = 100;
}