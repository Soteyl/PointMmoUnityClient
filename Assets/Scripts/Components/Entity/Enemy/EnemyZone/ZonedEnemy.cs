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
        public CharacterTriggerRunner CharacterTriggerRunner { get; set; }
        
        [OdinSerialize]
        public Movement EnemyMovement { get; set; }

        private Vector3 _initialPosition;

        private void Start()
        {
            _initialPosition = transform.position;
            CharacterTriggerRunner.CharacterLeavedTrigger += CharacterTriggerOnCharacterLeavedTrigger;
        }

        private void CharacterTriggerOnCharacterLeavedTrigger(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            EnemyMovement.MoveTo(new MoveRequest()
            {
                    VectorTarget = _initialPosition
            });
        }
        
        private void OnDestroy()
        {
            CharacterTriggerRunner.CharacterLeavedTrigger -= CharacterTriggerOnCharacterLeavedTrigger;
        }
    }
}