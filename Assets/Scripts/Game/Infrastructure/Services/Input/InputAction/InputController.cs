using System;
using System.Collections;
using Game.Logic.Services;
using Game.Utitlits;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace Core.Controllers.InputAction
{
    [DefaultExecutionOrder(-1)]
    public class InputController : MonoBehaviour, IInputService
    {
        #region Events

        public delegate void StartTouch(Vector3 position, float time);


        public Action<Vector3,float> OnStartTouch { get; set; }

        public Action<Vector3, float> OnEndTouch { get; set;}
        public Action<Vector3> OnMovedTouch { get; set;}
        public Vector2 Axis { get; set; }
        public Action<SwipeDirections> OnGetSwipe { get; set;}

        #endregion

        public InputSystemUIInputModule _inputSystemModule;

        public EventSystem _eventSystem;
        private Camera _mainCamera;
        private BaseAction _baseActions;

        public Camera MainCamera
        {
            get
            {
                if (!_mainCamera)
                {
                    _mainCamera = Camera.main;
                }
                return _mainCamera;
            }
        }


        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            _baseActions = new BaseAction();
            _mainCamera = Camera.main;
            if (_inputSystemModule == null)
            {
                _inputSystemModule = GetComponent<InputSystemUIInputModule>();
            }
            if (_eventSystem == null)
            {
                _eventSystem = GetComponent<EventSystem>();
            }
        }

        private void OnEnable()
        {
            _baseActions.Enable();
        }

        private void OnDisable()
        {
            _baseActions.Disable();
        }

        private void Start()
        {
            _baseActions.Touch.FirstTouch.started += ctx => TouchStarted(ctx);
            _baseActions.Touch.FirstTouch.canceled += ctx => TouchEnded(ctx);
        }

        private Coroutine debugCoroutine;
        private bool _clickStartInThisFrame = false;
        private bool _clickEndInThisFrame = false;
        private UnityEngine.InputSystem.InputAction.CallbackContext _currentFrameCtx;


        private void TouchStarted(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            _clickStartInThisFrame = true;
            _currentFrameCtx = ctx;
        }

        private void Update() 
        {
            if (_clickStartInThisFrame)
            {
                if (_eventSystem.IsPointerOverGameObject())
                {
                    //Debug.Log($"TouchStartOnUI");
                }
                else
                {
                    /*OnStartTouch?.Invoke(Utilits.GetPointFromCamera(_mainCamera,_baseActions.Touch.FirstTouchPosition.ReadValue<Vector2>()),(float)ctx.startTime);*/
                    OnStartTouch?.Invoke(_baseActions.Touch.FirstTouchPosition.ReadValue<Vector2>(), (float)_currentFrameCtx.startTime);
                    debugCoroutine = StartCoroutine(TouchMoved(_currentFrameCtx));
                }
                _clickStartInThisFrame = false;
                
            }
            if (_clickEndInThisFrame)
            {
                if (_eventSystem.IsPointerOverGameObject())
                {
                    //Debug.Log($"TouchendOnUi");
                }
                else
                {
                    //OnEndTouch?.Invoke(Utilits.GetPointFromCamera(_mainCamera, _baseActions.Touch.FirstTouchPosition.ReadValue<Vector2>()),(float)ctx.time);
                    
                }
                _clickEndInThisFrame = false;
            }
            
            
        }

        private IEnumerator TouchMoved(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            while(!_clickEndInThisFrame)
            {
                OnMovedTouch?.Invoke(_baseActions.Touch.FirstTouchPosition.ReadValue<Vector2>());
                yield return null;
            }
        }

        private void TouchEnded(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            _clickEndInThisFrame = true;
            _currentFrameCtx = ctx;
            OnEndTouch?.Invoke(_baseActions.Touch.FirstTouchPosition.ReadValue<Vector2>(), (float)_currentFrameCtx.startTime);
            if (debugCoroutine != null)
            {
                StopCoroutine(debugCoroutine);
            }
        }

        public void GetSwipe(SwipeDirections direction)
        {
            OnGetSwipe?.Invoke(direction);
        }

        public Vector3 TouchPosition() => Utilits.GetPointFromCamera(MainCamera, _baseActions.Touch.FirstTouchPosition.ReadValue<Vector2>());

        public Vector3 TouchPosition(out RaycastHit hit, LayerMask layerMask) => Utilits.GetPointFromCamera(MainCamera, _baseActions.Touch.FirstTouchPosition.ReadValue<Vector2>(), out hit, layerMask);

        public bool AttackButtonUp()
        {
            return false;
        }
    }
}
