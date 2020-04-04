using Harmony;
using LevelEditorTools;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(UngroupTool), "Run")]
    internal static class RunUngroupTool
    {
        internal static bool Prefix(ref bool __result)
        {
            if (!EditorToolset.IsSelectionRoot())
            {
                EditorToolset.PrintToolInspectionStackError();
                __result = false;
                return false;
            }
            return true;
        }
    }
}
