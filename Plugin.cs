﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ApPac256;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("PAC-MAN256.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        // Initialize Archipelago Data
        ArchipelagoManager.PowerUps = new Dictionary<PowerupType, int>();
        ArchipelagoManager.Loadout = new List<int> { 0, 0, 0 };

        // Apply Method Patches
        harmony.PatchAll();
    }

    private void OnDestroy()
    {
        // Remove Harmony Patches
        harmony.UnpatchSelf();

    }

    /// <summary>
    /// Disconnect from Archipelago when we exit the game
    /// </summary>
    void OnApplicationQuit()
    {
        ArchipelagoManager.Disconnect();
    }

}
