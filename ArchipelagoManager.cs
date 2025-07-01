using ApPac256.Data;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace ApPac256
{
    public class ResultEventArgs : EventArgs
    {
        public bool Value;
    }

    /// <summary>
    /// Handles the state of our Archipelago Multiworld, including connection details and gameplay data
    /// 
    /// Big shout-out to `alwaysintreble` for their Plugin Template, which was used as a base for the client
    /// https://github.com/alwaysintreble/ArchipelagoBepInExPluginTemplate/tree/master
    /// </summary>
    public static class ArchipelagoManager
    {
        #region Connection Info

        public static string ServerAddress {get; set;}
        public static string ServerPassword { get; set;}
        public static string PlayerName { get; set;}
        public static string Seed { get; set; }
        private const string Game = "PAC-MAN 256";
        public const string APVersion = "0.5.0";

        #endregion

        #region Connection Details

        public static bool Authenticated { get; set; }
        public static bool Connecting { get; set; }

        public static ArchipelagoSession Session { get; set; }
        
        /// <summary>
        /// Represents how caught up we are with Archipelago's sent items
        /// </summary>
        private static int Index;

        public static Dictionary<string, object> SlotData { get; set; }

        #endregion

        #region UI

        private static bool IsMainMenuInjected { get; set; }

        #endregion

        #region ???

        /// <summary>
        /// Fires when we're successfully connected to Archipelago
        /// </summary>
        public static event EventHandler<ResultEventArgs> Connected;

        #endregion

        #region Power-Ups

        public static Dictionary<PowerupType, int> PowerUps { get; set;}
        
        // Keep a cached version of PowerUps.Keys.ToList() for performance
        public static List<int> Loadout { get; set; }

        #endregion

        #region Shop

        public static LocationShop[] Purchasables { get; set; }

        #endregion

        #region ???

        public static List<long> CheckedLocations { get; set; }

        #endregion

        #region Options

        public static bool DisableFreeGifts { get; set; }

        #endregion

        #region Networking

        /// <summary>
        /// Attempts to connect to an Archipelago room
        /// </summary>
        public static void Connect()
        {
            // Ignore if we're already authenticated
            if (Authenticated || Connecting) return;
            Plugin.Logger.LogMessage("CONNECTING...");
            Connecting = true;

            // Setup Data
            SlotData?.Clear();
            SlotData = new Dictionary<string, object>();
            CheckedLocations = new List<long>();

            // Attempt to create the AP Session
            try
            {
                // Setup the Session
                Session = ArchipelagoSessionFactory.CreateSession(ServerAddress);
            }
            catch(Exception e)
            {
                Plugin.Logger.LogError("Failed to create the Archipelago Session");
                Plugin.Logger.LogError(e);
                return;
            }

            // Listen for received items
            Session.Items.ItemReceived += OnItemReceived;

            // Listen for generic messages
            Session.MessageLog.OnMessageReceived += message => Plugin.Logger.LogMessage(message.ToString());

            // Listen for errors
            Session.Socket.ErrorReceived += OnErrorReceived;

            // Listen for connection termination
            Session.Socket.SocketClosed += OnSocketSessionEnd;

            // Attempt to connect to the server
            try
            {
                // it's safe to thread this function call but unity notoriously hates threading so do not use excessively
                ThreadPool.QueueUserWorkItem(
                    _ => HandleConnectResult(
                        Session.TryConnectAndLogin(
                            Game,
                            PlayerName,
                            ItemsHandlingFlags.AllItems,
                            new Version(APVersion),
                            password: ServerPassword,
                            requestSlotData: SlotData.Count == 0
                        )));
            }
            catch (Exception e)
            {
                Plugin.Logger.LogError("Failed to connect to the Archipelago Room");
                Plugin.Logger.LogError(e);
                HandleConnectResult(new LoginFailure(e.ToString()));
            }
        }

        /// <summary>
        /// Handle the outcome of a connection attempt
        /// </summary>
        private static void HandleConnectResult(LoginResult result)
        {
            Plugin.Logger.LogMessage("HCR...");
            string outText;
            if (result.Successful)
            {
                // We are now connected!
                var success = (LoginSuccessful)result;
                Authenticated = true;

                // Store Session information
                SlotData = success.SlotData;
                Seed = Session.RoomState.Seed;

                // Complete any locations that we have
                Session.Locations.CompleteLocationChecksAsync(null, CheckedLocations.ToArray());
                outText = $"Successfully connected to {ServerAddress} as {PlayerName}!";

                // Let the game know that we've connected
                OnConnected();
            }
            else
            {
                // Log the error
                var failure = (LoginFailure)result;
                outText = $"Failed to connect to {ServerAddress} as {PlayerName}.";
                outText = failure.Errors.Aggregate(outText, (current, error) => current + $"\n    {error}");

                // Mark us as un-authenticated and disconnect
                Authenticated = false;
                Disconnect();
            }
            Connecting = false;

            // Log the message
            Plugin.Logger.LogMessage(outText);
        }

        /// <summary>
        /// @todo clean this lol
        /// </summary>
        public static void OnConnected()
        {
            // Log all slot data
            foreach(var kvp in SlotData)
            {
                Plugin.Logger.LogMessage($"KEY: {kvp.Key}");
                Plugin.Logger.LogMessage($"VAL: {kvp.Value.ToString()}");
            }

            // Initialize Shop
            Session.Locations.ScoutLocationsAsync(map => 
            {
                Purchasables = map.Select(kvp => new LocationShop {
                    Cost = 25,
                    ID = kvp.Key,
                    ItemName = kvp.Value.ItemName,
                    Owner = kvp.Value.Player.Name,
                    Purchased = false
            }).ToArray(); 
            Connected?.Invoke(null, new ResultEventArgs { Value = true }); 
            }, Session.Locations.AllLocations.ToArray());
        }

        /// <summary>
        /// Cleans up our Session with Archipelago
        /// </summary>
        public static void Disconnect()
        {
            Plugin.Logger.LogDebug("Disconnecting from Archipelago...");
            Session?.Socket.Disconnect();
            Session = null;
            Authenticated = false;

            // Let the game know that we've disconnected
            Connected?.Invoke(null, new ResultEventArgs { Value = true });
        }

        /// <summary>
        /// Log errors to the console
        /// </summary>
        private static void OnErrorReceived(Exception e, string message)
        {
            Plugin.Logger.LogError(e);
        }

        /// <summary>
        /// When we end our Session, disconnect from the Archipelago server
        /// </summary>
        private static void OnSocketSessionEnd(string reason)
        {
            Plugin.Logger.LogError($"Connection to Archipelago lost: {reason}");
            Disconnect();
        }

        /// <summary>
        /// Handle incoming items that come from Archipelago
        /// </summary>
        private static void OnItemReceived(ReceivedItemsHelper helper)
        {
            Plugin.Logger.LogInfo("TEST HERE");
            // Grab the item data
            var receivedItem = helper.DequeueItem();
            Plugin.Logger.LogInfo($" - {receivedItem.ItemName}");
            Plugin.Logger.LogInfo($" - INDEX: {Index} / HI {helper.Index}");

            // Ignore if this item is an old message
            if (helper.Index <= Index) return;

            // Keep track of how many messages we've had so far
            Index++;

            // Let the user know they obtained an item!
            MessageUtil.DisplayMessage($"Received {receivedItem.ItemDisplayName} from {receivedItem.Player.Name}");

            // Add the item @todo fix this for clean
            if(receivedItem.ItemName == "Coins")
            {
                Plugin.Logger.LogInfo($" - ADDING GOLD");
                GM.inst.currentSaveData.gold += 12;
            }
            else
            {
                // Update Powerup Map
                PowerupType powerup = (PowerupType)receivedItem.ItemId;
                if(PowerUps.ContainsKey(powerup))
                {
                    PowerUps[powerup] += 1;
                    if (PowerUps[powerup] > 8)
                    {
                        PowerUps[powerup] = 8;
                    }
                }
                else
                {
                    PowerUps[powerup] = 1;
                }

                // Set Cached Loadout
                Loadout = PowerUps.Keys.Select(k => (int)k).ToList();
                while(Loadout.Count < 3)
                {
                    Loadout.Add(0);
                }
            }
            Plugin.Logger.LogInfo($" - POWERUPS COUNT: {PowerUps.Count}");
        }

        #endregion

        #region Archipelago Menu UI

        /// <summary>
        /// Creates a UIKey Button for triggering the In-Game Archipelago Menu
        /// </summary>
        public static void TryCreateArchipelagoMenuButton()
        {
            // Ignore if we've injected the main menu
            if(IsMainMenuInjected) return;

            // Find the Button we're going to copy, since I can't find the reference to it
            var themeKey = Panel_PreGame.inst.expandKey.selectOnLeft.selectOnLeft;

            // Create Archipelago Menu
            var apMenuObj = GameObject.Instantiate(themeKey.gameObject, themeKey.transform);
            var apMenuRect = apMenuObj.GetComponent<RectTransform>();
            var apMenuKey = apMenuObj.GetComponent<UIKey>();
            apMenuRect.anchoredPosition = new Vector2(70, 0);

            // Connect the AP Menu Button to the UI for Keyboard/Gamepad navigation
            var singlePlayerKey = Panel_PreGame.inst.expandKey.selectOnLeft;
            apMenuKey.selectOnLeft = themeKey;
            apMenuKey.selectOnRight = singlePlayerKey;

            // Connect the rest of the UI to the AP Menu Button
            singlePlayerKey.selectOnLeft = apMenuKey;
            themeKey.selectOnRight = apMenuKey;
            singlePlayerKey.selectOnDown.selectOnLeft = apMenuKey;
            singlePlayerKey.selectOnUp.selectOnLeft = apMenuKey;

            // Load Archipelago Image from Code (trying to keep everything inside the plugin for now, open to suggestions)
            var tex = new Texture2D(0, 0, TextureFormat.RGBA32, false);
            tex.LoadImage(IconArchipelago.Pixels, true);
            tex.filterMode = FilterMode.Point;
            var icon = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

            // Change icon for Archipelago Menu and scale it to the correct size
            var img = apMenuObj.transform.FindChild("Icon").GetComponent<Image>();
            var iconRect = img.GetComponent<RectTransform>();
            img.sprite = icon;
            var newScale = iconRect.localScale * 0.5f;
            iconRect.INTERNAL_set_localScale(ref newScale);

            // Create the Archipelago View (Shop + Settings)
            var apMenuView = GameObject.Instantiate(PowerUpGridManager.inst.gameObject, PowerUpGridManager.inst.transform.parent);
            apMenuView.AddComponent<ArchipelagoUIMainMenu>();

            // Change the target for the menu trigger's submit event
            foreach (var m in apMenuKey.modules)
            {
                Plugin.Logger.LogMessage($"MODULE: {m.name} / TYPE: {m.GetType().ToString()}");
            }
            apMenuKey.onSubmit.RemoveAllListeners();
            apMenuKey.onSubmitWhileDisabled.RemoveAllListeners();
            apMenuKey.onSubmit = new UnityEngine.Events.UnityEvent();
            apMenuKey.onSubmit.AddListener(() =>
            {
                apMenuView.GetComponent<PowerUpGridManager>().ToggleOn();
            });

            // Mark that we've injected the Main Menu
            IsMainMenuInjected = true;
        }

        #endregion
    }
}
