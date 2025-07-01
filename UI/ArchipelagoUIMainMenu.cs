using ApPac256;
using BepInEx.Logging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchipelagoUIMainMenu : MonoBehaviour
{
    public static ArchipelagoUIMainMenu inst;

    public Text shopHeader;

    private void Awake()
    {
        // Ignore if we've already setup
        if (inst != null) return;

        // Hook our instance
        inst = this;

        // Rename our Game Object so it's easier to differentiate which is the Power-Up Grid and which is the AP Menu
        gameObject.name = "AP_MAIN_MENU_OBJ";

        // Traverse the transform tree
        var realRoot = transform.GetChild(0).Find("move");
        var scrollRoot = realRoot.Find("Scroll_Powerups").Find("Content").Find("Panel_Powerups");
        var slotRoot = realRoot.Find("Button_Slots");

        // Remove unnecessary pop-ups
        GameObject.DestroyImmediate(realRoot.Find("intro_PU").gameObject);
        GameObject.DestroyImmediate(realRoot.Find("intro_PU_Console").gameObject);

        // Get references
        shopHeader = slotRoot.Find("Heading").GetComponent<Text>();
        var comps = slotRoot.GetComponentsInChildren<Text>();
        foreach(var x in comps)
        {
            Plugin.Logger.LogMessage($"OBJ NAME: {x.gameObject.name}");
        }

        // Redecorate from Power-Ups to Shop
        shopHeader.text = "Archipelago Item Shop";
    }

    void OnEnable()
    {
        // Traverse the transform tree
        var realRoot = transform.GetChild(0).Find("move");
        var scrollRoot = realRoot.Find("Scroll_Powerups").Find("Content").Find("Panel_Powerups");
        var slotRoot = realRoot.Find("Button_Slots");

        // Remove unnecessary pop-ups
        GameObject.DestroyImmediate(realRoot.Find("intro_PU").gameObject);
        GameObject.DestroyImmediate(realRoot.Find("intro_PU_Console").gameObject);

        // Get references
        var texts = realRoot.GetComponentsInChildren<Text>();
        foreach(var t in texts)
        {
            t.text = $"AP - {t.gameObject.name} - PARENT: {t.transform.parent.name}";
            Plugin.Logger.LogMessage($"AP - {t.gameObject.name} - PARENT: {t.transform.parent.name}");
        }
    }

    void Update()
    {
        // Traverse the transform tree
        var realRoot = transform.GetChild(0).Find("move");
        var scrollRoot = realRoot.Find("Scroll_Powerups").Find("Content").Find("Panel_Powerups");
        var slotRoot = realRoot.Find("Button_Slots");

        // Remove unnecessary pop-ups
        GameObject.DestroyImmediate(realRoot.Find("intro_PU").gameObject);
        GameObject.DestroyImmediate(realRoot.Find("intro_PU_Console").gameObject);

        // Get references
        var texts = realRoot.GetComponentsInChildren<Text>();
        foreach (var t in texts)
        {
            t.text = $"AP - {t.gameObject.name} - PARENT: {t.transform.parent.name}";
        }
    }
}
