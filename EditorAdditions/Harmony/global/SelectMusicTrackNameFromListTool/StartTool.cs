#pragma warning disable IDE0051, RCS1213
using Harmony;
using LevelEditorTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EditorAdditions.Harmony
{
    //[HarmonyPatch(typeof(SelectMusicTrackNameFromListTool), "StartTool")]
    public static class StartSelectMusicTrackNameFromListTool
    {
        private static void Prefix(SelectMusicTrackNameFromListTool __instance)
        {
            if (Plugin.Configuration.GetItem<bool>("AdvancedMusicSelection"))
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                Dictionary<string, string> pathList = new Dictionary<string, string>();
                __instance.AddEntries(dictionary);
                __instance.AddPaths(pathList);
                if (dictionary.Count == 0)
                {
                    LevelEditorTool.PrintErrorMessage(__instance.NoEntriesError_);
                }
                if (pathList.Count == 0)
                {
                    List<KeyValuePair<string, string>> list = SelectStringFromListTool.OrderByAlphaNumeric<KeyValuePair<string, string>>((IEnumerable<KeyValuePair<string, string>>)dictionary.ToList<KeyValuePair<string, string>>(), (Func<KeyValuePair<string, string>, string>)(s => s.Key)).ToList<KeyValuePair<string, string>>();
                    if (list.Count > 10)
                    {
                        __instance.MakeBigPopup(list.ToArray());
                    }
                    else
                    {
                        __instance.MakePopup(list.ToArray(), true);
                    }
                }
                else
                {
                    UIExFancyPopup.Show(__instance.Title_, dictionary.ToArray<KeyValuePair<string, string>>(), pathList.Values.ToArray<string>(), new System.Action<string>(__instance.OnSelect));
                    if (__instance.customClick_ != null)
                    {
                        UIExFancyPopup.SetClickMode(__instance.customClick_);
                    }
                }
            }
        }
    }
}