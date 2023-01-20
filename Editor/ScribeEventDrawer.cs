using System;
using System.Reflection;

using UnityEngine;
using UnityEditor;

using SevenGame.Utility.Editor;

namespace Scribe.UI {

    [CustomPropertyDrawer( typeof( ScribeEvent ), true )]
    public class ScribeEventDrawer : PropertyDrawer {

        private static Type scribeEventType;

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            Rect rectType = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);


            EditorGUI.BeginProperty( position, label, property );


            SerializedProperty propConditions = property.FindPropertyRelative( "conditions" );
            EditorGUI.PropertyField( rectType, propConditions, GUIContent.none );
                
            rectType.y += EditorGUI.GetPropertyHeight(propConditions) + EditorGUIUtility.standardVerticalSpacing;


            SerializedProperty propEventType = property.FindPropertyRelative( "eventType" );
            EditorGUI.PropertyField( rectType, propEventType, GUIContent.none );
                
            rectType.y += EditorGUI.GetPropertyHeight(propEventType) + EditorGUIUtility.standardVerticalSpacing;


            IterateOverEventProperties(property, fieldInfo, (prop) => {
                    EditorGUI.PropertyField( rectType, prop, GUIContent.none );
                    rectType.y += EditorGUI.GetPropertyHeight(prop) + EditorGUIUtility.standardVerticalSpacing;
                }
            );


            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            
            SerializedProperty propConditions = property.FindPropertyRelative( "conditions" );
            SerializedProperty propEventType = property.FindPropertyRelative( "eventType" );

            float height = EditorGUI.GetPropertyHeight(propConditions) + EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(propEventType) + EditorGUIUtility.standardVerticalSpacing;

            IterateOverEventProperties(property, fieldInfo, (prop) => {
                    height += EditorGUI.GetPropertyHeight(prop) + EditorGUIUtility.standardVerticalSpacing;
                }
            );

            return height;
        }


        private void IterateOverEventProperties(SerializedProperty property, FieldInfo fieldInfo, Action<SerializedProperty> callback) {
            // TODO : Find a better way to obtain the type of the ScribeEvent
            if (scribeEventType == null) {
                ScribeEvent scribeEvent = SevenEditorUtility.GetTargetObject(property) as ScribeEvent;
                scribeEventType = scribeEvent.GetType();
            }

            System.Reflection.FieldInfo[] fields = scribeEventType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            

            SerializedProperty propEventData = property.FindPropertyRelative( "eventType" );
            int eventType = propEventData.intValue;

            while (propEventData.NextVisible(false) && propEventData.depth > 0) {
                // Get field with the property name
                System.Reflection.FieldInfo field = System.Array.Find(fields, f => f.Name == propEventData.name);
                
                // Get attribute data from field
                System.Attribute[] attributes = System.Attribute.GetCustomAttributes(field);

                // Check if the field has the HideInInspector attribute
                bool isCorrectEventTypeData = System.Array.Exists(attributes, a => 
                    a is ScribeEventDataAttribute scribeEventDataAttribute && 
                    Convert.ToInt32(scribeEventDataAttribute.eventType) == eventType);

                if (isCorrectEventTypeData)
                    callback(propEventData);


            }
        }


    }

}