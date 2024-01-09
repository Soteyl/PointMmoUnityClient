using System;
using System.Collections;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Entity.Enemy
{
    public class EnemyComponent: EntityComponent
    {
        [OdinSerialize]
        public ICharacterTrigger CharacterTrigger { get; private set; }
        
        [OdinSerialize]
        public EnemyCharacterAttacker CharacterAttacker { get; private set; }
        
        [OdinSerialize]
        public AttackCharacterOnTriggerComponent CharacterAttackOnTrigger { get; private set; }
        
        public EnemyComponent() : base(new Business.Entities.Entity())
        {
            Entity.Health.Died += HealthOnDied;
        }

        private void HealthOnDied(object sender, EventArgs e)
        {
            StartCoroutine(DieAfterAll());
        }

        private IEnumerator DieAfterAll()
        {
            yield return new WaitForFixedUpdate();
            Destroy(gameObject);
        }
    }
}