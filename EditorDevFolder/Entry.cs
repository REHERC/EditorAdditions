using Harmony;
using Spectrum.API.Configuration;
using Spectrum.API.GUI.Controls;
using Spectrum.API.GUI.Data;
using Spectrum.API.Interfaces.Plugins;
using Spectrum.API.Interfaces.Systems;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace EditorDevFolder
{
    public class Plugin : IPlugin
    {
        public static Settings Configuration = new Settings("Config");

        public void Initialize(IManager manager, string ipcIdentifier)
        {
            foreach (KeyValuePair<string, object> item in new Dictionary<string, object>()
            {
                {"DevFolderEnabled", false}
            })
                if (!Configuration.ContainsKey<string>(item.Key))
                    Configuration[item.Key] = item.Value;
            Configuration.Save();

            HarmonyInstance harmony = HarmonyInstance.Create("com.REHERC.EditorDevFolder");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            manager.Menus.AddMenu(MenuDisplayMode.MainMenu, new MenuTree("editordevmode.main", "LEVEL EDITOR DEV MODE")
            {
                new CheckBox(MenuDisplayMode.MainMenu, "editordevmode.main.enable", "ENABLE DEVLOPPER FOLDER")
                .WithGetter(() => (bool)Configuration["DevFolderEnabled"])
                .WithSetter((value) => {
                    Configuration["DevFolderEnabled"] = value;
                    Configuration.Save();
                })
                .WithDescription("Enables the Dev folder in the level editor library tab.\n[FF0000]Warning: the objects in this folder may or may not work in future updates and can potentially break your maps, use them at your own risk![-]")
            }, "Settings for the Editor Dev Folder plugin.");
        }
    }

    [HarmonyPatch(typeof(LibraryTab), "Start")]
    public class LibraryTab__Start__Patch
    {
        [HarmonyPrefix]
        public static bool Prefix (LibraryTab __instance)
        {
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
}