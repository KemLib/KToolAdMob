using GoogleMobileAds.Api;
using KTool.Init;
using System.Collections.Generic;
using UnityEngine;

namespace KTool.GoogleAdmob
{
    public class AdMobManager : MonoBehaviour, IIniter
    {
        #region Properties
        public const string ADMOB_SCOURCE = "GoogleAdMob",
            ADMOB_COUNTRY_CODE = "UnknownCountry";

        public static AdMobManager Instance
        {
            get;
            private set;
        }
        public static bool IsInit
        {
            get;
            private set;
        }
        public static bool IsIniting
        {
            get;
            private set;
        }

        [SerializeField]
        private string[] testDeviceIds;
        [SerializeField]
        private AdMobAdAppOpen[] adAppOpens;
        [SerializeField]
        private AdMobAdBanner[] adBanners;
        [SerializeField]
        private AdMobAdInterstitial[] adInterstitials;
        [SerializeField]
        private AdMobAdRewarded[] adRewardeds;
        [SerializeField]
        private AdMobAdRewardedInterstitial[] adRewardedInterstitials;

        private TrackEntrySource initTrackEntry;

        public bool RequiredConditions => true;
        #endregion

        #region Init
        public TrackEntry InitBegin()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                //
                AdMobSetting.Init();
                return AdMob_Init();
            }
            //
            return TrackEntry.TrackLoaderSuccess;
        }

        public void InitEnd()
        {

        }
        #endregion

        #region AdMob
        private TrackEntry AdMob_Init()
        {
            if (IsInit || IsIniting)
                return TrackEntry.TrackLoaderSuccess;
            //
            IsIniting = true;
            initTrackEntry = new TrackEntrySource();
            MobileAds.Initialize(AdMob_OnInitComplete);
            return initTrackEntry;
        }
        private void AdMob_OnInitComplete(InitializationStatus initStatus)
        {
            AdMob_RequestTestDevice();
            //
            IsIniting = false;
            IsInit = true;
            initTrackEntry.CompleteSuccess();
        }
        private void AdMob_RequestTestDevice()
        {
            List<string> ids = new List<string>();
            foreach (string deviceId in testDeviceIds)
            {
                if (string.IsNullOrEmpty(deviceId))
                    continue;
                ids.Add(deviceId);
            }
            if (ids.Count == 0)
                return;
            //
            RequestConfiguration requestConfiguration = new RequestConfiguration();
            foreach (string deviceId in ids)
                requestConfiguration.TestDeviceIds.Add(deviceId);
            MobileAds.SetRequestConfiguration(requestConfiguration);
        }
        #endregion

        #region AppOpen
        public int AppOpen_Count()
        {
            return adAppOpens.Length;
        }

        public AdMobAdAppOpen AppOpen_Get(int index)
        {
            if (index < 0 || index >= adAppOpens.Length)
                return null;
            return adAppOpens[index];
        }
        public AdMobAdAppOpen AppOpen_Get(string adName)
        {
            foreach (var ad in adAppOpens)
                if (ad.Name == adName)
                    return ad;
            return null;
        }
        #endregion

        #region Banner
        public int Banner_Count()
        {
            return adBanners.Length;
        }

        public AdMobAdBanner Banner_Get(int index)
        {
            if (index < 0 || index >= adBanners.Length)
                return null;
            return adBanners[index];
        }
        public AdMobAdBanner Banner_Get(string adName)
        {
            foreach (var ad in adBanners)
                if (ad.Name == adName)
                    return ad;
            return null;
        }
        #endregion

        #region Interstitial
        public int Interstitial_Count()
        {
            return adInterstitials.Length;
        }

        public AdMobAdInterstitial Interstitial_Get(int index)
        {
            if (index < 0 || index >= adInterstitials.Length)
                return null;
            return adInterstitials[index];
        }
        public AdMobAdInterstitial Interstitial_Get(string adName)
        {
            foreach (var ad in adInterstitials)
                if (ad.Name == adName)
                    return ad;
            return null;
        }
        #endregion

        #region Rewarded
        public int Rewarded_Count()
        {
            return adRewardeds.Length;
        }

        public AdMobAdRewarded Rewarded_Get(int index)
        {
            if (index < 0 || index >= adRewardeds.Length)
                return null;
            return adRewardeds[index];
        }
        public AdMobAdRewarded Rewarded_Get(string adName)
        {
            foreach (var ad in adRewardeds)
                if (ad.Name == adName)
                    return ad;
            return null;
        }
        #endregion

        #region RewardedInterstitial
        public int RewardedInterstitial_Count()
        {
            return adRewardedInterstitials.Length;
        }

        public AdMobAdRewardedInterstitial RewardedInterstitial_Get(int index)
        {
            if (index < 0 || index >= adRewardedInterstitials.Length)
                return null;
            return adRewardedInterstitials[index];
        }
        public AdMobAdRewardedInterstitial RewardedInterstitial_Get(string adName)
        {
            foreach (var ad in adRewardedInterstitials)
                if (ad.Name == adName)
                    return ad;
            return null;
        }
        #endregion
    }
}
