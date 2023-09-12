using System;

using UnityEngine;
using UnityEditor;

using SevenGame.Utility;

namespace Scribe.UI {

    [CustomPropertyDrawer( typeof( ScribeCondition ), true )]
    public class ScribeConditionDrawer : PropertyDrawer {

        private static bool insideCondition = false;

        private Type _scribeConditionType;
        private bool isChildCondition = false;



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

            if ( !isChildCondition ) {
                EditorGUI.PropertyField( rectType, propBinaryModifier, GUIContent.none );
                rectType.y += EditorGUI.GetPropertyHeight(propBinaryModifier) + EditorGUIUtility.standardVerticalSpacing;
            } 


            if ( propBinaryModifier.intValue != 2 ) {

                ScribeEditorUtility.IterateThroughScribeObjectFields(
                    GetTypeOfCondition(property), property, (prop, showLabel) => {

                        if ( SerializedProperty.EqualContents(propBinaryModifier, prop) ) {
                            return;
                        }

                        GUIContent labelContent = showLabel ? new GUIContent(prop.displayName) : GUIContent.none;
                        
                        EditorGUI.PropertyField( rectType, prop, labelContent );
                        rectType.y += EditorGUI.GetPropertyHeight(prop, labelContent) + EditorGUIUtility.standardVerticalSpacing;
                    }
                );

            }


            EditorGUI.EndProperty();
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {

            if ( insideCondition ) {
                isChildCondition = true;
            }

            SerializedProperty propBinaryModifier = property.FindPropertyRelative( "binaryModifier" );
            float height = 0;
            if ( isChildCondition ) {
                propBinaryModifier.intValue = 2;
            } else {
                height = EditorGUI.GetPropertyHeight(propBinaryModifier) + EditorGUIUtility.standardVerticalSpacing;
            }
            
            if ( propBinaryModifier.intValue != 2 ) {

                insideCondition = true;
                
                ScribeEditorUtility.IterateThroughScribeObjectFields(
                    GetTypeOfCondition(property), property, (prop, showLabel) => {

                        if ( SerializedProperty.EqualContents(propBinaryModifier, prop) ) {
                            return;
                        }

                        GUIContent labelContent = showLabel ? new GUIContent(prop.displayName) : GUIContent.none;

                        height += EditorGUI.GetPropertyHeight(prop, labelContent) + EditorGUIUtility.standardVerticalSpacing;
                    }
                );

            }
            height -= EditorGUIUtility.standardVerticalSpacing;

            insideCondition = false;

            return height;
        }
        
    }

}