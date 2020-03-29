#pragma warning disable IDE0051, RCS1213
using Harmony;
using LevelEditorTools;
using System.Collections.Generic;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(SelectMusicTrackNameFromListTool), "AddEntries")]
    public static class AddSelectMusicTrackNameFromListToolEntries
    {
        private static bool Prefix(ref Dictionary<string, string> entryList)
        {
            if (Plugin.Configuration.GetItem<bool>("AdvancedMusicSelection"))
            {
                foreach (AudioManager.MusicCue music in G.Sys.AudioManager_.MusicCues_)
                {
                    entryList.Add(music.displayName_, music.displayName_);
                }

                return false;
            }
            return true;
        }
    }
}