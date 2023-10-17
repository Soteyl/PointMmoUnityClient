using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Components
{
    public class ClickMovement : MonoBehaviour
    {
        public InputActionAsset inputActionAsset;
        public CharacterComponent character;
        public float speed = 3.5f;
        public LayerMask clickableLayers;
        private InputAction mouseClickAction;
        private NavMeshAgent navMeshAgent;

        void Awake()
        {
            mouseClickAction = inputActionAsset.FindAction("MouseClick");
            mouseClickAction.performed += OnMouseClick;
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = speed;
            character.Entity.Health.Resurrected += HealthOnResurrected;
        }

        private void HealthOnResurrected(object sender, EventArgs e)
        {
            navMeshAgent.SetDestination(transform.position);
        }

        void OnEnable()
        {
            mouseClickAction.Enable();
        }

        void OnDisable()
        {
            mouseClickAction.Disable();
        }

        void OnMouseClick(InputAction.CallbackContext context)
        {
            if (Camera.main == null) return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayers))
            {
                navMeshAgent.SetDestination(hit.point);
            }
        }
    }
}