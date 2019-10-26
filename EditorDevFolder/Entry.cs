using Harmony;
using LevelEditorTools;
using Spectrum.API.Configuration;
using Spectrum.API.GUI.Controls;
using Spectrum.API.GUI.Data;
using Spectrum.API.Interfaces.Plugins;
using Spectrum.API.Interfaces.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EditorDevFolder
{
    public class Plugin : IPlugin
    {
        public static Settings Configuration = new Settings("Config");
        public static Spectrum.API.Logging.Logger Log = new Spectrum.API.Logging.Logger("Log")
        {
            WriteToConsole = true,
            ColorizeLines = true
        };

        public void Initialize(IManager manager, string ipcIdentifier)
        {
            foreach (KeyValuePair<string, object> item in new Dictionary<string, object>()
            {
                {"DevFolderEnabled", false},
                {"AdvancedMusicSelection", false},
                {"OpenWorkshopLevels", false},
                {"EditorIconSize", 67f}
            })
                if (!Configuration.ContainsKey<string>(item.Key))
                    Configuration[item.Key] = item.Value;
            Configuration.Save();

            
            manager.Menus.AddMenu(MenuDisplayMode.MainMenu, new MenuTree("editordevmode.main", "LEVEL EDITOR DEVELOPER MODE")
            {
                new CheckBox(MenuDisplayMode.MainMenu, "editordevmode.main.enable", "ENABLE DEV FOLDER")
                .WithGetter(() => (bool)Configuration["DevFolderEnabled"])
                .WithSetter((value) => {
                    Configuration["DevFolderEnabled"] = value;
                    Configuration.Save();
                })
                .WithDescription("Enables the Dev folder in the level editor library tab.\n[FF0000]Warning: the objects in this folder may or may not work in future updates and can potentially break your maps, use them at your own risk![-]"),
                new CheckBox(MenuDisplayMode.MainMenu, "editordevmode.main.music", "SHOW HIDDEN MUSIC NAMES")
                .WithGetter(() => (bool)Configuration["AdvancedMusicSelection"])
                .WithSetter((value) => {
                    Configuration["AdvancedMusicSelection"] = value;
                    Configuration.Save();
                })
                .WithDescription("Adds hidden music cues to the music selection menu."),
                new CheckBox(MenuDisplayMode.MainMenu, "editordevmode.main.workshop", "SHOW WORKSHOP LEVELS")
                .WithGetter(() => (bool)Configuration["OpenWorkshopLevels"])
                .WithSetter((value) => {
                    Configuration["OpenWorkshopLevels"] = value;
                    Configuration.Save();
                })
                .WithDescription("Show workshop levels in the level editor open window.")
            }, "Settings for the Editor Dev Folder plugin.");

            try
            {
                HarmonyInstance harmony = HarmonyInstance.Create("com.REHERC.EditorDev");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }
    }

    [HarmonyPatch(typeof(LibraryTab), "Start")]
    public class LibraryTab__Start__Patch
    {
        [HarmonyPrefix]
        public static bool Prefix (LibraryTab __instance)
        {
            __instance.iconSizeSlider_.onChange.Add(new EventDelegate(() => {
                Plugin.Configuration["EditorIconSize"] = __instance.IconSize_;
                Plugin.Configuration.Save();

                Plugin.Log.Info("iconSizeSlider_.onChange");
            }));

            if (Plugin.Configuration.ContainsKey("EditorIconSize"))
                __instance.iconSize_ = Plugin.Configuration.GetItem<float>("EditorIconSize");

            __instance.rootFileData_ = G.Sys.ResourceManager_.LevelPrefabFileInfosRoot_;
            if (Plugin.Configuration["DevFolderEnabled"] is false)
                __instance.rootFileData_.RemoveAllChildInfos((LevelPrefabFileInfo x) => x.IsDirectory_ && x.Name_ == "Dev");
            __instance.currentDirectory_ = __instance.rootFileData_;
            __instance.iconSizeSlider_.value = Mathf.InverseLerp(32f, 256f, __instance.iconSize_);
            __instance.searchInput_ = __instance.GetComponentInChildren<UIExInput>();
            __instance.StartCoroutine(__instance.CreateIconsAfterAFrame());
            return false;
        }
    }

    //[HarmonyPatch(typeof(SelectMusicTrackNameFromListTool), "StartTool")]
    public class SelectMusicTrackNameFromListTool__StartTool__Patch
    {
        static void Prefix(SelectMusicTrackNameFromListTool __instance)
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
                        __instance.MakeBigPopup(list.ToArray());
                    else
                        __instance.MakePopup(list.ToArray(), true);
                }
                else
                {
                    UIExFancyPopup.Show(__instance.Title_, dictionary.ToArray<KeyValuePair<string, string>>(), pathList.Values.ToArray<string>(), new System.Action<string>(__instance.OnSelect));
                    if (__instance.customClick_ != null)
                        UIExFancyPopup.SetClickMode(__instance.customClick_);
                }
            }
        }
    }

    [HarmonyPatch(typeof(SelectMusicTrackNameFromListTool), "AddEntries")]
    public class SelectMusicTrackNameFromListTool__AddEntries__Patch
    {
        static bool Prefix(SelectMusicTrackNameFromListTool __instance, ref Dictionary<string, string> entryList)
        {
            if (Plugin.Configuration.GetItem<bool>("AdvancedMusicSelection"))
            {
                foreach (AudioManager.MusicCue music in G.Sys.AudioManager_.MusicCues_)
                    entryList.Add(music.displayName_, music.displayName_);

                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(LevelEditorMusicTrackSelectMenuLogic), "GenerateMusicNameList")]
    public class LevelEditorMusicTrackSelectMenuLogic__GenerateMusicNameList__Patch
    {
        static void Postfix(LevelEditorMusicTrackSelectMenuLogic __instance)
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

    [HarmonyPatch(typeof(LevelEditorLevelNameSelectMenuLogic), "Show")]
    public class LevelEditorLevelNameSelectMenuLogic__Show__Patch
    {
        static void Prefix(LevelEditorLevelNameSelectMenuLogic __instance)
        {
            if (Plugin.Configuration.GetItem<bool>("OpenWorkshopLevels"))
                __instance.GenerateLevelNameList();
        }
    }

    [HarmonyPatch(typeof(LevelEditorLevelNameSelectMenuLogic), "GenerateLevelNameList")]
    public class LevelEditorLevelNameSelectMenuLogic__GenerateLevelNameList__Patch
    {
        static void Postfix(LevelEditorLevelNameSelectMenuLogic __instance)
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

    [HarmonyPatch(typeof(AudioManager.MusicCue), MethodType.Constructor)]
    public class AudioManager_MusicCue__ctor__Patch
    {
        static void Postfix(AudioManager.MusicCue __instance)
        {
            if (__instance.devEvent_) Globals.DevMusic.Add(__instance);
        }
    }

    public static class Globals
    {
        public static List<AudioManager.MusicCue> DevMusic = new List<AudioManager.MusicCue>();
    }
}