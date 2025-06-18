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
        private static object _concurrent = new object();
        private static bool isOpen = false;
        private const float messageLingerTime = 3f;
        private static Queue<string> MessageQueue { get; set; } = new Queue<string>();

        /// <summary>
        /// Queues a message to be displayed in game
        /// </summary>
        /// <param name="msg">The Message to show to the user</param>
        public static void DisplayMessage(string msg)
        {
            // Queue the message
            lock(_concurrent)
            {
                Plugin.Logger.LogMessage(msg);
                MessageQueue.Enqueue(msg);
            }

            // If the Mission UI isn't already visible, make it visible
            if (!isOpen)
            {
                Plugin.Logger.LogInfo("Opening Message UI...");
                isOpen = true;
                MissionDetail_Reminder.inst.StartCoroutine(ShowMessageQueue());
            }
        }

        /// <summary>
        /// Display all messages in the queue
        /// </summary>
        /// <returns></returns>
        private static IEnumerator ShowMessageQueue()
        {
            // Show the panel
            isOpen = true;
            MissionDetail_Reminder.inst.root.SetActive(value: true);

            // Set initial message
            MissionDetail_Reminder.inst.descriptionLabel.text = MessageQueue.Peek();

            // Animate in
            Go.to(MissionDetail_Reminder.inst._transform, 1f, new GoTweenConfig().anchoredPosition(Vector2.zero).setEaseType(GoEaseType.QuadOut));
            MissionDetail_Reminder.inst.canvasGroup.alphaTo(1f, 1f);

            // Display each message
            do
            {
                MissionDetail_Reminder.inst.descriptionLabel.text = MessageQueue.Dequeue();
                yield return new WaitForSeconds(messageLingerTime);
            } while (MessageQueue.Count > 0);

            // Close the panel
            isOpen = false;
            Go.to(MissionDetail_Reminder.inst._transform, 1f, new GoTweenConfig().anchoredPosition(new Vector2(0f, 200f)).setEaseType(GoEaseType.QuadOut).onComplete(TurnOffMessage));
            MissionDetail_Reminder.inst.canvasGroup.alphaTo(1f, 0f);
        }

        private static void TurnOffMessage(AbstractGoTween tween)
        {
            MissionDetail_Reminder.inst.root.SetActive(value: false);
        }
    }
}
