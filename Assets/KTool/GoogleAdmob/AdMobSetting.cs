using UnityEngine;

namespace KTool.GoogleAdmob
{
    public class AdMobSetting : ScriptableObject
    {
        #region Properties
        public const string RESOURCES_PATH_FOLDER = "KTool/GoogleAdMob",
            RESOURCES_PATH_FILE = "AdMobSetting";
        public const string RESOURCES_PATH = RESOURCES_PATH_FOLDER + "/" + RESOURCES_PATH_FILE;

        public static AdMobSetting Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private AdMobSettingAdId[] appOpenIds,
            bannerIds,
            interstitialIds,
            rewardedIds,
            rewardedInterstitialIds;
        #endregion

        #region Unity Event

        #endregion

        #region Method
        public static void Init()
        {
            Instance = GetInstance();
        }
        public static AdMobSetting GetInstance()
        {
            return Resources.Load<AdMobSetting>(RESOURCES_PATH);
        }
        #endregion

        #region Ad
        public int Ad_Count(AdMobAdType adType)
        {
            switch (adType)
            {
                case AdMobAdType.AppOpen:
                    return appOpenIds.Length;
                case AdMobAdType.Banner:
                    return bannerIds.Length;
                case AdMobAdType.Interstitial:
                    return interstitialIds.Length;
                case AdMobAdType.Rewarded:
                    return rewardedIds.Length;
                case AdMobAdType.RewardedInterstitial:
                    return rewardedInterstitialIds.Length;
                default:
                    return 0;
            }
        }
        public AdMobSettingAdId Ad_Get(AdMobAdType adType, int index)
        {
            switch (adType)
            {
                case AdMobAdType.AppOpen:
                    return appOpenIds[index];
                case AdMobAdType.Banner:
                    return bannerIds[index];
                case AdMobAdType.Interstitial:
                    return interstitialIds[index];
                case AdMobAdType.Rewarded:
                    return rewardedIds[index];
                case AdMobAdType.RewardedInterstitial:
                    return rewardedInterstitialIds[index];
                default:
                    return null;
            }
        }
        #endregion
    }
}
