using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KTool.Advertisement;

namespace KTool.GoogleAdmob.Example
{
    public class PanelAdRewarded : MonoBehaviour
    {
        #region Properties
        private const string CLICK_INIT = "Ad Rewarded: Click init",
            CLICK_LOAD = "Ad Rewarded: Click load",
            CLICK_SHOW = "Ad Rewarded: Click show";
        private const string AD_EVENT_INIT = "Ad Rewarded: even Init",
            AD_EVENT_LOADED = "Ad Rewarded: even Loaded {0}",
            AD_EVENT_DISPLAYED = "Ad Rewarded: even Displayed {0}",
            AD_EVENT_CLICKED = "Ad Rewarded: even Clicked",
            AD_EVENT_SHOW_COMPLETE = "Ad Rewarded: even ShowComplete {0}",
            AD_EVENT_HIDDEN = "Ad Rewarded: even Hidden",
            AD_EVENT_REVENUE_PAID = "Ad Rewarded: even RevenuePaid {0}-{1}",
            AD_EVENT_DESTROY = "Ad Rewarded: even Destroy",
            AD_EVENT_RECEIVED_REWARD = "Ad Rewarded: even ReceivedReward {0}-{1}";
        private const string ERROR_ADD_EMPTY = "Ad Rewarded: No objects to select",
            ERROR_AD_IS_INITED = "Ad Rewarded: ad is inited",
            ERROR_AD_IS_NOT_INIT = "Ad Rewarded: ad not init",
            ERROR_AD_IS_LOADED = "Ad Rewarded: ad is loaded",
            ERROR_AD_IS_NOT_LOAD = "Ad Rewarded: ad not load",
            ERROR_AD_IS_NOT_READY = "Ad Rewarded: ad not ready",
            ERROR_AD_IS_SHOW = "Ad Rewarded: ad is showed";

        [SerializeField]
        private TMP_Dropdown dropdownAd;

        private PanelLog panelLog;
        private AdMobAdRewarded selectAd;

        private AdMobManager manager => AdMobManager.Instance;
        public bool IsShow => gameObject.activeSelf;
        public int Count => dropdownAd.options.Count;
        public AdMobAdRewarded SelectAd => selectAd;
        #endregion

        #region Unity Events

        #endregion

        #region Methods
        public void Init(PanelLog panelLog)
        {
            this.panelLog = panelLog;
            //
            if (manager == null || manager.Rewarded_Count() == 0)
            {
                dropdownAd.options.Clear();
                return;
            }
            //
            dropdownAd.options.Clear();
            int count = manager.Rewarded_Count();
            for (int i = 0; i < count; i++)
            {
                AdMobAdRewarded ad = manager.Rewarded_Get(i);
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
            selectAd = manager.Rewarded_Get(value);
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
            else if (SelectAd.IsLoaded)
                panelLog.AddLog(ERROR_AD_IS_LOADED);
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
            else if (!SelectAd.IsReady)
                panelLog.AddLog(ERROR_AD_IS_NOT_READY);
            else if (SelectAd.IsShow)
                panelLog.AddLog(ERROR_AD_IS_SHOW);
            else
                SelectAd.Show();
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
            selectAd.OnAdReceivedReward += SelectAd_OnAdReceivedReward;
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
            selectAd.OnAdReceivedReward -= SelectAd_OnAdReceivedReward;
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
            panelLog.AddLog(string.Format(AD_EVENT_REVENUE_PAID, revenuePaid.Value, revenuePaid.Currency));
        }
        private void SelectAd_OnAdDestroy()
        {
            panelLog.AddLog(AD_EVENT_DESTROY);
        }
        private void SelectAd_OnAdReceivedReward(AdRewardReceived rewardReceived)
        {
            panelLog.AddLog(string.Format(AD_EVENT_RECEIVED_REWARD, rewardReceived.Label, rewardReceived.Value));
        }
        #endregion
    }
}
