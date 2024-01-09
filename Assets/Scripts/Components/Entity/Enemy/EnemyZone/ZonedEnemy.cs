using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Entity.Enemy.EnemyZone
{
    public class ZonedEnemy: SerializedMonoBehaviour
    {
        [OdinSerialize]
        public EnemyZone EnemyZone { get; set; }
        
        [OdinSerialize]
        public EnemyComponent Enemy { get; set; }
        
        [OdinSerialize]
        public ICharacterTrigger CharacterTrigger { get; set; }

        private Vector3 _initialPosition;

        private void Start()
        {
            _initialPosition = transform.position;
            CharacterTrigger.CharacterLeavedTrigger += CharacterTriggerOnCharacterLeavedTrigger;
        }

        private void CharacterTriggerOnCharacterLeavedTrigger(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            Enemy.Movement.MoveTo(_initialPosition);
        }
    }
}