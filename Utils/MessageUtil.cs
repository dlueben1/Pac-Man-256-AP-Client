using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ApPac256
{
    public static class MessageUtil
    {
        /// <summary>
        /// Displays a Message in-game.
        /// Uses the orange topmost notification.
        /// </summary>
        /// <param name="msg">The Message to show to the user</param>
        public static void DisplayMessage(string msg)
        {
            MissionDetail_Reminder.inst.StartCoroutine(LingerMessage(msg));
        }

        private static IEnumerator LingerMessage(string msg)
        {
            MissionDetail_Reminder.inst.root.SetActive(value: true);
            MissionDetail_Reminder.inst.descriptionLabel.text = msg;
            Go.to(MissionDetail_Reminder.inst._transform, 1f, new GoTweenConfig().anchoredPosition(Vector2.zero).setEaseType(GoEaseType.QuadOut));
            MissionDetail_Reminder.inst.canvasGroup.alphaTo(1f, 1f);
            yield return new WaitForSeconds(3f);
            Go.to(MissionDetail_Reminder.inst._transform, 1f, new GoTweenConfig().anchoredPosition(new Vector2(0f, 200f)).setEaseType(GoEaseType.QuadOut).onComplete(TurnOffMessage));
            MissionDetail_Reminder.inst.canvasGroup.alphaTo(1f, 0f);
        }

        private static void TurnOffMessage(AbstractGoTween tween)
        {
            MissionDetail_Reminder.inst.root.SetActive(value: false);
        }

        [HarmonyPatch(typeof(Player), "KillCharacter")]
        class PatchMMessageAwake
        {
            static void Prefix(Player __instance)
            {
                //test
                DisplayMessage("Received 'Progressive Beam' from isgvfj");
            }
        }
    }
}
