using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using SevenGame.Utility.Editor;

namespace Scribe.UI {

    [CustomPropertyDrawer( typeof( ScribeCondition ), true )]
    public class ScribeConditionDrawer : PropertyDrawer {

        private static Type scribeConditionType;

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            EditorGUI.BeginProperty( position, label, property );

            Rect rectType = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);


            SerializedProperty propBinaryModifier = property.FindPropertyRelative( "binaryModifier" );

            EditorGUI.PropertyField( rectType, propBinaryModifier, GUIContent.none );
            rectType.y += EditorGUI.GetPropertyHeight(propBinaryModifier) + EditorGUIUtility.standardVerticalSpacing;

            if (propBinaryModifier.intValue != 2) {

                SerializedProperty propConditionType = property.FindPropertyRelative( "conditionType" );
                EditorGUI.PropertyField( rectType, propConditionType, GUIContent.none );
                rectType.y += EditorGUI.GetPropertyHeight(propConditionType) + EditorGUIUtility.standardVerticalSpacing;

                IterateOverConditionProperties(property, fieldInfo, (prop) => {
                        EditorGUI.PropertyField( rectType, prop );
                        rectType.y += EditorGUI.GetPropertyHeight(prop) + EditorGUIUtility.standardVerticalSpacing;
                    }
                );

            }

            EditorGUI.EndProperty();
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty propBinaryModifier = property.FindPropertyRelative( "binaryModifier" );
            float height = EditorGUI.GetPropertyHeight(propBinaryModifier);

            if (propBinaryModifier.intValue != 2) {

                SerializedProperty propConditionType = property.FindPropertyRelative( "conditionType" );
                height += EditorGUI.GetPropertyHeight(propConditionType) + EditorGUIUtility.standardVerticalSpacing;
                
                IterateOverConditionProperties(property, fieldInfo, (prop) => {
                        height += EditorGUI.GetPropertyHeight(prop) + EditorGUIUtility.standardVerticalSpacing;
                    }
                );

            }

            return height;
        }
        
        private void IterateOverConditionProperties(SerializedProperty property, FieldInfo fieldInfo, Action<SerializedProperty> callback) {

            SerializedProperty propConditionType = property.FindPropertyRelative( "conditionType" );


            if (propConditionType == null) {

                // Draw all visible properties
                SerializedProperty prop = property.FindPropertyRelative( "binaryModifier" );
                while (prop.NextVisible(false)) {
                    if (SerializedProperty.EqualContents(prop, property.GetEndProperty())) {
                        break;
                    }
                    callback(prop);
                }

            } else {

                // TODO : Find a better way to obtain the type of the ScribeCondition
                if (scribeConditionType == null) {
                    ScribeCondition scribeEvent = SevenEditorUtility.GetTargetObject(property) as ScribeCondition;
                    scribeConditionType = scribeEvent.GetType();
                }

                int conditionType = propConditionType.intValue;
                string[] propertyNames = ScribeEditorUtility.GetScribeEventProperties(scribeConditionType, conditionType);

                foreach (string propertyName in propertyNames) {
                    SerializedProperty prop = property.FindPropertyRelative(propertyName);
                    callback(prop);
                }

            }
            
        }
    }

}