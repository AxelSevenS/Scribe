using UnityEngine;
using UnityEditor;
using System;

namespace Scribe.UI {

    [CustomPropertyDrawer( typeof( ScribeMultiCondition ), true )]
    public class ScribeMultiConditionDrawer : PropertyDrawer {


        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {

            Rect rectType = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);


            EditorGUI.BeginProperty( position, label, property );


            SerializedProperty propCondition = property.FindPropertyRelative( "condition" );
            EditorGUI.PropertyField( rectType, propCondition, GUIContent.none );
                
            rectType.y += EditorGUI.GetPropertyHeight(propCondition) + EditorGUIUtility.standardVerticalSpacing;


            if (propCondition.FindPropertyRelative("binaryModifier").intValue != 2) {
                SerializedProperty propSubConditions = property.FindPropertyRelative( "subConditions" );
                EditorGUI.PropertyField( rectType, propSubConditions );
                    
                rectType.y += EditorGUI.GetPropertyHeight(propSubConditions);
            }


            EditorGUI.EndProperty();
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty propCondition = property.FindPropertyRelative( "condition" );
            float height = EditorGUI.GetPropertyHeight(propCondition);

            if (propCondition.FindPropertyRelative( "binaryModifier" ).intValue != 2) {
                height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative( "subConditions" )) + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }
    }

}