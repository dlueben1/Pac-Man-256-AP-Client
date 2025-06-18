using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApPac256
{
    public static class DisableProgression
    {
        /// <summary>
        /// Disables unlocking Steam Achievements when the conditions are met
        /// </summary>
        [HarmonyPatch]
        public class PatchDisableUnlockingSteamAchievement1
        {
            static MethodBase TargetMethod()
            {
                var type = AccessTools.TypeByName("SteamManager");
                return AccessTools.Method(type, "SetAchievementIndividual");
            }

            static bool Prefix()
            {
                // Skips the function that sets the Steam Achievement
                return false;
            }
        }

        /// <summary>
        /// Disables unlocking Steam Achievements when a file is loaded
        /// </summary>
        [HarmonyPatch]
        public class PatchDisableUnlockingSteamAchievement2
        {
            static MethodBase TargetMethod()
            {
                var type = AccessTools.TypeByName("SteamManager");
                return AccessTools.Method(type, "SetAchievementsFromLoad");
            }

            static bool Prefix()
            {
                // Skips the function that sets the Steam Achievement
                return false;
            }
        }

        [HarmonyPatch(typeof(Panel_PowerUpProgression), "Process")]
        public class PatchDisablePowerupProgressionUI
        {
            static bool Prefix()
            {
                Panel_GameOver.inst.Toggle(state: true);
                return false;
            }
        }

        [HarmonyPatch]
        public class PatchDisableSteamLeaderboard1
        {
            static MethodBase TargetMethod()
            {
                var type = AccessTools.TypeByName("SteamManager");
                return AccessTools.Method(type, "SetStat");
            }

            static bool Prefix()
            {
                // Skips the function that sets the Steam Achievement
                return false;
            }
        }


        [HarmonyPatch]
        public class PatchDisableSteamLeaderboard2
        {
            static MethodBase TargetMethod()
            {
                var type = AccessTools.TypeByName("SteamManager");
                return AccessTools.Method(type, "UploadMost256");
            }

            static bool Prefix()
            {
                // Skips the function that sets the Steam Achievement
                return false;
            }
        }

        [HarmonyPatch]
        public class PatchDisableSteamLeaderboard3
        {
            static MethodBase TargetMethod()
            {
                var type = AccessTools.TypeByName("SteamManager");
                return AccessTools.Method(type, "UploadHighestScore");
            }

            static bool Prefix()
            {
                // Skips the function that sets the Steam Achievement
                return false;
            }
        }

        [HarmonyPatch]
        public class PatchDisableSteamLeaderboard4
        {
            static MethodBase TargetMethod()
            {
                var type = AccessTools.TypeByName("SteamManager");
                return AccessTools.Method(type, "Hookup_Global_Leaderboard");
            }

            static bool Prefix()
            {
                // Skips the function that sets the Steam Achievement
                return false;
            }
        }

        [HarmonyPatch]
        public class PatchDisableSteamLeaderboard5
        {
            static MethodBase TargetMethod()
            {
                var type = AccessTools.TypeByName("SteamManager");
                return AccessTools.Method(type, "Hookup256_Leaderboard");
            }

            static bool Prefix()
            {
                // Skips the function that sets the Steam Achievement
                return false;
            }
        }

        [HarmonyPatch]
        public class PatchDisableSteamLeaderboard6
        {
            static MethodBase TargetMethod()
            {
                var type = AccessTools.TypeByName("SteamManager");
                return AccessTools.Method(type, "HookupHighScore_Leaderboard");
            }

            static bool Prefix()
            {
                // Skips the function that sets the Steam Achievement
                return false;
            }
        }
    }
}
