using System.Text.Json;
using FluentAssertions;
using Library.Json;
using Library.Models;

namespace Library.Tests
{
   public class JsonReaderWriterTests
   {
       [Fact]
       public void Write_ShouldFailWhenBothOriginalJsonAndSerializationFail()
       {
           var mockModel = new { NonSerializableProperty = new object() };

           bool success = JsonWriter.TryWrite(mockModel, out string _);
           success.Should().BeFalse();
       }

       [Theory]
       [InlineData("Sample1", false, true, 100)]
       [InlineData("Sample2", false, true, 100)]
       [InlineData("Sample5", true, false, 42)]
       [InlineData("Sample6", true, true, 200)]
       public void Read_ShouldMatchExpectedValues(string sampleName, bool expectedRequiredValue,
           bool expectedOptional, int expectedNestedValue)
       {
           var json = (string)typeof(Samples).GetProperty(sampleName)!.GetValue(null)!;

           var success = JsonReader.TryRead(json, out Model model);

           success.Should().BeTrue();
           model.Should().NotBeNull();
           model!.RequiredProperty.Value.Should().Be(expectedRequiredValue);
           model.OptionalProperty.Should().Be(expectedOptional);
           model.RequiredProperty.NestedProperty.Should().Be(expectedNestedValue);
       }

       [Theory]
       [InlineData("Sample3")]
       [InlineData("Sample7")]
       public void Read_ShouldFailForInvalidJson(string sampleName)
       {
           var json = (string)typeof(Samples).GetProperty(sampleName)!.GetValue(null)!;

           var success = JsonReader.TryRead(json, out Model model);

           success.Should().BeFalse();
           model.Should().BeNull();
       }

       [Theory]
       [InlineData("Sample1")]
       [InlineData("Sample2")]
       [InlineData("Sample5")]
       [InlineData("Sample6")]
       public void ReadWrite_ShouldPreserveOriginalData(string sampleName)
       {
           var json = (string)typeof(Samples).GetProperty(sampleName)!.GetValue(null)!;
           var success = JsonReader.TryRead(json, out Model model);
           success.Should().BeTrue();
           model.Should().NotBeNull();

           bool writeSuccess = JsonWriter.TryWrite(model!, out string serializedJson);

           writeSuccess.Should().BeTrue();
           serializedJson.Should().NotBeNullOrEmpty();

           var expectedModel = JsonSerializer.Deserialize<Model>(json);
           var actualModel = JsonSerializer.Deserialize<Model>(serializedJson);

           actualModel.Should().BeEquivalentTo(expectedModel, options => options
               .ComparingByMembers<Model>()
               .ComparingByMembers<RequiredPropertyModel>());
       }

       [Fact]
       public void Write_ShouldFailForNullModel()
       {
           JsonWriter.TryWrite<object>(null!, out string _).Should().BeFalse();
       }
   }
}