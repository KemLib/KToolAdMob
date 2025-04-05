using UnityEditor;
using UnityEngine;

namespace KTool.GoogleAdmob.Editor
{
    public class CreateGameObject
    {
        #region Properties
        private const string GAME_OBJECT_NAME_ADMOB_MANAGER = "KTool_Admob_Manager",
            GAME_OBJECT_NAME_ADMOB_AD_APP_OPEN = "KTool_AdMob_Ad_AppOpen",
            GAME_OBJECT_NAME_ADMOB_AD_BANNER = "KTool_AdMob_Ad_Banner",
            GAME_OBJECT_NAME_ADMOB_AD_INTERSTITIAL = "KTool_AdMob_Ad_Interstitial",
            GAME_OBJECT_NAME_ADMOB_AD_REWARDED = "KTool_AdMob_Ad_Rewarded",
            GAME_OBJECT_NAME_ADMOB_AD_REWARDED_INTERSTITIAL = "KTool_AdMob_Ad_RewardedInterstitial";
        #endregion

        #region Methods
        [MenuItem("GameObject/KTool/Admob/Create Admob Manager", priority = 0)]
        private static void Create_InitManager()
        {
            GameObject newGO = new GameObject(GAME_OBJECT_NAME_ADMOB_MANAGER);
            newGO.AddComponent<AdMobManager>();
            //
            if (Selection.activeTransform != null)
                newGO.transform.SetParent(Selection.activeTransform);
            Selection.activeGameObject = newGO;
        }
        [MenuItem("GameObject/KTool/Admob/Create Ad AppOpen", priority = 1)]
        private static void Create_AdAppOpen()
        {
            GameObject newGO = new GameObject(GAME_OBJECT_NAME_ADMOB_AD_APP_OPEN);
            newGO.AddComponent<AdMobAdAppOpen>();
            //
            if (Selection.activeTransform != null)
                newGO.transform.SetParent(Selection.activeTransform);
            Selection.activeGameObject = newGO;
        }
        [MenuItem("GameObject/KTool/Admob/Create Ad Banner", priority = 2)]
        private static void Create_AdBanner()
        {
            GameObject newGO = new GameObject(GAME_OBJECT_NAME_ADMOB_AD_BANNER);
            newGO.AddComponent<AdMobAdBanner>();
            //
            if (Selection.activeTransform != null)
                newGO.transform.SetParent(Selection.activeTransform);
            Selection.activeGameObject = newGO;
        }
        [MenuItem("GameObject/KTool/Admob/Create Ad Interstitial", priority = 3)]
        private static void Create_AdInterstitial()
        {
            GameObject newGO = new GameObject(GAME_OBJECT_NAME_ADMOB_AD_INTERSTITIAL);
            newGO.AddComponent<AdMobAdInterstitial>();
            //
            if (Selection.activeTransform != null)
                newGO.transform.SetParent(Selection.activeTransform);
            Selection.activeGameObject = newGO;
        }
        [MenuItem("GameObject/KTool/Admob/Create Ad Rewarded", priority = 4)]
        private static void Create_AdRewarded()
        {
            GameObject newGO = new GameObject(GAME_OBJECT_NAME_ADMOB_AD_REWARDED);
            newGO.AddComponent<AdMobAdRewarded>();
            //
            if (Selection.activeTransform != null)
                newGO.transform.SetParent(Selection.activeTransform);
            Selection.activeGameObject = newGO;
        }
        [MenuItem("GameObject/KTool/Admob/Create Ad Rewarded Interstitial", priority = 5)]
        private static void Create_AdRewardedInterstitial()
        {
            GameObject newGO = new GameObject(GAME_OBJECT_NAME_ADMOB_AD_REWARDED_INTERSTITIAL);
            newGO.AddComponent<AdMobAdRewardedInterstitial>();
            //
            if (Selection.activeTransform != null)
                newGO.transform.SetParent(Selection.activeTransform);
            Selection.activeGameObject = newGO;
        }
        #endregion
    }
}
