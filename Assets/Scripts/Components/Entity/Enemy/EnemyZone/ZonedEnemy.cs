using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Entity.Enemy.EnemyZone
{
    public class ZonedEnemy: SerializedMonoBehaviour
    {
        [OdinSerialize]
        public EnemyZoneSpawner EnemyZoneSpawner { get; set; }
        
        [OdinSerialize]
        public EnemyComponent Enemy { get; set; }

        private Vector3 _initialPosition;

        private void Start()
        {
            _initialPosition = transform.position;
            Enemy.CharacterTrigger.CharacterLeavedTrigger += CharacterTriggerOnCharacterLeavedTrigger;
        }

        private void CharacterTriggerOnCharacterLeavedTrigger(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            Enemy.Movement.MoveTo(_initialPosition);
        }
    }
}