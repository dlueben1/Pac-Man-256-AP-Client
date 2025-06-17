using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApPac256
{
    public static class SpawnItem
    {
        /// <summary>
        /// Allows all currently unlocked power-ups as spawnable
        /// </summary>
        [HarmonyPatch(typeof(LM), "GetRandomPowerup")]
        class PatchSpawnPowerup
        {
            static void Prefix()
            {
                GM.inst.currentSaveData.currentPowerUpLoadout = new List<int>{4, 5, 8, 9, 10, 11, 13, 16};
            }
        }

        [HarmonyPatch(typeof(SaveData), "CanDropPowerUp")]
        class PatchAllowPowerupToDrop1
        {
            static void Postfix(ref bool __result)
            {
                __result = true;
            }
        }

        [HarmonyPatch(typeof(SaveData), "IsPowerUpUnlocked")]
        class PatchAllowPowerupToDrop2
        {
            static void Postfix(ref bool __result)
            {
                __result = true;
            }
        }
    }
}
