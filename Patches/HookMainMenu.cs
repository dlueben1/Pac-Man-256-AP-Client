using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApPac256
{
    public static class HookMainMenu
    {
        /// <summary>
        /// This is called when the Main Menu is being shown, we use it to display the Archipelago Main Menu option
        /// </summary>
        [HarmonyPatch(typeof(Panel_PreGame), "Show")]
        class PatchOnMainMenuShow
        {
            static void Postfix()
            {
                ArchipelagoUIMainMenu.inst.TryCreateArchipelagoMenuButton();
            }
        }
    }
}
