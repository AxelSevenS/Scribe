using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Scribe.UI {

    public static class ScribeEditorUtility {

        public static readonly Dictionary<Type, ScribeTypeData> scribeTypesData = new Dictionary<Type, ScribeTypeData>();
        
        [InitializeOnLoadMethod]
        [MenuItem ("Scribe/Cache Type Data")]
        private static void CacheScribeEventTypes() {

            scribeTypesData.Clear();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies) {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types) {
                    if (typeof(ScribeAction).IsAssignableFrom(type) || typeof(ScribeCondition).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract) {
                        CacheScribeEventTypeData(type);
                    }
                }
            }


            // foreach (var item in scribeTypesData) {
            //     Debug.Log($" - {item.Key}");
            //     Debug.Log($" - Options :");
            //     foreach (var item2 in item.Value.options) {
            //         Debug.Log($" -     {item2}");
            //     }
            //     Debug.Log($" - Fields :");
            //     foreach (var item2 in item.Value.fieldsData) {
            //         Debug.Log($" -     {item2.Key}");
            //         foreach (var item3 in item2.Value) {
            //             Debug.Log($" -              {item3}");
            //         }
            //     }
                
            // }

        }

        public static void IterateThroughScribeObjectFields(Type type, SerializedProperty property, Action<SerializedProperty, bool> callback) {


            if ( !scribeTypesData.ContainsKey(type) ) {
                Debug.LogError(message: $"{nameof(ScribeEditorUtility)}: {type} is not a ScribeEvent or ScribeCondition. Please use {nameof(IterateThroughScribeObjectFields)} only on ScribeEvent or ScribeCondition types.");
                return;
            }

            
            // Get the first visible child property - this way we skip the script property
            SerializedProperty prop = property.Copy();

            bool enterChildren = true;
            // Draw all visible properties
            while (prop.NextVisible(enterChildren)) {
                enterChildren = false;

                // Check if we've reached the end of the list
                if (SerializedProperty.EqualContents(prop, property.GetEndProperty()) || prop == null) {
                    break;
                }

                if ( !scribeTypesData[type].fieldsData.ContainsKey(prop.name) ) {
                    Debug.LogWarning($"{nameof(ScribeEditorUtility)}: {type} does not contain a field named {prop.name}.");
                    continue;
                }

                // Check if the field should be displayed
                bool showLabel = true;
                if ( scribeTypesData[type].showLabelFlags.TryGetValue(prop.name, out bool dictShowLabel) )
                    showLabel = dictShowLabel;

                // If the field has no options, display it normally
                if ( scribeTypesData[type].fieldsData[prop.name].Count == 0 ) {
                    callback(prop, showLabel);
                    continue;
                }

                // Get all option values for the current property
                Dictionary<string, int> optionValues = new Dictionary<string, int>();
                foreach (string optionName in scribeTypesData[type].options) {
                    SerializedProperty propOption = property.FindPropertyRelative( optionName );
                    try {
                        int optionValue = propOption.intValue;
                        optionValues.Add(optionName, optionValue);
                    } catch {}
                }

                // Check if any options validate the field
                foreach( KeyValuePair<string, int> option in optionValues ) {

                    if ( IsScribeFieldDisplayed(type, prop.name, option.Key, option.Value) ) {
                        callback(prop, showLabel);
                        break;
                    }
                }

            }
        }


        public static bool IsScribeFieldDisplayed(Type scribeEventType, string propertyName, string optionName, int optionValue) {

            // Check if the specified option is set to the right value for the field to be displayed
            List<ScribeFieldData> fieldData = scribeTypesData[scribeEventType].fieldsData[propertyName];

            if (fieldData.Count == 0)
                return true;
            
            return fieldData.Any(data => data.optionName == optionName && data.optionValue == optionValue);
        }


        private static void CacheScribeEventTypeData(Type scribeEventType) {
            
            scribeTypesData[scribeEventType] = new ScribeTypeData();


            FieldInfo[] fields = scribeEventType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);


            foreach (FieldInfo field in fields) {

                scribeTypesData[scribeEventType].fieldsData[field.Name] = Enumerable.Empty<ScribeFieldData>().ToList();

                // Check if field is serialized
                if ( !field.IsPublic && !field.IsDefined(typeof(SerializeField), false) && !field.IsDefined(typeof(SerializeField), false) )
                    continue;



                Attribute[] attributes = Attribute.GetCustomAttributes(field);

                // Check if label should be hidden
                if (attributes.OfType<ScribeHideLabelAttribute>().Count() != 0) {
                    scribeTypesData[scribeEventType].showLabelFlags[field.Name] = false;
                }

                // Check if field is an option
                if (attributes.OfType<ScribeOptionAttribute>().Count() != 0) {
                    if ( !scribeTypesData[scribeEventType].options.Contains(field.Name) ) {
                        scribeTypesData[scribeEventType].options.Add(field.Name);
                    }
                    continue;
                }

                // Check if field has any ScribeFieldData attributes
                foreach (ScribeFieldAttribute scribeFieldDataAttribute in attributes.OfType<ScribeFieldAttribute>()) {

                    ScribeFieldData scribeFieldData = new() {
                        optionName = scribeFieldDataAttribute.optionName,
                        optionValue = scribeFieldDataAttribute.optionValue
                    };

                    scribeTypesData[scribeEventType].fieldsData[field.Name].Add(scribeFieldData);
                }
            }
        }

        public class ScribeTypeData {
            public List<string> options = Enumerable.Empty<string>().ToList();
            public Dictionary<string, List<ScribeFieldData>> fieldsData = new();
            public Dictionary<string, bool> showLabelFlags = new();
        }

        public class ScribeFieldData {
            public string optionName;
            public int optionValue;
        }
    }

}