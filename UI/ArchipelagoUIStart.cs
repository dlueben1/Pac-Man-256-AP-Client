using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ApPac256
{
    public class ArchipelagoUIStart : MonoBehaviour
    {
        /// <summary>
        /// Reference to the Main Camera that we alter for the initial menu
        /// </summary>
        public Camera MainCamera;

        /// <summary>
        /// Reference to the UI Camera that we alter for the initial menu
        /// </summary>
        public Camera UICamera;

        // Prevent a bug where you can start the game hundreds of times upon connection
        private readonly object _lock = new object();

        private enum APStartState: byte
        {
            Login = 0,
            TryConnect = 1,
            Destroying
        }

        private APStartState state;

        private const float labelOffset = 180f;
        private const float gapBetween = 36f;

        private string serverField = "";
        private string pwdField = "";
        private string playerField = "";

        void Start()
        {
            // Prevents the object from being deleted when the user presses space/A to start the game
            DontDestroyOnLoad(this.gameObject);
        }

        void OnGUI()
        {
            GUI.skin = CustomIMGUI.Skin;
            if(state == APStartState.Login)
            {
                DrawLoginUI();
            }
            else if(state == APStartState.TryConnect)
            {
                DrawConnectingUI();
            }
            
        }

        void DrawLoginUI()
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
            if (!ArchipelagoManager.Connecting && GUI.Button(new Rect(pivot.x - 80, pivot.y + (gapBetween * 3) + 12, 120, 32), "Connect"))
            {
                // Cache AP connection details
                ArchipelagoManager.ServerAddress = serverField;
                ArchipelagoManager.ServerPassword = pwdField;
                ArchipelagoManager.PlayerName = playerField;
                Plugin.Logger.LogInfo("Stored AP Connection Info in AP Manager");
                Plugin.Logger.LogInfo($"    - Server Address: {ArchipelagoManager.ServerAddress}");
                Plugin.Logger.LogInfo($"    - Server Password: {ArchipelagoManager.ServerPassword}");
                Plugin.Logger.LogInfo($"    - Player Name: {ArchipelagoManager.PlayerName}");

                // And try to connect
                state = APStartState.TryConnect;

                // Attempt to Connect
                ArchipelagoManager.Connect();
            }
        }

        void DrawConnectingUI()
        {
            // Calculate the center of the screen
            var windowCenter = new Vector2(Screen.width / 2, Screen.height / 2);

            // Calculate the Anchor for the Menu
            var pivot = new Vector2(windowCenter.x - 100, windowCenter.y + 100);

            // Draw the Window
            GUI.Box(new Rect(pivot.x, pivot.y, 150, 32), "Connecting...");

            // Listen for connection event
            ArchipelagoManager.Connected += OnConnectionStatus;
        }

        private void OnConnectionStatus(object sender, ResultEventArgs e)
        {
            // Stop listening for events
            ArchipelagoManager.Connected -= OnConnectionStatus;

            // If we connected, start the game
            if (e.Value)
            {
                StartGame();
            }
            // If we disconnected, go back to login
            else
            {
                state = APStartState.Login;
            }
        }

        void StartGame()
        {
            lock(_lock)
            {
                if (state == APStartState.Destroying) return;

                Plugin.Logger.LogMessage("START GAME...");

                // Disable the UI
                state = APStartState.Destroying;

                // Disable Free Gifts, if the player desired
                if(ArchipelagoManager.DisableFreeGifts)
                {
                    GM.inst.nextCoinAdWatchConsole = float.MaxValue;
                }

                // This is a functional copy of how the game starts itself
                UILogoController.inst.InvokeInternal("ChangeLogoSize");
                UILogoController.inst.ShowLogoLaunch(state: true);
                UILogoController.inst.isOpen = true;

                // Wrap up this GameObject
                StartCoroutine(OnExit());
            }
        }

        /// <summary>
        /// Restores the cameras once the developer logos are shown, so that visual glitches are not visible to the player
        /// </summary>
        private System.Collections.IEnumerator OnExit()
        {
            // Wait for the logos to start showing
            yield return new WaitForSeconds(3);

            // Fix the cameras we altered
            UICamera.clearFlags = CameraClearFlags.Depth;
            UICamera.backgroundColor = new Color(0.080f, 0.080f, 0.080f, 1.000f);
            MainCamera.enabled = true;

            // Add in the Main UI
            this.gameObject.AddComponent<ArchipelagoUIMainMenu>();

            // Stop listening to events
            ArchipelagoManager.Connected -= OnConnectionStatus;

            // Then destroy ourselves
            Destroy(this);
        }
    }
}
