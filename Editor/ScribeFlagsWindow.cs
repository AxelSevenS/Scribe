using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

using SevenGame.Utility;

namespace Scribe.UI {

    public class ScribeFlagsWindow : EditorWindow {

        [MenuItem ("Scribe/Flags")]
        private static void ShowWindow() {
            EditorWindow.GetWindow(typeof(ScribeFlagsWindow), false, "Scribe Flags");
        }

        private SerializableDictionary<string, int> flagsDict = new SerializableDictionary<string, int>();
        private SerializableDictionary<string, int> tempFlagsDict = new SerializableDictionary<string, int>();

        private void OnGUI() {

            FieldInfo globalFlags = typeof(ScribeFlags).GetField("flags", BindingFlags.Public | BindingFlags.Static);
            if (globalFlags != null) {
                
                flagsDict = (globalFlags.GetValue(null) as Dictionary<string, int>).ToSerializableDictionary();
                
                GUILayout.Label("Global Flags :", EditorStyles.boldLabel);
                foreach (var flag in (globalFlags.GetValue(null) as Dictionary<string, int>)) {
                    GUILayout.Label(flag.Key + ": " + flag.Value);
                }
            }

            GUILayout.Space(10);

            FieldInfo tempFlags = typeof(ScribeFlags).GetField("tempFlags", BindingFlags.Public | BindingFlags.Static);
            if (tempFlags != null) {

                tempFlagsDict = (globalFlags.GetValue(null) as Dictionary<string, int>).ToSerializableDictionary();
                
                GUILayout.Label("Temporary Flags :", EditorStyles.boldLabel);
                foreach (var flag in (tempFlags.GetValue(null) as Dictionary<string, int>)) {
                    GUILayout.Label(flag.Key + ": " + flag.Value);
                }
            }
        }

        private void Update() {
            Repaint();
        }

    }
}