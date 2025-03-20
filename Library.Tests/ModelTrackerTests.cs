using System.Text.Json.Nodes;
using FluentAssertions;
using Library.Models;

namespace Library.Tests;

public class ModelTrackerTests
{
    [Fact]
    public void GetState_ShouldThrowForNullModel()
    {
        Configuration? nullModel = null;

        Assert.Throws<ArgumentNullException>(() => 
            nullModel!.GetState());
    }

    [Fact]
    public void GetState_ShouldReturnSameStateForSameModel()
    {
        var model = new Configuration();

        var state1 = model.GetState();
        var state2 = model.GetState();

        state1.Should().BeSameAs(state2);
    }

    [Fact]
    public void SetState_ShouldUpdateModelState()
    {
        var model = new Configuration();
        var originalState = new ModelState
        {
            OriginalJsonStructure = JsonNode.Parse("{}"),
            PropertyMap = new Dictionary<string, bool> { ["test"] = true }
        };

        model.SetState(originalState);
        var retrievedState = model.GetState();

        retrievedState.Should().Be(originalState);
    }

    [Fact]
    public void SetState_ShouldThrowForNullModel()
    {
        Configuration? nullModel = null;
        var state = new ModelState();

        Assert.Throws<ArgumentNullException>(() => 
            nullModel!.SetState(state));
    }

    [Fact]
    public void SetState_ShouldThrowForNullState()
    {
        var model = new Configuration();
        ModelState? nullState = null;

        Assert.Throws<ArgumentNullException>(() => 
            model.SetState(nullState!));
    }

    [Fact]
    public void ModelState_ShouldHaveEmptyPropertyMapByDefault()
    {
        var modelState = new ModelState();

        modelState.PropertyMap.Should().BeEmpty();
        modelState.OriginalJsonStructure.Should().BeNull();
    }
}

