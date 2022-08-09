using System.Collections.Generic;
using System.Threading.Tasks;
using Facebook.Unity;
using GameAnalyticsSDK;
using UnityEngine;

namespace Game.Infrastructure.Analytics
{
    public class Analytics : MonoBehaviour, IAnalytics, IGameAnalyticsATTListener
    {
        private void Start()
        {
            GameAnalyticsInitialize();
            FacebookInitialize();
            CheckAttFacebook();
            SubscribeGameAnalyticEvent();
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
            /*
            if (Time.unscaledTime - timerStart > 35f)
            {
                _levelWasLongerThenBenchmark = true;
            }
            else
            {
                _levelWasLongerThenBenchmark = false;
            }*/
            /*_data.Clear();
            _data.Add("LevelNumber", tempInt);
            _data.Add("LongerThenBenchmark", _levelWasLongerThenBenchmark);
            GameAnalytics.NewDesignEvent("LevelData", _data);*/
            //GameAnalytics.NewDesignEvent($"LongerThenBenchmark{tempInt}", _levelWasLongerThenBenchmark ? 1 : 0);
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
}