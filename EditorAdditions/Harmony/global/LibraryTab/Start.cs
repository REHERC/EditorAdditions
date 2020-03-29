#pragma warning disable IDE0051, RCS1213
using Harmony;
using UnityEngine;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(LibraryTab), "Start")]
    public static class StartLibraryTab
    {
        [HarmonyPrefix]
        public static bool Prefix(LibraryTab __instance)
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
}