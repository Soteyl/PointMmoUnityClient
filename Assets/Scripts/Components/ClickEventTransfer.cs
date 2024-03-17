using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Components.Interacting;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using RaycastHit = UnityEngine.RaycastHit;

namespace Components
{
    public class ClickEventTransfer: SerializedMonoBehaviour
    {
        private readonly CachedComponentResolver<InteractableObject> _cachedInteractable = new();

        [OdinSerialize]
        private InputActionAsset _inputActionAsset;

        [OdinSerialize]
        private LayerMask _interactableLayer;
        
        [OdinSerialize]
        private float _holdDelay = 0.2f;

        private InputAction _mouseClickAction;

        private Camera _mainCamera;

        private DateTime? _holdAt;
        
        private bool _isMainCameraNull;

        public event EventHandler<InteractableObjectEventArgs> InteractableObjectInvoked;
        
        public event EventHandler<InteractableObjectEventArgs> InteractableObjectReleased;
        
        public event EventHandler<InteractableObjectEventArgs> InteractableObjectHold;
        

        public static ClickEventTransfer FindInScene()
        {
            return GameObject.FindWithTag("EventSystem").GetComponent<ClickEventTransfer>();
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
            _isMainCameraNull = _mainCamera == null;
            _mouseClickAction = _inputActionAsset.FindAction("MouseClick");
            _mouseClickAction.started += OnMouseClick;
            _mouseClickAction.canceled += OnMouseRelease;
        }

        private void Update()
        {
            if (_holdAt.HasValue 
                && _holdAt.Value.AddSeconds(_holdDelay) < DateTime.Now
                && TryGetInteractableEventArgs(out var eventArgs))
                InteractableObjectHold?.Invoke(this, eventArgs);
        }

        private void OnMouseRelease(InputAction.CallbackContext obj)
        {
            _holdAt = null;
            if (!TryGetInteractableEventArgs(out var eventArgs)) return;
            InteractableObjectReleased?.Invoke(this, eventArgs);
        }

        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            if (!TryGetInteractableEventArgs(out var eventArgs)) return;
            _holdAt = DateTime.Now;
            InteractableObjectInvoked?.Invoke(this, eventArgs);
        }

        // ReSharper disable Unity.PerformanceAnalysis because it is optimized
        private bool TryGetInteractableEventArgs(out InteractableObjectEventArgs eventArgs)
        {
            eventArgs = null;
            if (_isMainCameraNull) return false;
            var hits = new RaycastHit[50];
            var mouseClickPosition = Mouse.current.position.ReadValue();
            var ray = _mainCamera.ScreenPointToRay(mouseClickPosition);
            var hitCount = Physics.RaycastNonAlloc(ray, hits, Mathf.Infinity, _interactableLayer);
            
            var raycastHits = hits.Take(hitCount)
                .Select(x => new InteractableObjectEventArgs.ObjectInteraction 
                { 
                        HitPosition = x.point, 
                        Object = _cachedInteractable.Resolve(x.transform) 
                })
                .OrderByDescending(x => x.Object.Type).Where(x => x.Object.IsInteractable).ToList();
            
            if (!raycastHits.Any()) return false;

            eventArgs = new InteractableObjectEventArgs
            {
                Interactions = raycastHits,
                MouseClickPosition = mouseClickPosition
            };

            return true;
        }

        private void OnEnable()
        {
            _mouseClickAction.Enable();
        }

        private void OnDisable()
        {
            _mouseClickAction.Disable();
        }
    }

    public class InteractableObjectEventArgs : EventArgs
    {
        /// <summary>
        /// Interactable objects in order from the closest to cursor to the furthest
        /// </summary>
        public IEnumerable<ObjectInteraction> Interactions { get; set; }
        
        public ObjectInteraction Interaction => Interactions.First();
        
        public Vector3 MouseClickPosition { get; set; }

        public class ObjectInteraction
        {
            public InteractableObject Object { get; set; }
            
            public Vector3 HitPosition { get; set; }
        }
    }
}