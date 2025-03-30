using UnityEditor;
using UnityEngine;

namespace KTool.GoogleAdmob.Editor
{

    [CustomEditor(typeof(AdMobAdRewarded))]
    public class AdMobAdRewardedEditor : UnityEditor.Editor
    {
        #region Properties
        private SerializedProperty propertyAdName,
            propertyIsAutoReload,
            propertySetInstance,
            propertyInitRequiredConditions,
            propertyIndexAd;
        #endregion

        #region Methods Unity        
        private void OnEnable()
        {
            propertyAdName = serializedObject.FindProperty("adName");
            propertyIsAutoReload = serializedObject.FindProperty("isAutoReload");
            propertySetInstance = serializedObject.FindProperty("setInstance");
            propertyInitRequiredConditions = serializedObject.FindProperty("initRequiredConditions");
            propertyIndexAd = serializedObject.FindProperty("indexAd");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //
            EditorGUILayout.PropertyField(propertySetInstance, new GUIContent("Set Instance"));
            EditorGUILayout.PropertyField(propertyInitRequiredConditions, new GUIContent("Init Required Conditions"));
            EditorGUILayout.PropertyField(propertyAdName, new GUIContent("Ad Name"));
            if (string.IsNullOrEmpty(propertyAdName.stringValue))
            {
                propertyAdName.stringValue = serializedObject.targetObject.name;
            }
            EditorGUILayout.PropertyField(propertyIsAutoReload, new GUIContent("Auto Reload"));
            EditorGUILayout.PropertyField(propertyIndexAd, new GUIContent("Index Ad"));
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Methods

        #endregion
    }
}
