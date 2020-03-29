#pragma warning disable IDE0051, RCS1213
using Harmony;
using System;

namespace EditorAdditions.Harmony
{
    [HarmonyPatch(typeof(AudioManager.MusicCue), MethodType.Constructor, new Type[] { typeof(MusicCueID), typeof(uint), typeof(string), typeof(bool) })]
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