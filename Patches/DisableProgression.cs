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
        public class DisableUnlockingSteamAchievement1
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
        public class DisableUnlockingSteamAchievement2
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

        [HarmonyPatch]
        public class DisableSteamLeaderboard1
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
        public class DisableSteamLeaderboard2
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
        public class DisableSteamLeaderboard3
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
        public class DisableSteamLeaderboard4
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
        public class DisableSteamLeaderboard5
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
        public class DisableSteamLeaderboard6
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
