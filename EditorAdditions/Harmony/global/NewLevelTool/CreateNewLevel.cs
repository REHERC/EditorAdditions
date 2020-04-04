using Harmony;
using LevelEditorTools;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(NewLevelTool), "CreateNewLevel")]
    internal static class CreateNewLevel
    {
        internal static void Prefix()
        {
            EditorToolset.ClearQuickMemory();
        }
    }
}
