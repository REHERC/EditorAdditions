using Harmony;
using LevelEditorTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(GUtils), "GetExportedTypesOfType")]
    internal static class GetExportedTypesOfType
    {
        internal static void Postfix(Type baseType, ref Type[] __result)
        {
            List<Type> types = __result.ToList();

            if (new List<Type>() {
                typeof(ISerializable),
                typeof(LevelEditorTool),
                typeof(AddedComponent)
            }.Contains(baseType))
            {
                types.AddRange(GetTypesOfType(baseType));
            }
            __result = types.ToArray();
        }

        internal static List<Type> GetTypesOfType(Type baseType)
        {
            List<Type> result = new List<Type>();

            foreach (Type type in typeof(Plugin).Assembly.GetTypes())
            {
                if (type.IsSubclassOf(baseType))
                {
                    result.Add(type);
                }
            }

            return result;
        }
    }
}
