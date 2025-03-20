using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

namespace Library;

public class ModelStateTracker<T> where T : class
{
   private static readonly ConditionalWeakTable<T, ModelState> StateMap = [];

   public static ModelState GetState(T model)
   {
       ArgumentNullException.ThrowIfNull(model);
       
       return StateMap.GetOrCreateValue(model);
   }

   public static void SetState(T model, ModelState state)
   {
       ArgumentNullException.ThrowIfNull(model);
       ArgumentNullException.ThrowIfNull(state);

       StateMap.Remove(model);
       StateMap.Add(model, state);
   }
}

public class ModelState
{
   public JsonNode? OriginalJsonStructure { get; set; }
   public Dictionary<string, bool> PropertyMap { get; set; } = [];
}