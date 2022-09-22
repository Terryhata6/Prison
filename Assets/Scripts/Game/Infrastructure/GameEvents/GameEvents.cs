using System;
using UnityEngine;

namespace Game.Infrastructure.GameEvents
{
    public class GameEvents : MonoBehaviour
    {
        private static GameEvents _instance;
        public static GameEvents Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameEvents>(true);
                }

                return _instance;
            }
            set
            {
                _instance = value;
            }
            
        }
        
        
        public static bool RewardedIsReady = false;
        public static bool InterIsReady = false;

        public Action<string> OnInterstitialRequest;

        public void InterstitialRequest(string placement)
        {
            OnInterstitialRequest?.Invoke(placement);
        }


        public Action<bool> OnInterstitialReadyStateChange;
        public void InterstitialReadyState(bool value)
        {
            OnInterstitialReadyStateChange?.Invoke(value);
            InterIsReady = value;
        }

        public Action<bool> OnRewardedReadyStateChange;
        public void RewardedReadyState(bool b)
        {
            OnRewardedReadyStateChange?.Invoke(b);
            RewardedIsReady = b;
        }

        public event Action<Action, string> OnRewardedRequest;

        public void RewardedRequest(Action onComplete, string placement)
        {
            OnRewardedRequest?.Invoke(onComplete,placement);
            OnRewardedRequest = null;
        }

        public event Action OnCallMediationDebugger;
        public void CallMediationDebugger()
        {
            OnCallMediationDebugger?.Invoke();
        }
        
    }
}