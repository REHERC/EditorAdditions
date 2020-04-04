using Harmony;
using LevelEditorTools;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(GroupTool), "Run")]
    internal static class RunGroupTool
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
