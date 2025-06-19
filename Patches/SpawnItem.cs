using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
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
                // Allow any of our Archipelago Items to Spawn
                GM.inst.currentSaveData.currentPowerUpLoadout = ArchipelagoManager.Loadout;

                // Prevent spawning the "next locked item" if not unlocked
                GM.inst.currentSaveData.nextUnlockCost = -1;
                GM.inst.currentSaveData.nextUnlockType = -1;
            }

            static void Postfix(Pickup __result)
            {
                Plugin.Logger.LogMessage($"PROVIDED PICKUP: {__result?.name ?? "BLOCKED"}");
            }
        }

        [HarmonyPatch(typeof(SaveData), "CanDropPowerUp")]
        class PatchAllowPowerupToDrop1
        {
            static void Postfix(PowerupType t, ref bool __result)
            {
                // Allow Power-Pellets and None
                if(t == PowerupType.PowerDot || t == PowerupType.None)
                {
                    __result = true;
                }
                else
                {
                    __result = ArchipelagoManager.PowerUps.Count == 0 ? false : ArchipelagoManager.PowerUps.ContainsKey(t);
                }
            }
        }

        [HarmonyPatch(typeof(SaveData), "IsPowerUpUnlocked")]
        class PatchAllowPowerupToDrop2
        {
            static void Postfix(PowerupType t, ref bool __result)
            {
                // Allow Power-Pellets and None
                if (t == PowerupType.PowerDot || t == PowerupType.None)
                {
                    __result = true;
                }
                else
                {
                    __result = ArchipelagoManager.PowerUps.Count == 0 ? false : ArchipelagoManager.PowerUps.ContainsKey(t);
                }
            }
        }

        [HarmonyPatch(typeof(SaveData), "GetPowerUpDetails")]
        class PatchGetPowerupLevel
        {
            static void Postfix(PowerupType t, ref PowerUpDetails __result)
            {
                bool haveUnlocked = ArchipelagoManager.PowerUps.ContainsKey(t);
                __result = new PowerUpDetails
                {
                    PowerUpType = haveUnlocked ? t : PowerupType.None,
                    unlocked = haveUnlocked,
                    level = haveUnlocked ? ArchipelagoManager.PowerUps[t] : 0
                };
                Plugin.Logger.LogInfo($"GetDetails: P {t} / U {haveUnlocked} / L {__result.level}");
            }
        }
    }
}
