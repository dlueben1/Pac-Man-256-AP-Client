using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ApPac256
{
    public class ArchipelagoUIMainMenu : MonoBehaviour
    {
        void OnGUI()
        {
            // Draw IP Field
            GUI.Label(new Rect(0, 0, 150, 32), "PM256 Archipelago Alpha");
        }
    }
}
