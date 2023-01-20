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
            height -= EditorGUIUtility.standardVerticalSpacing;

            return height;
        }


        private void IterateOverEventProperties(SerializedProperty property, FieldInfo fieldInfo, Action<SerializedProperty> callback) {
            // TODO : Find a better way to obtain the type of the ScribeEvent
            if (scribeEventType == null) {
                ScribeEvent scribeEvent = SevenEditorUtility.GetTargetObject(property) as ScribeEvent;
                scribeEventType = scribeEvent.GetType();
            }

            int eventType = property.FindPropertyRelative( "eventType" ).intValue;
            string[] propertyNames = ScribeEditorUtility.GetScribeEventProperties(scribeEventType, eventType);

            foreach (string propertyName in propertyNames) {
                SerializedProperty prop = property.FindPropertyRelative(propertyName);
                callback(prop);
            }
            
        }


    }

}