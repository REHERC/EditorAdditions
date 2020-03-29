#pragma warning disable CA1031, CA1052, IDE0051, IDE0060, RCS1213

using Harmony;
using Spectrum.API.Configuration;
using Spectrum.API.GUI.Controls;
using Spectrum.API.GUI.Data;
using Spectrum.API.Interfaces.Plugins;
using Spectrum.API.Interfaces.Systems;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EditorAdditions
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
            {
                if (!Configuration.ContainsKey<string>(item.Key))
                {
                    Configuration[item.Key] = item.Value;
                }
            }

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

    public static class Globals
    {
        public static List<AudioManager.MusicCue> DevMusic { get; set; } = new List<AudioManager.MusicCue>();
    }
}