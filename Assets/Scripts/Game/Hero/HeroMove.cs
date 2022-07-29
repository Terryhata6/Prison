using System;
using Game.Data;
using Game.Enums;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.PersistantProgress;
using Game.Infrastructure.Services.SaveLoad;
using Game.Infrastructure.States;
using Game.Logic;
using Game.Logic.EventIndicator;
using Game.Logic.Services;
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
        public string NextLevel = "";
        
        private HeroLootTracker _heroLootTracker;
        private Vector3 _movementVector3 = Vector3.zero;
        private readonly Vector3 _gravity = new Vector3(0,-9.81f,0);
        private PlayerCurrency _playerCurrency;
        private EventIndicator currentEventIndicator;


        private void Awake()
        {
            SetNextLevel("Lobby");
            AllServices.Container.Single<ISaveLoadService>().SaveProgress();
            
            InitInput();
            WeaponController = GetComponent<WeaponController>();
            _characterController = GetComponent<CharacterController>();
            _heroAnimator = GetComponent<HeroAnimator>();
            _heroLootTracker = GetComponentInChildren<HeroLootTracker>();
            _heroLootTracker.Init(this);
            _playerCurrency = GetComponent<PlayerCurrency>();
        }

        private void InitInput()
        {
            _input = AllServices.Container.Single<IInputService>();
            _input.OnStartTouch += OnStartTouch;
            _input.OnEndTouch += OnEndTouch;
            _input.OnMovedTouch += OnMovedTouch;
        }
        private void OnStartTouch(Vector3 position, float time) => _currentPosition = _startPosition = position;
        private void OnMovedTouch(Vector3 position)
        {
            _currentPosition = position;
            Vector3 direction = (_currentPosition - _startPosition).normalized;
            Vector3 directionTransformed = new Vector3(direction.x, 0, direction.y);
            float distance = Vector3.Distance(_currentPosition, _startPosition);
            float speed = Mathf.Clamp(distance,0, 100);
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
        }

        private void CheckEventIndicators()
        {
            if (currentEventIndicator)
            {
                currentEventIndicator.FillProgress(this);
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


        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.WorldData.PositionOnLevel.Level == CurrentLevel())
            {/*
                Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
                if (savedPosition != null)
                {
                    Warp(to: savedPosition);
                }*/

                if (progress.PlayerData != null)
                {
                    WeaponController.SetCurrentWeapon(progress.PlayerData.WeaponType, this);
                }
                else
                {
                    WeaponController.SetCurrentWeapon(WeaponController.CurrentWeapon.Type, this);
                }
            }
        }

        public void SetNextLevel(string value)
        {
            NextLevel = value;
        }
        
        public void UpdateProgress(PlayerProgress progress, string currentLevel = null)
        {
            if (NextLevel != currentLevel)
            {
                currentLevel = NextLevel;
            }
            progress.WorldData.PositionOnLevel = new PositionOnLevel(currentLevel, transform.position.AsVectorData());
            progress.PlayerData.WeaponType = WeaponController.CurrentWeapon.Type;
        }

        public void EscapedFromCave()
        {
            EndLevelData Data = new EndLevelData();
            Data.EarnedCash = _heroLootTracker.GetCurrency();
            Data.CopCash = 0;
            Data.EscapeResult = true;
            Data.IsLevelLobby = false;
            OnLevelEnded?.Invoke(Data);
        }

        public void ReturnToJail()
        {
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
            EndLevelData Data = new EndLevelData();
            Data.IsLevelLobby = true;
            OnLevelEnded?.Invoke(Data);
        }

        public event Action<EndLevelData> OnLevelEnded;

        public void UpdateWeaponParams(float currentWeaponAttackSpeed) => _heroAnimator.SetAttackSpeed(currentWeaponAttackSpeed);

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EventIndicator"))
            {
                currentEventIndicator = other.GetComponent<EventIndicator>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("EventIndicator"))
            {
                if (other.GetComponent<EventIndicator>() == currentEventIndicator)
                {
                    currentEventIndicator.StopFillingProgress();
                    currentEventIndicator = null;
                }
            }
        }

        private void OnDestroy()
        {
            _input.OnStartTouch -= OnStartTouch;
            _input.OnEndTouch -= OnEndTouch;
            _input.OnMovedTouch -= OnMovedTouch;
        }

        public void AddMoney(float objEarnedCash)
        {
            _playerCurrency.AddCurrency(objEarnedCash);
        }
    }
}