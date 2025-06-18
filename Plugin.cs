using BepInEx;
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
    private int test = 0;

    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        // Initialize Archipelago Data
        ArchipelagoManager.PowerUps = new Dictionary<string, int>();
        ArchipelagoManager.Purchasables = new LocationShop[50];
        for(int i = 0; i < 50; i++)
        {
            ArchipelagoManager.Purchasables[i] = new LocationShop
            {
                ItemName = $"Item #{UnityEngine.Random.Range(0, 10000)}",
                Owner = "Claire",
                Cost = UnityEngine.Random.Range(16, 64)
            };
        }

        // Apply Method Patches
        harmony.PatchAll();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            test++;
            MessageUtil.DisplayMessage($"CHECK #${test} {System.DateTime.Now}");
        }
    }

    private void OnDestroy()
    {
        harmony.UnpatchSelf();
    }

}
