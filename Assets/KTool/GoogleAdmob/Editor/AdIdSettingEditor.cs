using UnityEditor;
using UnityEngine;

namespace KTool.GoogleAdmob.Editor
{
    public class AdIdSettingEditor
    {
        #region Properties
        private const string TEST_AD_ANDROID_APP_OPEN_ID = "ca-app-pub-3940256099942544/9257395921",
            TEST_AD_ANDROID_BANNER_ID = "ca-app-pub-3940256099942544/6300978111",
            TEST_AD_ANDROID_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712",
            TEST_AD_ANDROID_REWARDED_ID = "ca-app-pub-3940256099942544/5224354917",
            TEST_AD_ANDROID_REWARDED_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/5354046379";
        private const string TEST_AD_IOS_APP_OPEN_ID = "ca-app-pub-3940256099942544/5575463023",
            TEST_AD_IOS_BANNER_ID = "ca-app-pub-3940256099942544/2934735716",
            TEST_AD_IOS_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/4411468910",
            TEST_AD_IOS_REWARDED_ID = "ca-app-pub-3940256099942544/1712485313",
            TEST_AD_IOS_REWARDED_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/6978759866";

        private SerializedProperty propertyAds;
        private AdMobAdType adMobAdType;
        private bool isShow;
        #endregion

        #region Construction
        public AdIdSettingEditor(SerializedProperty propertyAds, AdMobAdType adMobAdType)
        {
            this.propertyAds = propertyAds;
            this.adMobAdType = adMobAdType;
            //
            isShow = false;
        }
        #endregion

        #region Method
        public void OnInspectorGUI()
        {
            string title = adMobAdType.ToString() + " Setting";
            GUILayout.BeginVertical(title, "window");
            OnInspectorGUI_Menu();
            if (isShow)
                OnInspectorGUI_Ids();
            GUILayout.EndVertical();
        }

        private void OnInspectorGUI_Menu()
        {
            GUILayout.BeginHorizontal();
            int count = EditorGUILayout.IntField(new GUIContent("Count"), propertyAds.arraySize);
            if (count != propertyAds.arraySize)
                propertyAds.arraySize = count;
            if (propertyAds.arraySize > 0)
            {
                if (isShow)
                {
                    if (GUILayout.Button("Hide"))
                        isShow = false;
                }
                else
                {
                    if (GUILayout.Button("Show"))
                        isShow = true;
                }
            }
            GUILayout.EndHorizontal();
        }

        private void OnInspectorGUI_Ids()
        {
            for (int i = 0; i < propertyAds.arraySize; i++)
            {
                SerializedProperty propertyAd = propertyAds.GetArrayElementAtIndex(i);
                string title = "Item " + i;
                GUILayout.Space(5);
                GUILayout.BeginVertical(title, "window");
                OnInspectorGUI_Id(propertyAd);
                GUILayout.EndVertical();
            }
        }

        private void OnInspectorGUI_Id(SerializedProperty propertyAd)
        {
            SerializedProperty propertyAndroidId = propertyAd.FindPropertyRelative("androidId"),
                propertyIosId = propertyAd.FindPropertyRelative("iosId");
            //
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(propertyAndroidId, new GUIContent("Android Id"));
            if (GUILayout.Button("Default ID"))
                propertyAndroidId.stringValue = GetAndroidId_Default();
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(propertyIosId, new GUIContent("Ios Id"));
            if (GUILayout.Button("Default ID"))
                propertyIosId.stringValue = GetIosId_Default();
            GUILayout.EndHorizontal();
        }
        #endregion

        private string GetAndroidId_Default()
        {
            switch (adMobAdType)
            {
                case AdMobAdType.AppOpen:
                    return TEST_AD_ANDROID_APP_OPEN_ID;
                case AdMobAdType.Banner:
                    return TEST_AD_ANDROID_BANNER_ID;
                case AdMobAdType.Interstitial:
                    return TEST_AD_ANDROID_INTERSTITIAL_ID;
                case AdMobAdType.Rewarded:
                    return TEST_AD_ANDROID_REWARDED_ID;
                case AdMobAdType.RewardedInterstitial:
                    return TEST_AD_ANDROID_REWARDED_INTERSTITIAL_ID;
                default:
                    return string.Empty;
            }
        }
        private string GetIosId_Default()
        {
            switch (adMobAdType)
            {
                case AdMobAdType.AppOpen:
                    return TEST_AD_IOS_APP_OPEN_ID;
                case AdMobAdType.Banner:
                    return TEST_AD_IOS_BANNER_ID;
                case AdMobAdType.Interstitial:
                    return TEST_AD_IOS_INTERSTITIAL_ID;
                case AdMobAdType.Rewarded:
                    return TEST_AD_IOS_REWARDED_ID;
                case AdMobAdType.RewardedInterstitial:
                    return TEST_AD_IOS_REWARDED_INTERSTITIAL_ID;
                default:
                    return string.Empty;
            }
        }
    }
}
