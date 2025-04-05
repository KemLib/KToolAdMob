using KTool.Advertisement;
using TMPro;
using UnityEngine;

namespace KTool.GoogleAdmob.Example
{
    public class PanelAdBanner : MonoBehaviour
    {
        #region Properties
        private const string CLICK_INIT = "Ad Banner: Click init",
            CLICK_LOAD = "Ad Banner: Click load",
            CLICK_SHOW = "Ad Banner: Click show";
        private const string AD_EVENT_INIT = "Ad Banner: even Init",
            AD_EVENT_LOADED = "Ad Banner: even Loaded {0}",
            AD_EVENT_DISPLAYED = "Ad Banner: even Displayed {0}",
            AD_EVENT_CLICKED = "Ad Banner: even Clicked",
            AD_EVENT_SHOW_COMPLETE = "Ad Banner: even ShowComplete {0}",
            AD_EVENT_HIDDEN = "Ad Banner: even Hidden",
            AD_EVENT_REVENUEPAID = "Ad Banner: even RevenuePaid {0}-{1}",
            AD_EVENT_DESTROY = "Ad Banner: even Destroy",
            AD_EVENT_EXPANDED = "Ad Banner: even Expanded {0}";
        private const string ERROR_ADD_EMPTY = "Ad Banner: No objects to select",
            ERROR_AD_IS_INITED = "Ad Banner: ad is inited",
            ERROR_AD_IS_NOT_INIT = "Ad Banner: ad not init",
            ERROR_AD_IS_NOT_LOAD = "Ad Banner: ad not load",
            ERROR_AD_IS_NOT_READY = "Ad Banner: ad not ready",
            ERROR_AD_IS_SHOW = "Ad Banner: ad is showed",
            ERROR_AD_IS_NOT_SHOW = "Ad Banner: ad not show";

        [SerializeField]
        private TMP_Dropdown dropdownAd;

        private PanelLog panelLog;
        private AdMobAdBanner selectAd;

        private AdMobManager manager => AdMobManager.Instance;
        public bool IsShow => gameObject.activeSelf;
        public int Count => dropdownAd.options.Count;
        public AdMobAdBanner SelectAd => selectAd;
        #endregion

        #region Unity Events

        #endregion

        #region Methods
        public void Init(PanelLog panelLog)
        {
            this.panelLog = panelLog;
            //
            if (manager == null || manager.Banner_Count() == 0)
            {
                dropdownAd.options.Clear();
                return;
            }
            //
            dropdownAd.options.Clear();
            int count = manager.Banner_Count();
            for (int i = 0; i < count; i++)
            {
                AdMobAdBanner ad = manager.Banner_Get(i);
                dropdownAd.options.Add(new TMP_Dropdown.OptionData(ad.Name));
            }
            dropdownAd.value = 0;
        }
        public void Show()
        {
            if (IsShow)
                return;
            //
            if (Count == 0)
            {
                panelLog.AddLog(ERROR_ADD_EMPTY);
                return;
            }
            gameObject.SetActive(true);
            OnSelectAd(dropdownAd.value);
        }
        public void Hide()
        {
            if (!IsShow)
                return;
            //
            gameObject.SetActive(false);
            SelectAd_EventUnRegister();
            selectAd = null;
        }
        #endregion

        #region Unity Events
        public void OnSelectAd(int value)
        {
            SelectAd_EventUnRegister();
            selectAd = manager.Banner_Get(value);
            SelectAd_EventRegister();
        }
        public void OnClick_Init()
        {
            if (!IsShow)
                return;
            //
            panelLog.AddLog(CLICK_INIT);
            //
            if (SelectAd.IsInited)
                panelLog.AddLog(ERROR_AD_IS_INITED);
            else
                SelectAd.Init();
        }
        public void OnClick_Load()
        {
            if (!IsShow)
                return;
            //
            panelLog.AddLog(CLICK_LOAD);
            //
            if (!SelectAd.IsInited)
                panelLog.AddLog(ERROR_AD_IS_NOT_INIT);
            else
                SelectAd.Load();
        }
        public void OnClick_Show()
        {
            if (!IsShow)
                return;
            //
            panelLog.AddLog(CLICK_SHOW);
            //
            if (!SelectAd.IsInited)
                panelLog.AddLog(ERROR_AD_IS_NOT_INIT);
            else if (!SelectAd.IsLoaded)
                panelLog.AddLog(ERROR_AD_IS_NOT_LOAD);
            else if (SelectAd.IsShow)
                panelLog.AddLog(ERROR_AD_IS_SHOW);
            else if (!SelectAd.IsReady)
                panelLog.AddLog(ERROR_AD_IS_NOT_READY);
            else
                SelectAd.Show();
        }
        public void OnClick_Hide()
        {
            if (!IsShow)
                return;
            //
            panelLog.AddLog(CLICK_SHOW);
            //
            if (!SelectAd.IsShow)
                panelLog.AddLog(ERROR_AD_IS_NOT_SHOW);
            else
                SelectAd.Hide();
        }
        #endregion

        #region Ad Event
        private void SelectAd_EventRegister()
        {
            if (selectAd == null)
                return;
            //
            selectAd.OnAdInited += SelectAd_OnAdInited;
            selectAd.OnAdLoaded += SelectAd_OnAdLoaded;
            selectAd.OnAdDisplayed += SelectAd_OnAdDisplayed;
            selectAd.OnAdClicked += SelectAd_OnAdClicked;
            selectAd.OnAdShowComplete += SelectAd_OnAdShowComplete;
            selectAd.OnAdHidden += SelectAd_OnAdHidden;
            selectAd.OnAdRevenuePaid += SelectAd_OnAdRevenuePaid;
            selectAd.OnAdDestroy += SelectAd_OnAdDestroy;
            selectAd.OnAdExpanded += SelectAd_OnAdExpanded;
        }
        private void SelectAd_EventUnRegister()
        {
            if (selectAd == null)
                return;
            //
            selectAd.OnAdInited -= SelectAd_OnAdInited;
            selectAd.OnAdLoaded -= SelectAd_OnAdLoaded;
            selectAd.OnAdDisplayed -= SelectAd_OnAdDisplayed;
            selectAd.OnAdClicked -= SelectAd_OnAdClicked;
            selectAd.OnAdShowComplete -= SelectAd_OnAdShowComplete;
            selectAd.OnAdHidden -= SelectAd_OnAdHidden;
            selectAd.OnAdRevenuePaid -= SelectAd_OnAdRevenuePaid;
            selectAd.OnAdDestroy -= SelectAd_OnAdDestroy;
        }
        private void SelectAd_OnAdInited()
        {
            panelLog.AddLog(AD_EVENT_INIT);
        }
        private void SelectAd_OnAdLoaded(bool isSuccess)
        {
            panelLog.AddLog(string.Format(AD_EVENT_LOADED, isSuccess));
        }
        private void SelectAd_OnAdDisplayed(bool isSuccess)
        {
            panelLog.AddLog(string.Format(AD_EVENT_DISPLAYED, isSuccess));
        }
        private void SelectAd_OnAdClicked()
        {
            panelLog.AddLog(AD_EVENT_CLICKED);
        }
        private void SelectAd_OnAdShowComplete(bool isSuccess)
        {
            panelLog.AddLog(string.Format(AD_EVENT_SHOW_COMPLETE, isSuccess));
        }
        private void SelectAd_OnAdHidden()
        {
            panelLog.AddLog(AD_EVENT_HIDDEN);
        }
        private void SelectAd_OnAdRevenuePaid(AdRevenuePaid revenuePaid)
        {
            panelLog.AddLog(string.Format(AD_EVENT_REVENUEPAID, revenuePaid.Value, revenuePaid.Currency));
        }
        private void SelectAd_OnAdDestroy()
        {
            panelLog.AddLog(AD_EVENT_DESTROY);
        }

        private void SelectAd_OnAdExpanded(bool isExpanded)
        {
            panelLog.AddLog(string.Format(AD_EVENT_EXPANDED, isExpanded));
        }
        #endregion
    }
}
