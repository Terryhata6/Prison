using System;
using Game.Data;
using Game.Enums;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.PersistantProgress;
using Game.Infrastructure.Services.SaveLoad;
using Game.Infrastructure.States;
using Game.Logic;
using Game.Logic.Cop;
using Game.Logic.EventIndicator;
using Game.Logic.InGameLoot;
using Game.Logic.Services;
using Game.UI.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Hero
{
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        public float MovementSpeed = 10f;
        private CharacterController _characterController;
        private IInputService _input;
        private HeroAnimator _heroAnimator;
        private Vector3 _startPosition;
        private Vector3 _currentPosition;
        private RaycastHit _hitInfo;

        public WeaponController WeaponController;
        public Transform Hips;
        public TileBox CurrentTileBox;
        public string NextLevel = "Lobby";
        
        private HeroLootTracker _heroLootTracker;
        private Vector3 _movementVector3 = Vector3.zero;
        private readonly Vector3 _gravity = new Vector3(0f,-9.81f,0f);
        public PlayerCurrency Currency;
        private EventIndicator _currentEventIndicator;
        private IUIService _uiService;
        private float _copTimer = 30f;
        private bool _copTimerStarted = false;
        public float MaximumCopTimer = 30f;


        private void Awake()
        {
            SetNextLevel("Lobby");
            _uiService = AllServices.Container.Single<IUIService>();
            InitInput();
            WeaponController = GetComponent<WeaponController>();
            _characterController = GetComponent<CharacterController>();
            _heroAnimator = GetComponent<HeroAnimator>();
            _heroLootTracker = GetComponentInChildren<HeroLootTracker>();
            _heroLootTracker.Init(this, _uiService);
            Currency = GetComponent<PlayerCurrency>();
            AllServices.Container.Single<ISaveLoadService>().SaveProgress();

            
        }

        private void InitInput()
        {
            _input = AllServices.Container.Single<IInputService>();
            ActivateHeroMovement();
        }

        private void ActivateHeroMovement()
        {
            _input.OnStartTouch += OnStartTouch;
            _input.OnEndTouch += OnEndTouch;
            _input.OnMovedTouch += OnMovedTouch;
        }
        private void DectivateHeroMovement()
        {
            _input.OnStartTouch -= OnStartTouch;
            _input.OnEndTouch -= OnEndTouch;
            _input.OnMovedTouch -= OnMovedTouch;
        }

        private void OnStartTouch(Vector3 position, float time) => _currentPosition = _startPosition = position;

        private void OnMovedTouch(Vector3 position)
        {
            _currentPosition = position;
            Vector3 direction = (_currentPosition - _startPosition).normalized;
            Vector3 directionTransformed = new Vector3(direction.x, 0f, direction.y);
            float distance = Vector3.Distance(_currentPosition, _startPosition);
            float speed = Mathf.Clamp(distance,0f, 100f);
            if (distance > 0)
            {
                MoveCharacter(directionTransformed, speed);
            }
        }

        private void OnEndTouch(Vector3 position, float time) => _heroAnimator.StopMoving();

        private void MoveCharacter(Vector3 direction, float speed)
        {
            _movementVector3 = direction * MovementSpeed * speed * 0.01f * Time.deltaTime;
            
            transform.LookAt(transform.position + direction, Vector3.up);
            _heroAnimator.Move(speed * 0.01f);
        }

        private void Warp(Vector3Data to)
        {
            _characterController.enabled = false;
            transform.position = to.AsUnityVector().AddY(_characterController.height);
            _characterController.enabled = true;
        }

        private static string CurrentLevel() => SceneManager.GetActiveScene().name;


        private void Attack() => _heroAnimator.PlayAttack();

        public void CheckHit()
        {
            if (CurrentTileBox) CurrentTileBox.GetDamage(StopAttack);
        }

        private void Update()
        {
            MovingHero();
            CheckAttack();
            CheckEventIndicators();
            CopTimerTick();
        }

        private void CopTimerTick()
        {
            if (_copTimerStarted)
            {
                _copTimer -= Time.deltaTime;
                _uiService.UpdateCopUi(_copTimer, MaximumCopTimer);
                if (_copTimer <= 0)
                {
                    _copTimerStarted = false;
                    _copTimer = 0;
                    FindObjectOfType<CopView>(true).gameObject.SetActive(true);
                }
            }
        }

        private void CheckEventIndicators()
        {
            if (_currentEventIndicator)
            {
                _currentEventIndicator.FillProgress(this);
            }
        }

        private void CheckAttack()
        {
            if (_heroAnimator.State != AnimatorState.Dead)
            {
                if (Physics.Raycast(Hips.position, transform.forward, out _hitInfo, 1f, 1 << 10))
                {
                    if (_hitInfo.collider.CompareTag("Tile"))
                    {
                        Attack();
                        CurrentTileBox = _hitInfo.collider.gameObject.GetComponent<TileBox>();
                    }
                    else
                    {
                        StopAttack();
                        CurrentTileBox = null;
                    }
                }
                else
                {
                    StopAttack();
                    CurrentTileBox = null;
                }
            }
        }

        private void MovingHero()
        {
            _characterController.Move(_movementVector3 + _gravity);
            _movementVector3 = Vector3.zero;
        }

        private void StopAttack() => _heroAnimator.StopAttack();


        private void ActivateExitNavigationArrow()
        {
            PlayerPrefs.SetInt("MapBuyed", 0);
            var navigationArrow = GetComponentInChildren<NavigationArrow>();
            if(navigationArrow)
                navigationArrow.ActivateNavigationToCaveExit(this);
            else
            {
                Debug.Log($"{navigationArrow} Arrow is not exist");
            }
        }

        #region Cop

        private void SetCopTimer(float playerDataCopDelayTime)
        {
            MaximumCopTimer = _copTimer = playerDataCopDelayTime;
        }

        private void StartCopTimer()
        {
            _copTimerStarted = true;
        }

        public void UpgradeCopDelayTimer(float timeDelay, int price)
        {
            Currency.SpendMoney(price);
            MaximumCopTimer += timeDelay;
            AllServices.Container.Single<ISaveLoadService>().SaveProgress();
        }
        #endregion

        #region SaveLoad

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.WorldData.PositionOnLevel.Level == CurrentLevel())
            {
                if (progress.PlayerData != null)
                {
                    WeaponController.SetCurrentWeapon(progress.PlayerData.weaponWeaponType, this);
                    _heroLootTracker.SetMaximumInventorySize(progress.PlayerData.MaximumStackSize);
                    SetCopTimer(progress.PlayerData.CopDelayTime);
                }
                else
                {
                    WeaponController.SetCurrentWeapon(WeaponController.CurrentWeapon.Type, this);
                    _heroLootTracker.SetMaximumInventorySize(10);
                }
                Debug.Log("PlayerProgress Loaded");
            }
        }

        public void UpdateProgress(PlayerProgress progress, string currentLevel = null)
        {
            progress.WorldData.PositionOnLevel = new PositionOnLevel(NextLevel, transform.position.AsVectorData());
            progress.PlayerData.weaponWeaponType = WeaponController.CurrentWeapon.Type;
            progress.PlayerData.CopDelayTime = MaximumCopTimer;
            Debug.Log("PlayerProgress Saved");
        }

        #endregion

        public void InitCaveLevelStarted()
        {
            StartCopTimer();
            if (PlayerPrefs.GetInt("MapBuyed", 0) > 0)
            {
                ActivateExitNavigationArrow();
            }
        }

        public void SetNextLevel(string value)
        {
            NextLevel = value;
        }

        #region OnLevelEndScenarios

        public void EscapedFromCave()
        {
            PlayerPrefs.SetInt("JailRoom", 1);
            DectivateHeroMovement();
            EndLevelData Data = new EndLevelData();
            Data.EarnedCash = _heroLootTracker.GetCurrency();
            Data.CopCash = 0f;
            Data.EscapeResult = true;
            Data.IsLevelLobby = false;
            OnLevelEnded?.Invoke(Data);
        }

        public void ReturnToJail()
        {
            DectivateHeroMovement();
            EndLevelData Data = new EndLevelData();
            float cashValue = _heroLootTracker.GetCurrency();
            Data.EarnedCash = cashValue * 0.6f;
            Data.CopCash = cashValue * 0.4f;
            Data.EscapeResult = false;
            Data.IsLevelLobby = false;
            OnLevelEnded?.Invoke(Data);
        }

        public void GoToCave()
        {
            DectivateHeroMovement();
            EndLevelData Data = new EndLevelData();
            Data.IsLevelLobby = true;
            OnLevelEnded?.Invoke(Data);
        }


        public event Action<EndLevelData> OnLevelEnded;

        #endregion

        #region UnityEvents

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EventIndicator"))
            {
                _currentEventIndicator = other.GetComponent<EventIndicator>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("EventIndicator"))
            {
                if (other.GetComponent<EventIndicator>() == _currentEventIndicator)
                {
                    _currentEventIndicator.StopFillingProgress();
                    _currentEventIndicator = null;
                }
            }
        }

        private void OnDestroy()
        {
            _input.OnStartTouch -= OnStartTouch;
            _input.OnEndTouch -= OnEndTouch;
            _input.OnMovedTouch -= OnMovedTouch;
        }

        #endregion

        #region Inventory

        public void AddMoney(float objEarnedCash)
        {
            Currency.AddCurrency(objEarnedCash);
        }

        public void SpendMoney(int spoonPrice)
        {
            Currency.SpendMoney(spoonPrice);
        }

        public void CollectCurrentInventory(Action<LootContainer> action)
        {
            StartCoroutine(_heroLootTracker.CollectCurrentCurrency(action));
        }

        public void ActivateNewWeapon(WeaponType type, int price)
        {
            WeaponController.SetCurrentWeapon(type, this);
            Currency.SpendMoney(price);
        }

        public void UpdateWeaponParams(float currentWeaponAttackSpeed) => _heroAnimator.SetAttackSpeed(currentWeaponAttackSpeed);

        #endregion
    }
}