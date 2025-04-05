using UnityEditor;
using UnityEngine;

namespace KTool.GoogleAdmob.Editor
{
    [CustomEditor(typeof(AdMobSetting))]
    public class AdMobSettingEditor : UnityEditor.Editor
    {
        #region Properties
        public const string ASSET_ADMOB_SETTING_FOLDER_NAME = "Assets/KTool/Resources/" + AdMobSetting.RESOURCES_PATH_FOLDER,
            ASSET_ADMOB_SETTING_FILE_NAME = AdMobSetting.RESOURCES_PATH_FILE;
        public const string ASSET_GOOGLE_ADMOB_SETTING_PATH = ASSET_ADMOB_SETTING_FOLDER_NAME + "/" + ASSET_ADMOB_SETTING_FILE_NAME + ".asset";

        private GoogleAdMobSettingEditor googleAdMobSetting;
        private AdIdSettingEditor appOpenSetting,
            bannerSetting,
            interstitialSetting,
            rewardedSetting,
            rewardedInterstitialSetting;
        #endregion

        #region Unity Event
        private void OnEnable()
        {
            Init();
        }

        public override void OnInspectorGUI()
        {
            OnGui_GoogleAdMobSetting();
            GUILayout.Space(5);
            OnGui_AdMobSetting();
        }
        private void OnGui_GoogleAdMobSetting()
        {
            googleAdMobSetting.OnInspectorGUI();
        }
        private void OnGui_AdMobSetting()
        {
            serializedObject.Update();
            //
            appOpenSetting.OnInspectorGUI();
            GUILayout.Space(5);
            bannerSetting.OnInspectorGUI();
            GUILayout.Space(5);
            interstitialSetting.OnInspectorGUI();
            GUILayout.Space(5);
            rewardedSetting.OnInspectorGUI();
            GUILayout.Space(5);
            rewardedInterstitialSetting.OnInspectorGUI();
            //
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Method
        private void Init()
        {
            googleAdMobSetting = new GoogleAdMobSettingEditor();
            SerializedProperty propertyAppOpenIds = serializedObject.FindProperty("appOpenIds"),
                propertyBannerIds = serializedObject.FindProperty("bannerIds"),
                propertyInterstitialIds = serializedObject.FindProperty("interstitialIds"),
                propertyRewardedIds = serializedObject.FindProperty("rewardedIds"),
                propertyRewardedInterstitialIds = serializedObject.FindProperty("rewardedInterstitialIds"),
                propertyNativeIds = serializedObject.FindProperty("nativeIds");
            appOpenSetting = new AdIdSettingEditor(propertyAppOpenIds, AdMobAdType.AppOpen);
            bannerSetting = new AdIdSettingEditor(propertyBannerIds, AdMobAdType.Banner);
            interstitialSetting = new AdIdSettingEditor(propertyInterstitialIds, AdMobAdType.Interstitial);
            rewardedSetting = new AdIdSettingEditor(propertyRewardedIds, AdMobAdType.Rewarded);
            rewardedInterstitialSetting = new AdIdSettingEditor(propertyRewardedInterstitialIds, AdMobAdType.RewardedInterstitial);
        }
        #endregion
    }
}
