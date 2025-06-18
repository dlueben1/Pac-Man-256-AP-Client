using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ApPac256
{
    public class ArchipelagoUIMainMenu : MonoBehaviour
    {
        private const float gapBetween = 12f;

        #region Shop GUI

        private bool showShop = false;
        private Rect windowRect = new Rect(200, 20, 500, 500);
        private Vector2 scrollVector = Vector2.zero;

        #endregion

        void OnGUI()
        {
            DrawGeneralInfo();
            DrawItemInfo();
            DrawShop();
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
                        GUI.Button(new Rect(300, itemOffset * i, 100, 24), "Buy");
                    }
                    GUI.EndScrollView();
                }, "Shop");
            }
        }
    }
}
