using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Scribe.UI {

    public static class ScribeEditorUtility {

        public static readonly Dictionary<Type, List<ScribeEventData>> scribeEventTypesData = new Dictionary<Type, List<ScribeEventData>>();
        
        [UnityEditor.Callbacks.DidReloadScripts]
        [MenuItem ("Scribe/Cache Type Data")]
        private static void CacheScribeEventTypes() {
            scribeEventTypesData.Clear();


            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies) {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types) {
                    if (typeof(ScribeEvent).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract) {
                        CacheScribeEventTypeData(type);
                    }
                }
            }


        }

        public static string[] GetScribeEventProperties(Type scribeEventType, int eventType) {
            if ( !scribeEventTypesData.ContainsKey(scribeEventType) )
                CacheScribeEventTypeData(scribeEventType);

            return scribeEventTypesData[scribeEventType]
                .Where(scribeEventData => scribeEventData.scribeEventType == eventType)
                .Select(scribeEventData => scribeEventData.propertyName)
                .ToArray();
        }

        private static void CacheScribeEventTypeData(Type scribeEventType) {
            FieldInfo[] fields = scribeEventType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields) {

                // Get attribute data from field
                System.Attribute[] attributes = System.Attribute.GetCustomAttributes(field);

                // Get all attributes of Type ScribeEventDataAttribute
                ScribeEventDataAttribute[] scribeEventDataAttributes = attributes.OfType<ScribeEventDataAttribute>().ToArray();

                foreach (ScribeEventDataAttribute scribeEventDataAttribute in scribeEventDataAttributes) {
                    ScribeEventData scribeEventData = new ScribeEventData {
                        scribeEventType = scribeEventDataAttribute.eventType,
                        propertyName = field.Name
                    };

                    if (ScribeEditorUtility.scribeEventTypesData.ContainsKey(scribeEventType)) {
                        ScribeEditorUtility.scribeEventTypesData[scribeEventType].Add(scribeEventData);

                    } else {
                        ScribeEditorUtility.scribeEventTypesData.Add(scribeEventType, new List<ScribeEventData> { scribeEventData });

                    }
                }
            }
        }

        public struct ScribeEventData {
            public int scribeEventType;
            public string propertyName;
        }
    }

}