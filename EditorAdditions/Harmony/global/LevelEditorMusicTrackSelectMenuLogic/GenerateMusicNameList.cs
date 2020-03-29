#pragma warning disable IDE0051, RCS1213
using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(LevelEditorMusicTrackSelectMenuLogic), "GenerateMusicNameList")]
    public static class GenerateMusicNameList
    {
        private static void Postfix(LevelEditorMusicTrackSelectMenuLogic __instance)
        {
            if (Plugin.Configuration.GetItem<bool>("AdvancedMusicSelection"))
            {
                __instance.buttonList_.Clear();
                List<AudioManager.MusicCue> music = G.Sys.AudioManager_.MusicCues_;
                music.AddRange(Globals.DevMusic);
                __instance.CreateButtons(music, Color.white);
                __instance.buttonList_.SortAndUpdateVisibleButtons();
            }
        }
    }
}