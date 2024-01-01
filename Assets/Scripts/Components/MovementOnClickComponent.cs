using System;
using System.Collections;
using Business.Entities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Components
{
    public class MovementOnClickComponent : MonoBehaviour
    {
        public InputActionAsset inputActionAsset;
        public CharacterComponent character;
        public float speed = 3.5f;
        public LayerMask clickableLayers;
        private InputAction _mouseClickAction;
        private NavMeshAgent _navMeshAgent;

        private Coroutine _moveCoroutine;

        private void Awake()
        {
            _mouseClickAction = inputActionAsset.FindAction("MouseClick");
            _mouseClickAction.performed += OnMouseClick;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = speed;
            character.Entity.Health.Resurrected += HealthOnResurrected;
            character.Entity.Health.Died += HealthOnDied;
        }

        private void HealthOnDied(object sender, EventArgs e)
        {
            _navMeshAgent.isStopped = true;
        }

        private void HealthOnResurrected(object sender, EventArgs e)
        {
            _navMeshAgent.ResetPath();
            _navMeshAgent.isStopped = false;
        }

        private void OnEnable()
        {
            _mouseClickAction.Enable();
        }

        private void OnDisable()
        {
            _mouseClickAction.Disable();
        }

        private void OnMouseClick(InputAction.CallbackContext context)
        {
            if (Camera.main == null || !character.Entity.Health.IsAlive) return;
            
            if (_moveCoroutine is not null)
            {
                StopCoroutine(_moveCoroutine);
                _navMeshAgent.ResetPath();
                _navMeshAgent.isStopped = true;
                _moveCoroutine = null;
            }

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayers))
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(hit.point);
                _moveCoroutine = StartCoroutine(WaitUntilFinishMove());
            }
        }
        
        private IEnumerator WaitUntilFinishMove()
        {
            yield return new WaitForFixedUpdate();
            while (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
            {
                yield return new WaitForFixedUpdate();
            }

            _navMeshAgent.ResetPath();
        }
    }
}