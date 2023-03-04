using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using SevenGame.Utility.Editor;
using System.Collections.Generic;

namespace Scribe.UI {

    [CustomPropertyDrawer( typeof( ScribeCondition ), true )]
    public class ScribeConditionDrawer : PropertyDrawer {

        private Type _scribeConditionType;



        private Type GetTypeOfCondition(SerializedProperty property) {
            if (_scribeConditionType == null) {
                object scribeCondition = SevenEditorUtility.GetTargetObject(property);
                _scribeConditionType = scribeCondition.GetType();
            }
            return _scribeConditionType;
        }

        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            EditorGUI.BeginProperty( position, label, property );

            Rect rectType = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);


            SerializedProperty propBinaryModifier = property.FindPropertyRelative( "binaryModifier" );


            
            ScribeEditorUtility.IterateThroughScribeObjectFields(GetTypeOfCondition(property), property, (prop, showLabel) => {

                    if (!SerializedProperty.EqualContents(propBinaryModifier, prop) && propBinaryModifier.intValue == 2) {
                        return;
                    }

                    GUIContent labelContent = showLabel ? new GUIContent(prop.displayName) : GUIContent.none;
                    
                    EditorGUI.PropertyField( rectType, prop, labelContent );
                    rectType.y += EditorGUI.GetPropertyHeight(prop, labelContent) + EditorGUIUtility.standardVerticalSpacing;
                }
            );


            EditorGUI.EndProperty();
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty propBinaryModifier = property.FindPropertyRelative( "binaryModifier" );
            float height = 0;
            
            ScribeEditorUtility.IterateThroughScribeObjectFields(GetTypeOfCondition(property), property, (prop, showLabel) => {

                    if (!SerializedProperty.EqualContents(propBinaryModifier, prop) && propBinaryModifier.intValue == 2) {
                        return;
                    }

                    GUIContent labelContent = showLabel ? new GUIContent(prop.displayName) : GUIContent.none;

                    height += EditorGUI.GetPropertyHeight(prop, labelContent) + EditorGUIUtility.standardVerticalSpacing;
                }
            );
            height -= EditorGUIUtility.standardVerticalSpacing;

            return height;
        }
        
    }

}