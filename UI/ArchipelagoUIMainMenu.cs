using ApPac256.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ApPac256
{
    public class ArchipelagoUIMainMenu : MonoBehaviour
    {
        /// <summary>
        /// This is how the developers have made it easy to grab references to the instance for different UI items, so I'm following the pattern here
        /// </summary>
        public static ArchipelagoUIMainMenu inst;

        /// <summary>
        /// The Main Menu UIKey, generated when the main menu becomes visible initially
        /// </summary>
        private GameObject apMenuObj;

        private const float gapBetween = 12f;

        #region Shop GUI

        private bool showShop = false;
        private Rect windowRect = new Rect(200, 20, 500, 500);
        private Vector2 scrollVector = Vector2.zero;

        #endregion

        void Awake()
        {
            inst = this;
        }

        /// <summary>
        /// Creates a UIKey Button for triggering the In-Game Archipelago Menu
        /// </summary>
        public void TryCreateArchipelagoMenuButton()
        {
            // If the button already exists, ignore this
            if (apMenuObj != null) return;

            // Find the Button we're going to copy, since I can't find the reference to it
            var themeKey = Panel_PreGame.inst.expandKey.selectOnLeft.selectOnLeft;

            var playKey = Panel_PreGame.inst.expandKey.selectOnLeft;
            var y = playKey.gameObject.GetComponentsInChildren<Text>();
            foreach (Text p in y)
            {
                foreach(var f in p.font.fontNames)
                {
                    Plugin.Logger.LogMessage($"FONT NAME: {f}");
                }
            }

            // Create Archipelago Menu
            apMenuObj = GameObject.Instantiate(themeKey.gameObject, themeKey.transform);
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

            // Change the target for the submit event
            foreach(var m in apMenuKey.modules)
            {
                Plugin.Logger.LogMessage($"MODULE: {m.name} / TYPE: {m.GetType().ToString()}");
            }
            apMenuKey.onSubmit.RemoveAllListeners();
            apMenuKey.onSubmitWhileDisabled.RemoveAllListeners();
            apMenuKey.onSubmit = new UnityEngine.Events.UnityEvent();
            apMenuKey.onSubmit.AddListener(() =>
            {
                Plugin.Logger.LogMessage("PRESSED AP MENU!");
            });
        }

        void Update()
        {
            
        }

        void OnGUI()
        {
            try
            {
                // Always on UI

                // Togglable UI
                DrawGeneralInfo();
                DrawItemInfo();
                DrawShop();
            }
            catch { }
        }

        void DrawGeneralInfo()
        {
            // Draw Header
            GUI.Label(new Rect(0, 0, 150, 32), "PM256 Archipelago Alpha");

            // Draw Connection Details
            GUI.Label(new Rect(0, gapBetween * 1, 500, 32), $"Server: {ArchipelagoManager.ServerAddress}");
            GUI.Label(new Rect(0, gapBetween * 2, 500, 32), $"Player: {ArchipelagoManager.PlayerName}");
        }

        void DrawItemInfo()
        {
            GUI.Label(new Rect(0, 300 - gapBetween, 150, 32), $"Coins: {GM.inst.currentSaveData.gold}");
            GUI.Label(new Rect(0, 300 + gapBetween * 0, 150, 32), $"{ArchipelagoManager.PowerUps.Count} Unlocked Power-Ups");
            var _offset = 1;
            foreach(var pair in ArchipelagoManager.PowerUps)
            {
                GUI.Label(new Rect(0, 300 + gapBetween * _offset, 150, 32), $"  {pair.Key}: Level {pair.Value}");
                _offset++;
            }
        }

        void DrawShop()
        {
            if(GUI.Button(new Rect(0, 120, 150, 32), "Toggle Shop"))
            {
                showShop = !showShop;
            }
            if(showShop)
            {
                windowRect = GUI.Window(0, windowRect, (int id) =>
                {
                    var itemOffset = 32;
                    var padding = 10;
                    scrollVector = GUI.BeginScrollView(new Rect(16, 24, windowRect.width - (padding * 2), windowRect.height - padding), scrollVector, new Rect(0, 0, windowRect.width - (2 * padding), (itemOffset * ArchipelagoManager.Purchasables.Length) ));
                    for(int i = 0; i < ArchipelagoManager.Purchasables.Length; i++)
                    {
                        var item = ArchipelagoManager.Purchasables[i];
                        GUI.Label(new Rect(0, itemOffset * i, 299, 32), $"{item.ItemName} - {item.Owner} - {item.Cost}¢");
                        if(item.Purchased)
                        {
                            GUI.Label(new Rect(300, itemOffset * i, 100, 24), "SOLD OUT");
                        }
                        else
                        {
                            if(GUI.Button(new Rect(300, itemOffset * i, 100, 24), "Buy"))
                            {
                                if(item.Cost <= GM.inst.currentSaveData.gold)
                                {
                                    BuyItem(item);
                                }
                            }
                        }
                    }
                    GUI.EndScrollView();
                }, "Shop");
            }
        }

        void BuyItem(LocationShop item)
        {
            // Tell Archipelago
            ArchipelagoManager.Session.Locations.CompleteLocationChecks(item.ID);

            // Spend gold
            GM.inst.currentSaveData.gold -= item.Cost;

            // Mark item as bought
            item.Purchased = true;
        }
    }
}
