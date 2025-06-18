using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApPac256
{
    public static class DisableSaveData
    {
        [HarmonyPatch(typeof(FileIO), "LoadGameData")]
        class PatchDisableLoadingSaveData
        {
            static bool Prefix(string identifier, string fn, Action<string> callback)
            {
                // Skip to what we do when the load fails
                callback(null);

                // Then skip the rest of this function
                return false;
            }
        }

        [HarmonyPatch(typeof(GM), "SaveGame")]
        class PatchDisableWritingSaveData
        {
            static bool Prefix()
            {
                // Skip the function
                return false;
            }
        }

        [HarmonyPatch(typeof(GM), "MergeData")]
        class PatchDisableMergingSaveData
        {
            static bool Prefix()
            {
                // Skip the function
                return false;
            }
        }

        [HarmonyPatch(typeof(GM), "LoadGame_Complete")]
        class PatchSkipNewGameIntro
        {
            static void Postfix(GM __instance)
            {
                __instance.currentSaveData.acceptEULA = true;
            }
        }
    }
}
