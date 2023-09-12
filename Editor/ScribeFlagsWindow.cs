using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


namespace Scribe.UI {

    public class ScribeFlagsWindow : EditorWindow {

        private List<KeyValuePair<string, int>> globalFlagsDict = new();
        private List<KeyValuePair<string, int>> tempFlagsDict = new();

        private ReorderableList globalFlagsReorderableList;
        private ReorderableList temporaryFlagsReorderableList;

        [SerializeField] private bool showAsBinary = false;



        [MenuItem ("Scribe/Flags")]
        private static void ShowWindow() {
            GetWindow(typeof(ScribeFlagsWindow), false, "Scribe Flags");
        }

        private static ReorderableList CreateList(Dictionary<string, int> flagsDict, List<KeyValuePair<string ,int>> flagsList, string label, bool showAsBinary, Action<string, int> onFlagChanged) {
            GUIContent globalFlagsLabel = new(label);

            return new ReorderableList(flagsList, typeof(KeyValuePair<string, int>), true, true, true, true) {
                drawHeaderCallback = (Rect rect) => {
                    EditorGUI.LabelField(rect, globalFlagsLabel);
                },
                drawNoneElementCallback = (Rect rect) => {
                    EditorGUI.LabelField(rect, "Dictionary is Empty");
                },
                onAddCallback = (ReorderableList list) => {
                    string key = list.count > 0 ? $"New Key {list.count}" : "New Key";
                    ScribeFlags.flags.Add(key, 0);
                    onFlagChanged("", 0);
                },
                onRemoveCallback = (ReorderableList list) => {
                    ScribeFlags.flags.Remove(ScribeFlags.flags.ElementAt(list.index).Key);
                    onFlagChanged("", 0);
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                    const float margin = 2f;
                    rect.y += EditorGUIUtility.standardVerticalSpacing;
                    float thirdWidth = rect.width / 3 - margin;

                    KeyValuePair<string, int> pair = flagsList.ElementAt(index);

                    string key = EditorGUI.TextField(new Rect(rect.x, rect.y, thirdWidth, EditorGUIUtility.singleLineHeight), pair.Key);

                    if (showAsBinary) {
                        string binary = Convert.ToString(pair.Value, 2);
                        binary = EditorGUI.TextField(new Rect(rect.x + thirdWidth + margin, rect.y, thirdWidth * 2f, EditorGUIUtility.singleLineHeight), binary);
                        if (int.TryParse(binary, out _)) {
                            int value = Convert.ToInt32(binary, 2);

                            if (pair.Value != value || pair.Key != key) {
                                flagsDict.Remove(pair.Key);
                                flagsDict[key] = value;
                                onFlagChanged(key, value);
                            }
                        }
                    } else {
                        int value = EditorGUI.IntField(new Rect(rect.x + thirdWidth + margin, rect.y, thirdWidth * 2f, EditorGUIUtility.singleLineHeight), pair.Value);

                        if (pair.Value != value || pair.Key != key) {
                            flagsDict.Remove(pair.Key);
                            flagsDict[key] = value;
                            onFlagChanged(key, value);
                        }
                    }

                }
            };
        }

        private void OnGUI() {

            // bool field
            bool oldValue = showAsBinary;
            showAsBinary = EditorGUILayout.Toggle("Show as Binary", showAsBinary);

            if (oldValue != showAsBinary) {
                globalFlagsReorderableList = null;
                temporaryFlagsReorderableList = null;
            }



            globalFlagsReorderableList ??= CreateList(ScribeFlags.flags, globalFlagsDict, "Global Flags", showAsBinary, (string key, int value) => {
                FlagsUpdate(key, value, ScribeFlags.FlagType.GlobalFlag);
            });
            globalFlagsReorderableList.DoLayoutList();


            GUILayout.Space(10);


            temporaryFlagsReorderableList ??= CreateList(ScribeFlags.tempFlags, tempFlagsDict, "Temporary Flags", showAsBinary, (string key, int value) => {
                FlagsUpdate(key, value, ScribeFlags.FlagType.TemporaryFlag);
            });
            temporaryFlagsReorderableList.DoLayoutList();
        }

        private void Update() {
            Repaint();
        }

        private void FlagsUpdate(string flagName, int flagValue, ScribeFlags.FlagType flagType) {
            if (flagType == ScribeFlags.FlagType.GlobalFlag) {
                globalFlagsDict.Clear();
                foreach (KeyValuePair<string, int> item in ScribeFlags.flags) {
                    globalFlagsDict.Add(new KeyValuePair<string, int>(item.Key, item.Value));
                }
            } else {
                tempFlagsDict.Clear();
                foreach (KeyValuePair<string, int> item in ScribeFlags.tempFlags) {
                    tempFlagsDict.Add(new KeyValuePair<string, int>(item.Key, item.Value));
                }
            }
        }

        private void OnEnable() {
            ScribeFlags.OnFlagChanged += FlagsUpdate;
        }

        private void OnDisable() {
            ScribeFlags.OnFlagChanged -= FlagsUpdate;
        }

    }
}