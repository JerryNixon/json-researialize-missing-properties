using System.Text.Json.Serialization;

namespace Library;

public class SampleModel
{
    [JsonPropertyName("required-property")]
    public RequiredPropertyModel RequiredProperty { get; set; } = new();

    [JsonPropertyName("optional-property")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool OptionalProperty { get; set; } = true;
}

public class RequiredPropertyModel
{
    [JsonPropertyName("value")]
    public bool Value { get; set; }
    
    [JsonPropertyName("nested-property")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int NestedProperty { get; set; } = 100;
}