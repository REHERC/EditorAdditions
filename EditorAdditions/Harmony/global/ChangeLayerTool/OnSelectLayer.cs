using Harmony;
using LevelEditorTools;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(ChangeLayerTool), "OnSelectLayer")]
    internal static class OnSelectLayer
    {
        internal static bool Prefix(ChangeLayerTool __instance)
        {
            if (!EditorToolset.IsSelectionRoot())
            {
                EditorToolset.PrintToolInspectionStackError();
                __instance.state_ = ToolState.Cancelled;
                return false;
            }
            return true;
        }
    }
}
