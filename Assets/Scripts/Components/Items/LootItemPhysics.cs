using Components.Interacting;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Items
{
    [RequireComponent(typeof(Collider))]
    public class LootItemPhysics: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private Rigidbody _rigidbody;

        [OdinSerialize]
        private LootItem _lootItem;

        [OdinSerialize]
        private Vector2 _throwForce;
        
        [OdinSerialize]
        private float fallMultiplier = 2.5f; 

        public void Throw()
        {
            var xDirection = Random.Range(-1f, 1f);
            var zDirection = Random.Range(-1f, 1f);
            var throwDirection = new Vector3(xDirection, 1, zDirection).normalized;

            _lootItem.IsInteractable = false;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(throwDirection * _throwForce, ForceMode.Impulse);
        }
        
        private void FixedUpdate()
        {
            if (_rigidbody.velocity.y < 0)
            {
                _rigidbody.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime);
            }
        }
        
        private void OnTriggerEnter(Collider collision)
        {
            if (!collision.TryGetComponent<InteractableObject>(out var interactable)
                || interactable.Type != InteractableObjectType.Floor
                || _rigidbody.isKinematic) return;

            _lootItem.IsInteractable = true;
            _rigidbody.velocity = Vector3.zero;       
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;           
        }
    }
}