using System;
using System.Reflection;

using UnityEngine;
using UnityEditor;

using SevenGame.Utility.Editor;
using System.Collections.Generic;

namespace Scribe.UI {

    [CustomPropertyDrawer( typeof( ScribeEvent ), true )]
    public class ScribeEventDrawer : PropertyDrawer {

        private Type _scribeEventType;

        private const float CONDITION_PROPERTY_SPACE = 5f;


        private Type GetTypeOfEvent(SerializedProperty property) {
            if (_scribeEventType == null) {
                object scribeEvent = SevenEditorUtility.GetTargetObject(property);
                _scribeEventType = scribeEvent.GetType();
            }
            return _scribeEventType;
        }

        private bool IsExpandedConditionsField(SerializedProperty property) {
            return property.name == "conditions" && property.FindPropertyRelative("condition").FindPropertyRelative("binaryModifier").intValue != 2;
        }


        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            Rect rectType = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);


            EditorGUI.BeginProperty( position, label, property );

            
            ScribeEditorUtility.IterateThroughScribeObjectFields(GetTypeOfEvent(property), property, (prop, showLabel) => {

                    GUIContent labelContent = showLabel ? new GUIContent(prop.displayName) : GUIContent.none;

                    EditorGUI.PropertyField( rectType, prop, labelContent );
                    rectType.y += EditorGUI.GetPropertyHeight(prop, labelContent) + EditorGUIUtility.standardVerticalSpacing;

                    if ( IsExpandedConditionsField(prop) ) {
                        rectType.y += CONDITION_PROPERTY_SPACE;
                    }
                }
            );


            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            
            float height = 0;
            
            ScribeEditorUtility.IterateThroughScribeObjectFields(GetTypeOfEvent(property), property, (prop, showLabel) => {

                    GUIContent labelContent = showLabel ? new GUIContent(prop.displayName) : GUIContent.none;

                    height += EditorGUI.GetPropertyHeight(prop, labelContent) + EditorGUIUtility.standardVerticalSpacing;

                    if ( IsExpandedConditionsField(prop) ) {
                        height += CONDITION_PROPERTY_SPACE;
                    }
                }
            );
            height -= EditorGUIUtility.standardVerticalSpacing;

            return height;
        }


    }

}