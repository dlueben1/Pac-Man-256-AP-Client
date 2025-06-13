using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ApPac256
{
    public class ArchipelagoUIMainMenu : MonoBehaviour
    {
        private float labelOffset = 180f;
        private float gapBetween = 36f;

        private string serverField = "";
        private string pwdField = "";
        private string playerField = "";

        void OnGUI()
        {
            // Calculate the center of the screen
            var windowCenter = new Vector2(Screen.width / 2, Screen.height / 2);

            // Calculate the Anchor for the Menu
            var pivot = new Vector2(windowCenter.x + 10, windowCenter.y + 100);

            // Draw the Window
            GUI.Box(new Rect(pivot.x - 200, pivot.y - 32, 350, 200), "Archipelago Connection Settings");

            // Draw IP Field
            GUI.Label(new Rect(pivot.x - labelOffset, pivot.y, 150, 32), "Server IP:");
            serverField = GUI.TextField(new Rect(pivot.x - 50, pivot.y, 180, 32), serverField);

            // Draw Password Field
            GUI.Label(new Rect(pivot.x - labelOffset, pivot.y + (gapBetween * 1), 150, 32), "Server Password:");
            pwdField = GUI.TextField(new Rect(pivot.x - 50, pivot.y + (gapBetween * 1), 180, 32), pwdField);

            // Draw Player Name
            GUI.Label(new Rect(pivot.x - labelOffset, pivot.y + (gapBetween * 2), 150, 32), "Player Name:");
            playerField = GUI.TextField(new Rect(pivot.x - 50, pivot.y + (gapBetween * 2), 180, 32), playerField);

            // Handle Connection Attempt
            if (GUI.Button(new Rect(pivot.x - 80, pivot.y + (gapBetween * 3) + 12, 120, 32), "Connect"))
            {
                // Cache AP connection details
                ArchipelagoManager.ServerAddress = serverField;
                ArchipelagoManager.ServerPassword = pwdField;
                ArchipelagoManager.PlayerName = playerField;
                Plugin.Logger.LogInfo("Stored AP Connection Info in AP Manager");
                Plugin.Logger.LogInfo($"    - Server Address: {ArchipelagoManager.ServerAddress}");
                Plugin.Logger.LogInfo($"    - Server Password: {ArchipelagoManager.ServerPassword}");
                Plugin.Logger.LogInfo($"    - Player Name: {ArchipelagoManager.PlayerName}");
            }
        }
    }
}
