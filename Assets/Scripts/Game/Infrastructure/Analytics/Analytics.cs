using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Unity;
using GameAnalyticsSDK;
using UnityEngine;

namespace Game.Infrastructure.Analytics
{
    public class Analytics : MonoBehaviour, IAnalytics, IGameAnalyticsATTListener
    {
        private bool _enableAppMetrika = true;
        private float _lastADSTime = 0f;
        public float adsCooldown = 30f;
        
        private void Start()
        { 
            
            GameAnalyticsInitialize();
            FacebookInitialize();
            CheckAttFacebook();
            SubscribeGameAnalyticEvent();
            return;
            //Invoke("ShowMediationDebuggerUI", 10f);
            MaxSdk.SetSdkKey("6AQkyPv9b4u7yTtMH9PT40gXg00uJOTsmBOf7hDxa_-FnNZvt_qTLnJAiKeb5-2_T8GsI_dGQKKKrtwZTlCzAR");
            MaxSdk.InitializeSdk();
            
            InitializeInterstitialAds();
            InitializeBannerAds();
            InitializeRewardedAds();
        }

        public void ShowMediationDebuggerUI()
        {
            MaxSdk.ShowMediationDebugger();
        }

        #region GameAnalytics

        private void GameAnalyticsInitialize()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                GameAnalytics.RequestTrackingAuthorization(this);
            }
            GameAnalytics.Initialize();
        }


        private void SubscribeGameAnalyticEvent()
        {
            
        }

        private int tempInt;
        private float timerStart = 0f;
        //private bool _levelWasLongerThenBenchmark = false;
        //private Dictionary<string, object> _data = new Dictionary<string, object>();

        public void OnLevelStart()
        {
            tempInt = PlayerPrefs.GetInt("TriesPlayed", 0);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, $"{tempInt}");
            //GameAnalytics.NewDesignEvent("LevelStart", tempInt);
            timerStart = Time.unscaledTime;
#if UNITY_EDITOR
            Debug.LogWarning($"LevelStart:{tempInt}");
            
#endif
        }

        public void OnLevelFailed()
        {
            tempInt = PlayerPrefs.GetInt("TriesPlayed", 0);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, $"{tempInt}");
            //GameAnalytics.NewDesignEvent("LevelFailed", tempInt);
#if UNITY_EDITOR
            Debug.LogWarning($"LevelFailed:{tempInt}");
#endif
        }

        public void OnLevelVictory()
        {
            tempInt = PlayerPrefs.GetInt("TriesPlayed", 0);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, $"{tempInt}");
            //GameAnalytics.NewDesignEvent("LevelVictory", tempInt);
#if UNITY_EDITOR
            Debug.LogWarning($"LevelVictory:{tempInt}");
            
#endif
            
            /*if (Time.unscaledTime - timerStart > 35f)
            {
                _levelWasLongerThenBenchmark = true;
            }
            else
            {
                _levelWasLongerThenBenchmark = false;
            }*/
            /*_data.Clear();
            _data.Add("TriesPlayed", tempInt);
            GameAnalytics.NewDesignEvent("LevelData", _data);*/
        }

        #endregion

        #region FacebookSDK

        private void FacebookInitialize()
        {
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
        }

        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
            }
            else
            {
                Debug.LogWarning("Failed to Initialize the Facebook SDK");
            }
#if UNITY_IOS
            Invoke(nameof(RegisterAppForNetworkAttribution), 1);
#endif
        }

#if UNITY_IOS
        private void RegisterAppForNetworkAttribution()
        {
            SkAdNetworkBinding.SkAdNetworkRegisterAppForNetworkAttribution();
        }
#endif

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        private async void CheckAttFacebook()
        {
            await FaseboockSuckTask();
        }

        private Task FaseboockSuckTask()
        {
            Task task = new Task(() =>
            {
#if UNITY_IPHONE
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    FB.Mobile.SetAdvertiserTrackingEnabled(UnityEngine.iOS.Device.advertisingTrackingEnabled);
                }
                //Debug.Log("TASKENDED");
#endif
            });
            task.Start();
            return task;
        }

        #endregion

        #region Events

        private int tempLevelCount;
        private int tempCurrentLevelNumber;
        private int UpscaleLevel;
        Dictionary<string, object> _data = new Dictionary<string, object>();


        private int StartTime = 0;
        private int EndTime = 0;
        
        public void OnVideoAdsAvailable(AdsType type, string placement, bool result)
        {
            _data.Clear();

            if (type == AdsType.interstitial)
            {
                _data.Add("ad_type", "interstitial");
            }
            else if (type == AdsType.reward)
            {
                _data.Add("ad_type", "rewarded");
            }

            _data.Add("placement", placement);
            _data.Add("result", result ? "success" : "not_available");
            if (_enableAppMetrika)
            {
                AppMetrica.Instance.ReportEvent("video_ads_available", _data);
            }
        }

        public void OnVideoAdsStarted(AdsType type, string placement, bool result)
        {
            _data.Clear();


            if (type == AdsType.interstitial)
            {
                _data.Add("ad_type", "interstitial");
            }
            else if (type == AdsType.reward)
            {
                _data.Add("ad_type", "rewarded");
            }

            _data.Add("placement", placement);
            _data.Add("result", "started");
            if (_enableAppMetrika)
            {
                AppMetrica.Instance.ReportEvent("video_ads_started", _data);
            }
        }

        public void OnVideoAdsWatch(AdsType type, string placement, bool result)
        {
            tempCurrentLevelNumber = PlayerPrefs.GetInt("CurrentLevel", 0) + 1;
            UpscaleLevel = PlayerPrefs.GetInt("UpscaleLevel", 0);
            tempLevelCount = tempCurrentLevelNumber + UpscaleLevel * 10;

            _data.Clear();
            _data.Add("level_number", tempCurrentLevelNumber);
            _data.Add("level_count", tempLevelCount);
            _data.Add("level_loop", UpscaleLevel);

            if (type == AdsType.interstitial)
            {
                _data.Add("ad_type", "interstitial");
            }
            else if (type == AdsType.reward)
            {
                _data.Add("ad_type", "rewarded");
            }

            _data.Add("placement", placement);
            _data.Add("result", "started");

            if (_enableAppMetrika)
            {
                AppMetrica.Instance.ReportEvent("video_ads_watch", _data);
            }
        }

        #endregion
        
        
         #region ApplovinInters

        string _adInterUnitId = "2d3784de34b7b0ae";
        int _intersRetryAttempt;

        public void InitializeInterstitialAds()
        {
            // Attach callback
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
            GameEvents.GameEvents.Instance.OnInterstitialRequest += ShowInterstitial;
            // Load the first interstitial
            LoadInterstitial();
        }

        private void LoadInterstitial()
        {
            GameEvents.GameEvents.Instance.InterstitialReadyState(false);
            MaxSdk.LoadInterstitial(_adInterUnitId);
        }

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

            // Reset retry attempt
            _intersRetryAttempt = 0;
            OnVideoAdsAvailable(AdsType.interstitial, "endlevel", true);
            GameEvents.GameEvents.Instance.InterstitialReadyState(true);
        }

        private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to load 
            // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

            _intersRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, _intersRetryAttempt));
            OnVideoAdsAvailable(AdsType.interstitial, "endlevel", false);
            Invoke("LoadInterstitial", (float) retryDelay);
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            OnVideoAdsStarted(AdsType.interstitial, "endlevel", true);
        }

        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
            LoadInterstitial();
        }

        private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is hidden. Pre-load the next ad.
            LoadInterstitial();
            OnVideoAdsWatch(AdsType.interstitial, "endlevel", true);
        }

        public void ShowInterstitial(string placement)
        {
            if ((Time.unscaledTime - _lastADSTime) < adsCooldown)
            {
                return;
            }
            if (MaxSdk.IsInterstitialReady(_adInterUnitId))
            {
                MaxSdk.ShowInterstitial(_adInterUnitId);
                _lastADSTime = Time.unscaledTime;
            }
        }

        #endregion

        #region ApplovinRewarded

        string _adRewardedUnitId = "45cff65cf878156f";
        int _rewardedRetryAttempt;

        public void InitializeRewardedAds()
        {
            // Attach callback
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            GameEvents.GameEvents.Instance.OnRewardedRequest += ShowRewardedAd;
            // Load the first rewarded ad
            LoadRewardedAd();
        }

        private void LoadRewardedAd()
        {
            GameEvents.GameEvents.Instance.RewardedReadyState(false);
            MaxSdk.LoadRewardedAd(_adRewardedUnitId);
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

            // Reset retry attempt
            _rewardedRetryAttempt = 0;
            OnVideoAdsAvailable(AdsType.reward, "mainMenu", true);
            GameEvents.GameEvents.Instance.RewardedReadyState(true);
        }

        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Rewarded ad failed to load 
            // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

            _rewardedRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, _rewardedRetryAttempt));
            OnVideoAdsAvailable(AdsType.reward, "mainMenu", false);
            Invoke("LoadRewardedAd", (float) retryDelay);
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            OnVideoAdsStarted(AdsType.reward, "mainMenu", true);
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
            LoadRewardedAd();
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is hidden. Pre-load the next ad
            LoadRewardedAd();
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            // The rewarded ad displayed and the user should receive the reward.
            OnVideoAdsWatch(AdsType.reward, "mainmenu", true);
            _currentRewardedInstance?.Invoke();
        }

        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Ad revenue paid. Use this callback to track user revenue.
        }

        private Action _currentRewardedInstance;

        public void ShowRewardedAd(Action rewardedInstance, string placement)
        {
            if (MaxSdk.IsRewardedAdReady(_adRewardedUnitId))
            {
                _currentRewardedInstance = null;
                _currentRewardedInstance += rewardedInstance;
                MaxSdk.ShowRewardedAd(_adRewardedUnitId);
                _lastADSTime = Time.unscaledTime;
            }
        }

        #endregion

        #region AppLovinBanner

        string _bannerAdUnitId = "ebd9d666109e4ca4"; // Retrieve the ID from your account

        public void InitializeBannerAds()
        {
            // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
            // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
            MaxSdk.CreateBanner(_bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

            // Set background or background color for banners to be fully functional
            MaxSdk.SetBannerBackgroundColor(_bannerAdUnitId, Color.gray);

            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
        }

        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
        }

        private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
        }

        #endregion
        
        

        public void GameAnalyticsATTListenerNotDetermined()
        {
            GameAnalytics.Initialize();
        }

        public void GameAnalyticsATTListenerRestricted()
        {
            GameAnalytics.Initialize();
        }

        public void GameAnalyticsATTListenerDenied()
        {
            GameAnalytics.Initialize();
        }

        public void GameAnalyticsATTListenerAuthorized()
        {
            GameAnalytics.Initialize();
        }
    }
    
    public enum AdsType
    {
        interstitial,
        reward
    }
}