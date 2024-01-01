using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Components.Interacting;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Components
{
    public class ClickEventTransfer: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private InputActionAsset _inputActionAsset;

        [OdinSerialize]
        private LayerMask _interactableLayer;

        private InputAction _mouseClickAction;

        public event EventHandler<InteractableObjectInvokedEventArgs> InteractableObjectInvoked;

        public static ClickEventTransfer FindInScene()
        {
            return GameObject.FindWithTag("EventSystem").GetComponent<ClickEventTransfer>();
        }

        private void Awake()
        {
            _mouseClickAction = _inputActionAsset.FindAction("MouseClick");
            _mouseClickAction.performed += OnMouseClick;
        }

        private void OnMouseClick(InputAction.CallbackContext obj)
        {
            if (Camera.main == null) return;

            var mouseClickPosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mouseClickPosition);
            RaycastHit hit;

            var raycast = Physics.RaycastAll(ray, Mathf.Infinity, _interactableLayer)
                .Select(x => new { Point = x.point, Interactable = x.transform.GetComponent<InteractableObject>() }).OrderBy(x => x.Interactable.Type).FirstOrDefault();
            if (raycast is not null)
            {
                InteractableObjectInvoked?.Invoke(this, new InteractableObjectInvokedEventArgs
                {
                    Object = raycast.Interactable,
                    MouseClickPosition = mouseClickPosition,
                    HitPosition = raycast.Point
                });
            }
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

    public class InteractableObjectInvokedEventArgs : EventArgs
    {
        public InteractableObject Object { get; set; }
        
        public Vector3 MouseClickPosition { get; set; }
        
        public Vector3 HitPosition { get; set; }
    }
}