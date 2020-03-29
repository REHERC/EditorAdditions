#pragma warning disable IDE0051, RCS1213
using Harmony;
using System.Linq;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(LevelEditorLevelNameSelectMenuLogic), "GenerateLevelNameList")]
    public static class GenerateLevelNameList
    {
        private static void Postfix(LevelEditorLevelNameSelectMenuLogic __instance)
        {
            if (Plugin.Configuration.GetItem<bool>("OpenWorkshopLevels"))
            {
                LevelSetsManager levelSets = G.Sys.LevelSets_;

                __instance.CreateButtons(levelSets.LevelsLevelFilePaths_.ToList<string>(), Colors.YellowColors.gold, LevelEditorLevelNameSelectMenuLogic.LevelPathEntry.DisplayOption.RelativePath);
                __instance.CreateButtons(levelSets.WorkshopLevelFilePaths_.ToList<string>(), GConstants.communityLevelColor_, LevelEditorLevelNameSelectMenuLogic.LevelPathEntry.DisplayOption.LevelName);

                __instance.buttonList_.SortAndUpdateVisibleButtons();
            }
        }
    }
}