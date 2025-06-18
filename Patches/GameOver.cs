using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ApPac256
{
    public static class GameOver
    {
        /// <summary>
        /// If the option to disable gifts is applied, and somehow you can click the free energy button, disable it
        /// </summary>
        [HarmonyPatch(typeof(Panel_GameOver), "OnClickedFreeEnergy")]
        class PatchDisableFreeGiftClick
        {
            static bool Prefix()
            {
                return !ArchipelagoManager.DisableFreeGifts;
            }
        }

        /// <summary>
        /// Make the free gift UI invisible
        /// </summary>
        [HarmonyPatch(typeof(Panel_GameOver), "Toggle")]
        class PatchDisableFreeGiftUI
        {
            static void Prefix()
            {
                Panel_GameOver.inst.freeEnergyRoot.transform.Find("message_Console").gameObject.SetActive(false);
                Panel_GameOver.inst.freeEnergyRoot.transform.Find("Button_GetFreeEnergy").gameObject.SetActive(false);
            }
        }
    }
}
