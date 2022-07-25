using Game.Data;
using Game.Enums;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.PersistantProgress;
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
        public TileBox _tileBox;


        private void Awake()
        {
            InitInput();
            WeaponController = GetComponent<WeaponController>();
            _characterController = GetComponent<CharacterController>();
            _heroAnimator = GetComponent<HeroAnimator>();
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
            _characterController.Move(direction * MovementSpeed * speed * 0.01f * Time.deltaTime);
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
            if (_tileBox) _tileBox.GetDamage(() => StopAttack());
        }

        private void Update()
        {
            if (_heroAnimator.State == AnimatorState.Idle)
            {
                if (Physics.Raycast(Hips.position, transform.forward, out _hitInfo , 1f, 1 << 10))
                {
                    if (_hitInfo.collider.CompareTag("Tile"))
                    {
                        Attack();
                        _tileBox = _hitInfo.collider.gameObject.GetComponent<TileBox>();
                    }
                    else
                    {
                        StopAttack();
                        _tileBox = null;
                    }
                }
            }
        }

        private void StopAttack()
        {
            _heroAnimator.StopAttack();
        }


        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.WorldData.PositionOnLevel.Level == CurrentLevel())
            {
                Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
                if (savedPosition != null)
                {
                    Warp(to: savedPosition);
                }

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

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
            progress.PlayerData.WeaponType = WeaponController.CurrentWeapon.Type;
        }

        public void UpdateWeaponParams(float currentWeaponAttackSpeed)
        {
            _heroAnimator.SetAttackSpeed(currentWeaponAttackSpeed);
        }
    }
}