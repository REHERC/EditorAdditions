#pragma warning disable IDE0051, RCS1213
using Harmony;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(AudioManager.MusicCue), MethodType.Constructor)]
    public static class ConstructMusicCue
    {
        private static void Postfix(AudioManager.MusicCue __instance)
        {
            if (__instance.devEvent_)
            {
                Globals.DevMusic.Add(__instance);
            }
        }
    }
}