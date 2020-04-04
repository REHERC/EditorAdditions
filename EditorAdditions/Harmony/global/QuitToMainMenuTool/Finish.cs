using Harmony;
using LevelEditorTools;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(QuitToMainMenuTool), "Finish")]
    internal static class FinishQuitToMainMenuTool
    {
        internal static void Prefix()
        {
            EditorToolset.ClearQuickMemory();
        }
    }
}
