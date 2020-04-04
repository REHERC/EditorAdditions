using Harmony;
using LevelEditorTools;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(GenerateTrackmogrifyLevelTool), "Finish")]
    internal static class FinishGenerateTrackmogrifyLevelTool
    {
        internal static void Prefix()
        {
            EditorToolset.ClearQuickMemory();
        }
    }
}
