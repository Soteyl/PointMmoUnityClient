using System;
using System.Collections.Generic;
using System.Linq;
using Components.Entity.Character;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Entity.Enemy.EnemyZone
{
    public class EnemyZone: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private List<EnemySpawn> _entities;
        
        [OdinSerialize]
        private float _spawnRadius = 5f;

        [OdinSerialize]
        private bool _attackTogether = true;

        private List<ZonedEnemy> _spawnedEntities = new();
        
        private int _triggeredEnemiesCount;
        
        private CustomCharacterTrigger _characterTrigger = new ();

        private bool IsTriggeredFirst => _triggeredEnemiesCount == 1;

        private void Awake()
        {
            SpawnEntities();
        }

        private void SpawnEntities()
        {
            var allSpawnedEntities = _entities.SelectMany(x => Enumerable.Repeat(x.Prefab, x.Count)).ToList();
            var angleStep = 360f / allSpawnedEntities.Count;
            var angle = 0f;
            
            foreach (var entity in allSpawnedEntities)
            {
                var spawnedEntity = Instantiate(entity, CountSpawnPosition(angle), Quaternion.identity);
                _spawnedEntities.Add(AddZonedEnemyComponent(spawnedEntity));
                angle += angleStep;
            }
        }

        private ZonedEnemy AddZonedEnemyComponent(GameObject obj)
        {
            var zonedEnemy = obj.AddComponent<ZonedEnemy>();
            zonedEnemy.EnemyZone = this;
            zonedEnemy.Enemy = obj.GetComponent<EnemyComponent>();

            if (_attackTogether)
            {
                zonedEnemy.CharacterTrigger = _characterTrigger;
                zonedEnemy.Enemy.CharacterTrigger.CharacterTriggered += EnemyTriggered;
                zonedEnemy.Enemy.CharacterTrigger.CharacterLeavedTrigger += EnemyLeavedTrigger;
                zonedEnemy.Enemy.CharacterAttackOnTrigger.CharacterTrigger = _characterTrigger;
            }
            else
            {
                zonedEnemy.CharacterTrigger = zonedEnemy.Enemy.CharacterTrigger;
            }

            return zonedEnemy;
        }

        private void EnemyLeavedTrigger(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            _triggeredEnemiesCount--;
            
            if (_triggeredEnemiesCount == 0)
            {
                _characterTrigger.LeaveTrigger(e.Character);
            }
        }

        private void EnemyTriggered(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            _triggeredEnemiesCount++;

            if (IsTriggeredFirst)
            {
                _characterTrigger.Trigger(e.Character);
            }
        }

        private Vector3 CountSpawnPosition(float angle)
        {
            return new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * _spawnRadius,
                0, 
                Mathf.Sin(angle * Mathf.Deg2Rad) * _spawnRadius
            ) + transform.position;
        }
    }

    public class CustomCharacterTrigger: ICharacterTrigger
    {
        public event EventHandler<TriggeredCharacterEnemyEventArgs> CharacterTriggered;

        public event EventHandler<TriggeredCharacterEnemyEventArgs> CharacterLeavedTrigger;

        public void Trigger(CharacterComponent characterComponent)
        {
            CharacterTriggered?.Invoke(this, new TriggeredCharacterEnemyEventArgs()
            {
                    Character = characterComponent
            });
        }
        
        public void LeaveTrigger(CharacterComponent characterComponent)
        {
            CharacterLeavedTrigger?.Invoke(this, new TriggeredCharacterEnemyEventArgs()
            {
                Character = characterComponent
            });
        }
    }

    public class EnemySpawn
    {
        [OdinSerialize]
        public GameObject Prefab { get; set; }
        
        [OdinSerialize]
        public int Count { get; set; }
    }
}