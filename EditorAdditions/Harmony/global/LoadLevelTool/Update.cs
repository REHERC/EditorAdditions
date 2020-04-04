using Harmony;
using LevelEditorTools;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(LoadLevelTool), "Update")]
    internal static class UpdateLoadLevelTool
    {
        internal static void Prefix()
        {
            EditorToolset.ClearQuickMemory();
        }
    }
}
