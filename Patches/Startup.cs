using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ApPac256
{
    public static class Startup
    {
        private static float logoY;

        /// <summary>
        /// Patch to prevent the default bootup sequence, so that we can inject an Archipelago Login screen first
        /// </summary>
        [HarmonyPatch(typeof(UILogoController), "Start")]
        class PatchStopNormalLoad
        {
            static bool Prefix(UILogoController __instance)
            {
                Plugin.Logger.LogMessage("Overriding normal game boot sequence...");
                System.IO.File.WriteAllText("alive.txt", "PLUGIN LIVES");

                // Set the initial logo position
                logoY = 8f;

                // Disable the in-game camera, for now
                Camera mainCam = Camera.main;
                Camera.main.enabled = false;

                // Find the UI Camera
                Camera uiCam = null;
                foreach(var cam in Camera.allCameras)
                {
                    if (!cam.CompareTag("MainCamera")) { uiCam = cam; break; }
                }

                // If somehow the camera is still null, we have problems
                if(uiCam == null)
                {
                    Plugin.Logger.LogError("Could not find UI camera!");
                    return false;
                }

                // Inject the Archipelago Startup Menu
                __instance.StartCoroutine(APStart(__instance, mainCam, uiCam));

                // Prevent original from running initially
                return false;
            }
        }

        /// <summary>
        /// Override behavior for Application Startup.
        /// Used to inject the Archipelago Connection Menu.
        /// </summary>
        static IEnumerator APStart(UILogoController startup, Camera mainCam, Camera uiCam)
        {
            // Hijack the camera, for now
            //uiCam.clearFlags = CameraClearFlags.SolidColor;
            //uiCam.backgroundColor = Color.blue;

            // Not sure the purpose of this, but it was used in the original and fixes problems
            float t2 = 0.2f;
            while (t2 > 0f)
            {
                t2 -= Time.deltaTime;
                yield return null;
            }

            Go.to(startup.logo, 1f, new GoTweenConfig().localPosition(new Vector3(0f, logoY, 0f)).setEaseType(GoEaseType.QuadOut));
            t2 = 1f;
            while (t2 > 0f)
            {
                t2 -= Time.deltaTime;
                yield return null;
            }

            // Run important commands from the original startup
            startup.InvokeInternal("ChangeLogoSize");

            // Inject the Archipelago Startup Menu
            var menuObj = new GameObject("Archipelago UI - Start");
            var component = menuObj.AddComponent<ArchipelagoUIStart>();
            component.MainCamera = mainCam;
            component.UICamera = uiCam;

            yield return null;
        }
    }
}
