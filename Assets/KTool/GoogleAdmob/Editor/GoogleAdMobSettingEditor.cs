using KTool.FileIo;
using UnityEditor;
using UnityEngine;

namespace KTool.GoogleAdmob.Editor
{
    public class GoogleAdMobSettingEditor
    {
        #region Properties
        private const string ASSET_GOOGLE_ADMOB_SETTING_FOLDER_NAME = "Assets/GoogleMobileAds/Resources",
            ASSET_GOOGLE_ADMOB_SETTING_FILE_NAME = "GoogleMobileAdsSettings";
        private const string ASSET_GOOGLE_ADMOB_SETTING_PATH = ASSET_GOOGLE_ADMOB_SETTING_FOLDER_NAME + "/" + ASSET_GOOGLE_ADMOB_SETTING_FILE_NAME + ".asset";
        private const string DEFAULT_ANDROID_APP_ID = "ca-app-pub-3940256099942544~3347511713",
            DEFAULT_IOS_APP_ID = "ca-app-pub-3940256099942544~3347511713";

        private SerializedObject serializedGoogleAdMob;
        private SerializedProperty propertyAdMobAndroidAppId,
            propertyAdMobIOSAppId,
            propertyUserLanguage,
            propertyEnableKotlinXCoroutinesPackagingOption,
            propertyOptimizeInitialization,
            propertyOptimizeAdLoading,
            propertyUserTrackingUsageDescription,
            propertyValidateGradleDependencies;
        private bool isShowProperty;
        #endregion

        #region Construction
        public GoogleAdMobSettingEditor()
        {
            SettingObject_Load();
        }
        #endregion

        #region Unity Event
        public void OnInspectorGUI()
        {
            GUILayout.BeginVertical("GoogleAd Mob Setting", "window");
            if (serializedGoogleAdMob == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Create Setting"))
                {
                    SettingObject_Create();
                    SettingObject_Load();
                }
                GUILayout.EndHorizontal();
            }
            else
                OnGUI_SerializedGoogleAdMob();
            GUILayout.EndVertical();
        }
        private void OnGUI_SerializedGoogleAdMob()
        {
            if (isShowProperty)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Hide Setting"))
                {
                    isShowProperty = false;
                }
                GUILayout.EndHorizontal();
                //
                if (!isShowProperty)
                    return;
                OnGUI_PropertyGoogleAdMob();
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Show Setting"))
                {
                    isShowProperty = true;
                }
                GUILayout.EndHorizontal();
            }
        }
        private void OnGUI_PropertyGoogleAdMob()
        {
            serializedGoogleAdMob.Update();
            //
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(propertyAdMobAndroidAppId, new GUIContent("Android App Id"));
            if (GUILayout.Button("Default ID"))
                propertyAdMobAndroidAppId.stringValue = DEFAULT_ANDROID_APP_ID;
            GUILayout.EndHorizontal();
            //
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(propertyAdMobIOSAppId, new GUIContent("IOS App Id"));
            if (GUILayout.Button("Default ID"))
                propertyAdMobIOSAppId.stringValue = DEFAULT_IOS_APP_ID;
            GUILayout.EndHorizontal();
            //
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(propertyUserLanguage, new GUIContent("User Language"));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(propertyEnableKotlinXCoroutinesPackagingOption, new GUIContent("Enable KotlinX Coroutines Packaging Option"));
            EditorGUILayout.PropertyField(propertyOptimizeInitialization, new GUIContent("Optimize Initialization"));
            EditorGUILayout.PropertyField(propertyOptimizeAdLoading, new GUIContent("Optimize Ad Loading"));
            EditorGUILayout.PropertyField(propertyUserTrackingUsageDescription, new GUIContent("User Tracking Usage Description"));
            EditorGUILayout.PropertyField(propertyValidateGradleDependencies, new GUIContent("Validate Gradle Dependencies"));
            //
            serializedGoogleAdMob.ApplyModifiedProperties();
        }
        #endregion

        #region Method
        private void SettingObject_Load()
        {
            ScriptableObject scriptable = AssetDatabase.LoadAssetAtPath<ScriptableObject>(ASSET_GOOGLE_ADMOB_SETTING_PATH);
            if (scriptable == null)
            {
                serializedGoogleAdMob = null;
                propertyAdMobAndroidAppId = null;
                propertyAdMobIOSAppId = null;
                propertyUserLanguage = null;
                propertyEnableKotlinXCoroutinesPackagingOption = null;
                propertyOptimizeInitialization = null;
                propertyOptimizeAdLoading = null;
                propertyUserTrackingUsageDescription = null;
                propertyValidateGradleDependencies = null;
                return;
            }
            serializedGoogleAdMob = new SerializedObject(scriptable);
            propertyAdMobAndroidAppId = serializedGoogleAdMob.FindProperty("adMobAndroidAppId");
            propertyAdMobIOSAppId = serializedGoogleAdMob.FindProperty("adMobIOSAppId");
            propertyUserLanguage = serializedGoogleAdMob.FindProperty("userLanguage");
            propertyEnableKotlinXCoroutinesPackagingOption = serializedGoogleAdMob.FindProperty("enableKotlinXCoroutinesPackagingOption");
            propertyOptimizeInitialization = serializedGoogleAdMob.FindProperty("optimizeInitialization");
            propertyOptimizeAdLoading = serializedGoogleAdMob.FindProperty("optimizeAdLoading");
            propertyUserTrackingUsageDescription = serializedGoogleAdMob.FindProperty("userTrackingUsageDescription");
            propertyValidateGradleDependencies = serializedGoogleAdMob.FindProperty("validateGradleDependencies");
        }
        private void SettingObject_Create()
        {
            if (!AssetFinder.Exists(ASSET_GOOGLE_ADMOB_SETTING_FOLDER_NAME))
            {
                AssetFinder.CreateFolder(ASSET_GOOGLE_ADMOB_SETTING_FOLDER_NAME);
                AssetDatabase.Refresh();
            }
            ScriptableObject scriptable = ScriptableObject.CreateInstance("GoogleMobileAdsSettings");
            AssetDatabase.CreateAsset(scriptable, ASSET_GOOGLE_ADMOB_SETTING_PATH);
        }
        #endregion
    }
}
