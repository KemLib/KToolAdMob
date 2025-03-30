using UnityEditor;
using UnityEngine;
using KTool.Advertisement;

namespace KTool.GoogleAdmob.Editor
{

    [CustomEditor(typeof(AdMobAdBanner))]
    public class AdMobAdBannerEditor : UnityEditor.Editor
    {
        #region Properties
        private SerializedProperty propertyAdName,
            propertyIsAutoReload,
            propertyAdPosition,
            propertyAdSize,
            propertyPosition,
            propertySetInstance,
            propertyInitRequiredConditions,
            propertyIndexAd,
            propertyShowAfterInit;
        private AdPosition[] listAdPosition = new AdPosition[] { AdPosition.Custom, AdPosition.TopLeft, AdPosition.TopCenter, AdPosition.TopRight, AdPosition.MidCenter, AdPosition.BotLeft, AdPosition.BotCenter, AdPosition.BotRight };
        private string[] listAdPositionString = new string[] { "Custom", "TopLeft", "Top", "TopRight", "Center", "BottomLeft", "Bottom", "BottomRight" };
        private int indexAdPosition;
        private AdSize[] listAdSize = new AdSize[] { AdSize.Standard, AdSize.Large, AdSize.Medium, AdSize.FullSize };
        private string[] listAdSizeString = new string[] { "Banner", "Medium Rectangle", "IABBanner", "Leaderboard" };
        private int indexAdSize;
        #endregion

        #region Methods Unity        
        private void OnEnable()
        {
            propertyAdName = serializedObject.FindProperty("adName");
            propertyIsAutoReload = serializedObject.FindProperty("isAutoReload");
            propertyAdPosition = serializedObject.FindProperty("adPosition");
            propertyAdSize = serializedObject.FindProperty("adSize");
            propertyPosition = serializedObject.FindProperty("position");
            propertySetInstance = serializedObject.FindProperty("setInstance");
            propertyInitRequiredConditions = serializedObject.FindProperty("initRequiredConditions");
            propertyIndexAd = serializedObject.FindProperty("indexAd");
            propertyShowAfterInit = serializedObject.FindProperty("showAfterInit");
            //
            indexAdPosition = -1;
            for (int i = 0; i < listAdPosition.Length; i++)
                if ((int)listAdPosition[i] == propertyAdPosition.enumValueIndex)
                {
                    indexAdPosition = i;
                    break;
                }
            //
            indexAdSize = -1;
            for (int i = 0; i < listAdSize.Length; i++)
                if ((int)listAdSize[i] == propertyAdSize.enumValueIndex)
                {
                    indexAdSize = i;
                    break;
                }
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
            if(indexAdPosition == -1)
            {
                indexAdPosition = 6;
                propertyAdPosition.enumValueIndex = (int)listAdPosition[indexAdPosition];
            }
            int newIndexAdPosition = EditorGUILayout.Popup(new GUIContent("Ad Position"), indexAdPosition, listAdPositionString);
            if(newIndexAdPosition != indexAdPosition)
            {
                indexAdPosition = newIndexAdPosition;
                propertyAdPosition.enumValueIndex = (int)listAdPosition[indexAdPosition];
            }
            if (propertyAdPosition.enumValueIndex == (int)AdPosition.Custom)
                EditorGUILayout.PropertyField(propertyPosition, new GUIContent("Position"));
            //
            if (indexAdSize == -1)
            {
                indexAdSize = 0;
                propertyAdSize.enumValueIndex = (int)listAdSize[indexAdSize];
            }
            int newIndexAdSize = EditorGUILayout.Popup(new GUIContent("Ad Size"), indexAdSize, listAdSizeString);
            if (newIndexAdSize != indexAdSize)
            {
                indexAdSize = newIndexAdSize;
                propertyAdSize.enumValueIndex = (int)listAdSize[indexAdSize];
            }
            //
            EditorGUILayout.PropertyField(propertyShowAfterInit, new GUIContent("Show After Init"));
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Methods

        #endregion
    }
}
