using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Library.Tests
{
    public class JsonReaderWriterTests
    {
        [Theory]
        [InlineData("Sample1", false, true, 100)] // Default nested property value
        [InlineData("Sample2", false, true, 100)] // Default nested property value
        [InlineData("Sample3", true, true, 100)]  // Special case with missing required property
        [InlineData("Sample5", true, false, 42)]  // Custom nested property value
        [InlineData("Sample6", true, true, 200)]  // Custom nested property value
        public void Read_ShouldMatchExpectedValues(string sampleName, bool expectedRequiredValue, 
            bool expectedOptional, int expectedNestedValue)
        {
            // Arrange
            var json = (string)typeof(Samples).GetProperty(sampleName)!.GetValue(null)!;

            // Act
            var success = JsonReader.TryRead(json, out SampleModel? model);

            // Assert
            success.Should().BeTrue();
            model.Should().NotBeNull();
            model!.RequiredProperty.Value.Should().Be(expectedRequiredValue);
            model.OptionalProperty.Should().Be(expectedOptional);
            model.RequiredProperty.NestedProperty.Should().Be(expectedNestedValue);
        }
        
        [Fact]
        public void Read_ShouldFailForMissingRequiredValue()
        {
            // Arrange
            var json = Samples.Sample7; // Missing required-property.value

            // Act
            var success = JsonReader.TryRead(json, out SampleModel? model);

            // Assert
            success.Should().BeFalse();
            model.Should().BeNull();
        }

        [Theory]
        [InlineData("Sample1")]
        [InlineData("Sample2")]
        [InlineData("Sample3")] // Special case
        [InlineData("Sample4")]
        [InlineData("Sample5")]
        [InlineData("Sample6")]
        public void ReadWrite_ShouldPreserveOriginalFormat(string sampleName)
        {
            // Arrange
            var json = (string)typeof(Samples).GetProperty(sampleName)!.GetValue(null)!;
            var success = JsonReader.TryRead(json, out SampleModel? model);
            success.Should().BeTrue();
            model.Should().NotBeNull();

            // Act
            string serializedJson = JsonWriter.Write(model!);
            
            // Normalize both JSONs for comparison (remove whitespace differences)
            var expectedDict = JsonSerializer.Deserialize<JsonDocument>(json).RootElement;
            var actualDict = JsonSerializer.Deserialize<JsonDocument>(serializedJson).RootElement;

            // Assert - compare structure and values
            JsonElementComparer.Compare(expectedDict, actualDict).Should().BeTrue();
        }
    }
    
    // Helper class for comparing JsonElements
    public static class JsonElementComparer
    {
        public static bool Compare(JsonElement expected, JsonElement actual)
        {
            if (expected.ValueKind != actual.ValueKind)
                return false;
                
            switch (expected.ValueKind)
            {
                case JsonValueKind.Object:
                    return CompareObjects(expected, actual);
                case JsonValueKind.Array:
                    return CompareArrays(expected, actual);
                case JsonValueKind.String:
                    return expected.GetString() == actual.GetString();
                case JsonValueKind.Number:
                    return expected.GetRawText() == actual.GetRawText();
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return expected.GetBoolean() == actual.GetBoolean();
                case JsonValueKind.Null:
                    return true;
                default:
                    return false;
            }
        }
        
        private static bool CompareObjects(JsonElement expected, JsonElement actual)
        {
            foreach (var property in expected.EnumerateObject())
            {
                if (!actual.TryGetProperty(property.Name, out var actualProp))
                    return false;
                    
                if (!Compare(property.Value, actualProp))
                    return false;
            }
            
            foreach (var property in actual.EnumerateObject())
            {
                if (!expected.TryGetProperty(property.Name, out _))
                    return false;
            }
            
            return true;
        }
        
        private static bool CompareArrays(JsonElement expected, JsonElement actual)
        {
            if (expected.GetArrayLength() != actual.GetArrayLength())
                return false;
                
            var expectedArray = expected.EnumerateArray().ToArray();
            var actualArray = actual.EnumerateArray().ToArray();
            
            for (int i = 0; i < expectedArray.Length; i++)
            {
                if (!Compare(expectedArray[i], actualArray[i]))
                    return false;
            }
            
            return true;
        }
    }
}