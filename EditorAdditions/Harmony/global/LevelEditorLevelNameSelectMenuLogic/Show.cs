#pragma warning disable IDE0051, RCS1213
using Harmony;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(LevelEditorLevelNameSelectMenuLogic), "Show")]
    public static class ShowLevelEditorLevelNameSelectMenuLogic
    {
        private static void Prefix(LevelEditorLevelNameSelectMenuLogic __instance)
        {
            if (Plugin.Configuration.GetItem<bool>("OpenWorkshopLevels"))
            {
                __instance.GenerateLevelNameList();
            }
        }
    }
}