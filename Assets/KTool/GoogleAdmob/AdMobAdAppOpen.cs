using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using KTool.Advertisement;
using KTool.Init;
using System;
using System.Collections;
using UnityEngine;

namespace KTool.GoogleAdmob
{
    public class AdMobAdAppOpen : AdInterstitial, IIniter
    {
        #region Properties
        private const string ERROR_LOAD_FAIL = "Ad AppOpen load fail: {0}",
            ERROR_SHOW_FAIL_AD_NOT_READY = "Ad AppOpen show fail: ad not ready",
            ERROR_SHOW_FAIL_AD_IS_SHOWED = "Ad AppOpen show fail: ad is show";
        private const int AD_EXPIRE_HOUR = 4;

        [SerializeField]
        private bool initRequiredConditions;
        [SerializeField]
        private bool setInstance;
        [SerializeField, SelectAdId(AdMobAdType.AppOpen)]
        private int indexAd = 0;
        [SerializeField]
        private bool showIfAppResumed;

        private bool isLoading;
        private int attemptLoad;
        private AppOpenAd adObject;
        private DateTime expireTime;
        private TrackEntrySource initTrackEntrySource;
        private bool initEnded;
        private AdInterstitialTrackingSource adInterstitialTrackingSource;

        public bool RequiredConditions => initRequiredConditions;
        public string AdId
        {
            get
            {
                AdMobSettingAdId settingAdId = AdMobSetting.Instance.Ad_Get(AdMobAdType.AppOpen, indexAd);
                if (settingAdId != null)
                    return settingAdId.AdID;
                return string.Empty;
            }
        }
        public bool ShowIfAppResumed => showIfAppResumed;
        public override bool IsAutoReload
        {
            get => base.IsAutoReload;
            protected set
            {
                if (value == base.IsAutoReload)
                    return;
                base.IsAutoReload = value;
                if (base.IsAutoReload)
                    Load();
            }
        }
        public override bool IsReady => base.IsReady && adObject != null && adObject.CanShowAd();
        #endregion

        #region Unity Event
        private void Update()
        {
            Update_ExpireTime();
        }
        private void OnDestroy()
        {
            AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
            Ad_Destroy();
            State = AdState.Destroy;
            if (Instance != null && Instance.GetInstanceID() == GetInstanceID())
                Instance = null;
            PushEvent_Destroy();
        }
        private void OnAppStateChanged(AppState state)
        {
            // if the app is Foregrounded and the ad is available, show it.
            if (state == AppState.Foreground)
            {
                if (ShowIfAppResumed && IsReady)
                    Show();
            }
        }
        #endregion

        #region Init
        public TrackEntry InitBegin()
        {
            TrackEntrySource trackEntrySource = initTrackEntrySource = new TrackEntrySource();
            initEnded = false;
            OnAdLoaded += InitOnLoaded;
            Load();
            return trackEntrySource;
        }
        public void InitEnd()
        {
            initEnded = true;
        }
        private void InitOnLoaded(bool isSuccess)
        {
            if (!isSuccess)
                return;
            //
            initTrackEntrySource.CompleteSuccess();
            initTrackEntrySource = null;
            OnAdLoaded -= InitOnLoaded;
            //
            if (!initEnded)
                Show();
        }
        #endregion

        #region Method
        public override void Init()
        {
            if (IsInited)
                return;
            //
            if (setInstance)
                Instance = this;
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
            State = AdState.Inited;
            PushEvent_Inited();
        }
        public override void Load()
        {
            Init();
            //
            if (IsLoaded)
                return;
            //
            Ad_Create();
        }
        public override void Destroy()
        {

        }
        public override AdInterstitialTracking Show()
        {
            if (IsShow)
                return new AdInterstitialTrackingSource(ERROR_SHOW_FAIL_AD_IS_SHOWED);
            if (!IsReady)
                return new AdInterstitialTrackingSource(ERROR_SHOW_FAIL_AD_NOT_READY);
            //
            State = AdState.Show;
            adInterstitialTrackingSource = new AdInterstitialTrackingSource();
            adObject.Show();
            return adInterstitialTrackingSource;
        }
        private void Update_ExpireTime()
        {
            if (!IsLoaded || IsShow || DateTime.Now < expireTime)
                return;
            //
            Ad_Destroy();
            if (IsAutoReload)
                Ad_Create();
        }
        #endregion

        #region Ad
        private void Ad_Create()
        {
            if (isLoading)
                return;
            isLoading = true;
            attemptLoad = 0;
            //
            CoroutineManager.Instance.Coroutine_Start(Ad_LoadAd());
        }
        private void Ad_Destroy()
        {
            if (!IsLoaded)
                return;
            //
            adObject.Destroy();
            adObject = null;
            State = AdState.Inited;
        }
        private IEnumerator Ad_LoadAd()
        {
            if (attemptLoad == 0)
            {
                if (!AdMobManager.IsInit)
                    yield return new WaitForSecondsRealtime(2);
            }
            else
            {
                float delay = Mathf.Pow(2, attemptLoad);
                yield return new WaitForSecondsRealtime(delay);
            }
            //
            if (!AdMobManager.IsInit)
            {
                PushEvent_Loaded(false);
                yield break;
            }
            //
            AdRequest adRequest = new AdRequest();
            AppOpenAd.Load(AdId, adRequest, Ad_OnLoadComplete);
        }
        private void Ad_OnLoadComplete(AppOpenAd adObject, LoadAdError error)
        {
            isLoading = false;
            if (error != null || adObject == null)
            {
                attemptLoad = Mathf.Min(attemptLoad + 1, 6);
                Debug.LogError(string.Format(ERROR_LOAD_FAIL, error.GetMessage()));
                //
                PushEvent_Loaded(false);
                if (IsAutoReload)
                    Ad_Create();
                return;
            }
            attemptLoad = 0;
            //
            this.adObject = adObject;
            expireTime = DateTime.Now + TimeSpan.FromHours(AD_EXPIRE_HOUR);
            Ad_EventRegister();
            //
            State = AdState.Ready;
            PushEvent_Loaded(true);
        }
        private void Ad_EventRegister()
        {
            adObject.OnAdFullScreenContentOpened += Ad_OnFullScreenContentOpened;
            adObject.OnAdFullScreenContentFailed += Ad_OnFullScreenContentFailed;
            adObject.OnAdClicked += Ad_OnClicked;
            adObject.OnAdFullScreenContentClosed += Ad_OnFullScreenContentClosed;
            adObject.OnAdPaid += Ad_OnPaid;
            adObject.OnAdImpressionRecorded += Ad_OnImpressionRecorded;
        }
        private void Ad_OnFullScreenContentOpened()
        {
            PushEvent_Displayed(true);
            adInterstitialTrackingSource.Displayed(true);
        }
        private void Ad_OnFullScreenContentFailed(AdError adError)
        {
            State = AdState.Inited;
            Ad_Destroy();
            //
            PushEvent_Displayed(false);
            adInterstitialTrackingSource.Displayed(false);
            //
            if (IsAutoReload)
                Ad_Create();
        }
        private void Ad_OnClicked()
        {
            PushEvent_Clicked();
            adInterstitialTrackingSource.Clicked();
        }
        private void Ad_OnFullScreenContentClosed()
        {
            State = AdState.Inited;
            Ad_Destroy();
            //
            PushEvent_ShowComplete(true);
            adInterstitialTrackingSource.ShowComplete(true);
            //
            PushEvent_Hidden();
            adInterstitialTrackingSource.Hidden();
            //
            if (IsAutoReload)
                Ad_Create();
        }
        private void Ad_OnPaid(AdValue adValue)
        {
            AdRevenuePaid revenuePaid = new AdRevenuePaid(
                AdMobManager.ADMOB_SCOURCE,
                AdMobManager.ADMOB_SCOURCE,
                AdId,
                AdMobManager.ADMOB_COUNTRY_CODE,
                AdType,
                adValue.Value,
                adValue.CurrencyCode);
            //
            PushEvent_RevenuePaid(revenuePaid);
            adInterstitialTrackingSource.RevenuePaid(revenuePaid);
        }
        private void Ad_OnImpressionRecorded()
        {

        }
        #endregion
    }
}
