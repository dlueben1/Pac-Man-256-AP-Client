using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ApPac256
{
    public static class CustomIMGUI
    {
        private static GUISkin _skin;
        public static GUISkin Skin
        {
            get
            {
                if(_skin == null)
                {
                    BuildCustomSkin();
                }
                return _skin;
            }
        }

        private static void BuildCustomSkin()
        {
            // Get default styles as a base
            var styleBtn = new GUIStyle(GUI.skin.button);
            var styleTextField = new GUIStyle(GUI.skin.textField);
            var styleLabel = new GUIStyle(GUI.skin.label);
            var styleBox = new GUIStyle(GUI.skin.box);

            // Make changes to each style
            styleLabel.normal.textColor = Color.white;

            // Apply to the skin
            _skin = new GUISkin();
            _skin.button = styleBtn;
            _skin.textField = styleTextField;
            _skin.label = styleLabel;
            _skin.box = styleBox;
        }
    }
}
