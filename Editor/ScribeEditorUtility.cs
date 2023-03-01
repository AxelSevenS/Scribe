using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Scribe.UI {

    public static class ScribeEditorUtility {

        public static readonly Dictionary<Type, List<ScribeFieldData>> ScribeTypeData = new Dictionary<Type, List<ScribeFieldData>>();
        
        [UnityEditor.Callbacks.DidReloadScripts]
        [MenuItem ("Scribe/Cache Type Data")]
        private static void CacheScribeEventTypes() {
            ScribeTypeData.Clear();


            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies) {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types) {
                    if (typeof(ScribeEvent).IsAssignableFrom(type) || typeof(ScribeCondition).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract) {
                        CacheScribeEventTypeData(type);
                    }
                }
            }


        }

        public static string[] GetScribeEventProperties(Type scribeEventType, int eventType) {

            if ( !ScribeTypeData.ContainsKey(scribeEventType) )
                CacheScribeEventTypeData(scribeEventType);

            return ScribeTypeData[scribeEventType]
                .Where(scribeEventData => scribeEventData.scribeEventType == eventType || scribeEventData.scribeEventType < 0)
                .Select(scribeEventData => scribeEventData.propertyName)
                .ToArray();
        }

        private static void CacheScribeEventTypeData(Type scribeEventType) {
            FieldInfo[] fields = scribeEventType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (!ScribeEditorUtility.ScribeTypeData.ContainsKey(scribeEventType))
                ScribeEditorUtility.ScribeTypeData.Add(scribeEventType, Enumerable.Empty<ScribeFieldData>().ToList());

            foreach (FieldInfo field in fields) {

                // Check if field is serialized
                if ( !field.IsPublic && !field.IsDefined(typeof(SerializeField), false) && !field.IsDefined(typeof(SerializeField), false) )
                    continue;

                // Get attribute data from field
                System.Attribute[] attributes = System.Attribute.GetCustomAttributes(field);

                // Get all attributes of Type ScribeFieldAttribute
                ScribeFieldAttribute[] scribeEventDataAttributes = attributes.OfType<ScribeFieldAttribute>().ToArray();

                foreach (ScribeFieldAttribute scribeEventDataAttribute in scribeEventDataAttributes) {
                    ScribeFieldData scribeEventData = new ScribeFieldData {
                        scribeEventType = scribeEventDataAttribute.eventType,
                        propertyName = field.Name
                    };

                    ScribeEditorUtility.ScribeTypeData[scribeEventType].Add(scribeEventData);
                }
            }
        }

        public struct ScribeFieldData {
            public int scribeEventType;
            public string propertyName;
        }
    }

}