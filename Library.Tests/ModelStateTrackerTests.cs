using System.Text.Json.Nodes;
using FluentAssertions;

namespace Library.Tests;

public class ModelStateTrackerTests
{
    [Fact]
    public void GetState_ShouldThrowForNullModel()
    {
        // Arrange
        SampleModel? nullModel = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ModelStateTracker<SampleModel>.GetState(nullModel!));
    }

    [Fact]
    public void GetState_ShouldReturnSameStateForSameModel()
    {
        // Arrange
        var model = new SampleModel();

        // Act
        var state1 = ModelStateTracker<SampleModel>.GetState(model);
        var state2 = ModelStateTracker<SampleModel>.GetState(model);

        // Assert
        state1.Should().BeSameAs(state2);
    }

    [Fact]
    public void SetState_ShouldUpdateModelState()
    {
        // Arrange
        var model = new SampleModel();
        var originalState = new ModelState
        {
            OriginalJsonStructure = JsonNode.Parse("{}"),
            PropertyMap = new Dictionary<string, bool> { ["test"] = true }
        };

        // Act
        ModelStateTracker<SampleModel>.SetState(model, originalState);
        var retrievedState = ModelStateTracker<SampleModel>.GetState(model);

        // Assert
        retrievedState.Should().Be(originalState);
    }

    [Fact]
    public void SetState_ShouldThrowForNullModel()
    {
        // Arrange
        SampleModel? nullModel = null;
        var state = new ModelState();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ModelStateTracker<SampleModel>.SetState(nullModel!, state));
    }

    [Fact]
    public void SetState_ShouldThrowForNullState()
    {
        // Arrange
        var model = new SampleModel();
        ModelState? nullState = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ModelStateTracker<SampleModel>.SetState(model, nullState!));
    }

    [Fact]
    public void ModelState_ShouldHaveEmptyPropertyMapByDefault()
    {
        // Arrange
        var modelState = new ModelState();

        // Assert
        modelState.PropertyMap.Should().BeEmpty();
        modelState.OriginalJsonStructure.Should().BeNull();
    }
}