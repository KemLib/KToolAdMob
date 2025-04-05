using GoogleMobileAds.Api;
using KTool.Advertisement;
using KTool.Init;
using System;
using System.Collections;
using UnityEngine;

namespace KTool.GoogleAdmob
{
    public class AdMobAdBanner : AdBanner, IIniter
    {
        #region Properties
        private const string ERROR_LOAD_FAIL = "Ad Banner load fail: {0}",
            ERROR_SHOW_FAIL_AD_NOT_READY = "Ad Banner show fail: ad not ready",
            ERROR_SHOW_FAIL_AD_IS_SHOWED = "Ad Banner show fail: ad is show";
        private const int AD_EXPIRE_HOUR = 4;

        [SerializeField]
        private bool initRequiredConditions;
        [SerializeField]
        private bool setInstance;
        [SerializeField, SelectAdId(AdMobAdType.Banner)]
        private int indexAd = 0;
        [SerializeField]
        private bool showAfterInit = true;

        private bool isLoading;
        private int attemptLoad;
        private BannerView adObject;
        private DateTime expireTime;
        private TrackEntrySource initTrackEntrySource;
        private AdBannerTrackingSource adBannerTrackingSource;

        public event Action OnAdImpressionRecorded;

        public bool RequiredConditions => initRequiredConditions;
        public string AdId
        {
            get
            {
                AdMobSettingAdId settingAdId = AdMobSetting.Instance.Ad_Get(AdMobAdType.Banner, indexAd);
                if (settingAdId != null)
                    return settingAdId.AdID;
                return string.Empty;
            }
        }
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
        public override bool IsReady => base.IsReady && adObject != null;
        public override Advertisement.AdPosition PositionType
        {
            get => base.PositionType;
            protected set
            {
                if (value == base.PositionType)
                    return;
                //
                base.PositionType = value;
                if (adObject != null)
                    adObject.SetPosition(Utility.ConvertPosition(base.PositionType));
            }
        }
        public override Vector2 Position
        {
            get => base.Position;
            protected set
            {
                if (value == base.Position)
                    return;
                //
                base.Position = value;
                if (adObject != null)
                {
                    Vector2 point = Utility.Convert_UnityToAdMob(base.Position);
                    adObject.SetPosition((int)point.x, (int)point.y);
                }
            }
        }
        #endregion

        #region Unity Event
        private void Update()
        {
            Update_ExpireTime();
        }
        private void OnDestroy()
        {
            Ad_Destroy();
            State = AdState.Destroy;
            if (Instance != null && Instance.GetInstanceID() == GetInstanceID())
                Instance = null;
            PushEvent_Destroy();
        }
        #endregion

        #region Init
        public TrackEntry InitBegin()
        {
            TrackEntrySource trackEntrySource = initTrackEntrySource = new TrackEntrySource();
            OnAdLoaded += InitOnLoaded;
            Load();
            return trackEntrySource;
        }
        public void InitEnd()
        {
            if (showAfterInit && IsReady)
                Show();
        }
        private void InitOnLoaded(bool isSuccess)
        {
            if (!isSuccess)
                return;
            //
            initTrackEntrySource.CompleteSuccess();
            initTrackEntrySource = null;
            OnAdLoaded -= InitOnLoaded;
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
            State = AdState.Inited;
            PushEvent_Inited();
        }
        public override void Load()
        {
            Init();
            //
            Ad_Create();
        }
        public override void Destroy()
        {

        }
        public override AdBannerTracking Show()
        {
            if (IsShow)
                return new AdBannerTrackingSource(ERROR_SHOW_FAIL_AD_IS_SHOWED);
            if (!IsReady)
                return new AdBannerTrackingSource(ERROR_SHOW_FAIL_AD_NOT_READY);
            //
            State = AdState.Show;
            adBannerTrackingSource = new AdBannerTrackingSource();
            adObject.Show();
            return adBannerTrackingSource;
        }
        public override void Hide()
        {
            if (!IsShow)
                return;
            //
            State = AdState.Ready;
            adObject.Hide();
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
            if (adObject != null)
            {
                if (IsShow)
                    adObject.Hide();
                adObject.Destroy();
                adObject = null;
            }
            //
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
            Ad_Destroy();
            adObject = Utility.Create_AdBanner(AdId, SizeType, PositionType, Size, Position);
            adObject.OnBannerAdLoaded += Ad_OnLoaded;
            adObject.OnBannerAdLoadFailed += Ad_OnLoadFailed;
            Ad_EventRegister();
            //
            var adRequest = new AdRequest();
            adObject.LoadAd(adRequest);
        }
        private void Ad_OnLoaded()
        {
            isLoading = false;
            attemptLoad = 0;
            //
            expireTime = DateTime.Now + TimeSpan.FromHours(AD_EXPIRE_HOUR);
            State = AdState.Ready;
            adObject.Hide();
            PushEvent_Loaded(true);
        }
        private void Ad_OnLoadFailed(LoadAdError error)
        {
            isLoading = false;
            attemptLoad = Mathf.Min(attemptLoad + 1, 6);
            Debug.LogError(string.Format(ERROR_LOAD_FAIL, error.GetMessage()));
            //
            PushEvent_Loaded(false);
            if (IsAutoReload)
                Ad_Create();
        }
        private void Ad_EventRegister()
        {
            adObject.OnAdFullScreenContentOpened += Ad_OnFullScreenContentOpened;
            adObject.OnAdClicked += Ad_OnClicked;
            adObject.OnAdFullScreenContentClosed += Ad_OnFullScreenContentClosed;
            adObject.OnAdPaid += Ad_OnPaid;
            adObject.OnAdImpressionRecorded += Ad_OnImpressionRecorded;
        }
        private void Ad_OnFullScreenContentOpened()
        {
            PushEvent_Displayed(true);
            adBannerTrackingSource.Displayed(true);
        }
        private void Ad_OnClicked()
        {
            PushEvent_Clicked();
            adBannerTrackingSource.Clicked();
        }
        private void Ad_OnFullScreenContentClosed()
        {
            State = AdState.Inited;
            Ad_Destroy();
            //
            PushEvent_ShowComplete(true);
            adBannerTrackingSource.ShowComplete(true);
            //
            PushEvent_Hidden();
            adBannerTrackingSource.Hidden();
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
            adBannerTrackingSource.RevenuePaid(revenuePaid);
        }
        private void Ad_OnImpressionRecorded()
        {
            OnAdImpressionRecorded?.Invoke();
        }
        #endregion
    }
}
