using Harmony;
using LevelEditorTools;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(CreateCustomObjectTool), "Start")]
    internal static class StartCreateCustomObjectTool
    {
        internal static bool Prefix()
        {
            if (!EditorToolset.IsSelectionRoot())
            {
                EditorToolset.PrintToolInspectionStackError();
                return false;
            }
            return true;
        }
    }
}
