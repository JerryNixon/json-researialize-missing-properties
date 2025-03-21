using System.Runtime.CompilerServices;

namespace Library.Models;

public static class ModelStateManager
{
    private static readonly ConditionalWeakTable<object, ModelState> StateMap = [];

    public static ModelState GetState<T>(this T model) where T : class
    {
        ArgumentNullException.ThrowIfNull(model);

        return StateMap.GetOrCreateValue(model);
    }

    public static void SetState<T>(this T model, ModelState state)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(state);

        StateMap.Remove(model);
        StateMap.Add(model, state);
    }
}
