using System.Reflection;
using Mono.Reflection;

namespace Plugin.Helpers
{
    internal class ReadonlyModifier
    {
        public static void SetValue(object Root, string Property, object Value)
        {
            PropertyInfo PI = Root.GetType().GetProperty(Property);
            FieldInfo FI = PI.GetBackingField();
            FI.SetValue(Root, Value);
        }
    }
}
