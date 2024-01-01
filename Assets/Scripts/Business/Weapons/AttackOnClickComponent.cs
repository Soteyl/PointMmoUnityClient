using System.Collections;
using System.Threading;
using Business.Entities;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Business.Weapons
{
    public class AttackOnClickComponent: SerializedMonoBehaviour
    {
        [OdinSerialize]
        public CharacterComponent Character { get; set; }
        
        [OdinSerialize]
        public NavMeshAgent NavMeshAgent { get; set; }
        
        [OdinSerialize]
        public InputActionAsset InputActionAsset { get; set; }
        
        [OdinSerialize]
        public LayerMask ClickableLayers { get; set; }
        
        private IWeapon Weapon => Character?.Entity.Weapon;

        private new Transform transform => Character?.transform;

        private InputAction _inputAction;

        private Coroutine _attackCoroutine;

        private CancellationTokenSource _attackCoroutineCancellationTokenSource;

        private void Awake()
        {
            _inputAction = InputActionAsset.FindAction("MouseClick");
            _inputAction.performed += InputActionOnPerformed;
        }
        
        private Vector3 GetPointNearTarget(EntityComponent targetEntity)
        {
            var position = targetEntity.transform.position;
            Vector3 directionToEntity = (position - transform.position).normalized;
            Vector3 destinationPoint = position - directionToEntity * (Weapon.Distance - 0.5f);
            return destinationPoint;
        }

        private void InputActionOnPerformed(InputAction.CallbackContext obj)
        {
            StopAttackCoroutine();
            
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ClickableLayers) && hit.transform.TryGetComponent<EntityComponent>(out var entity)) // todo add radius of entity
            {
                NavMeshAgent.ResetPath();
                if (Vector3.Distance(transform.position, entity.transform.position) > Weapon.Distance)
                {
                    Vector3 destinationPoint = GetPointNearTarget(entity);
                    NavMeshAgent.SetDestination(destinationPoint); 
                }
                _attackCoroutine = StartCoroutine(WaitUntilAttack(entity));
            }
        }

        private IEnumerator WaitUntilAttack(EntityComponent target)
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);
            while (distance > Weapon.Distance)
            {
                yield return new WaitForFixedUpdate();
                distance = Vector3.Distance(transform.position, target.transform.position);
            }

            _attackCoroutineCancellationTokenSource = new CancellationTokenSource();

            NavMeshAgent.SetDestination(transform.position);

            Character.Entity.AttackAsync(target.Entity, _attackCoroutineCancellationTokenSource.Token);
        }

        private void StopAttackCoroutine()
        {
            if (_attackCoroutine is null) return;
            
            StopCoroutine(_attackCoroutine);
            _attackCoroutineCancellationTokenSource?.Cancel();
            _attackCoroutine = null;
            _attackCoroutineCancellationTokenSource = null;
        }
    }
}