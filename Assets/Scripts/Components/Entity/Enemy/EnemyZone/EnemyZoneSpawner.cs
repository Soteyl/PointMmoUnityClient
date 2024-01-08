using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Entity.Enemy.EnemyZone
{
    public class EnemyZoneSpawner: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private List<EnemySpawn> _entities;
        
        [OdinSerialize]
        private float _spawnRadius = 5f;
                
        private List<GameObject> _spawnedEntities = new();
        
        private void Awake()
        {
            var allSpawnedEntities = _entities.SelectMany(x => Enumerable.Repeat(x.Prefab, x.Count)).ToList();
            var angleStep = 360f / allSpawnedEntities.Count;
            var position = transform.position;
            var angle = 0f;
            
            foreach (var entity in allSpawnedEntities)
            {
                var spawnPosition = new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad) * _spawnRadius,
                    0, 
                    Mathf.Sin(angle * Mathf.Deg2Rad) * _spawnRadius
                ) + position;
                
                var spawnedEntity = Instantiate(entity, spawnPosition, Quaternion.identity);
                var zonedEnemy = spawnedEntity.AddComponent<ZonedEnemy>();
                zonedEnemy.EnemyZoneSpawner = this;
                zonedEnemy.Enemy = spawnedEntity.GetComponent<EnemyComponent>();
                _spawnedEntities.Add(spawnedEntity);
                angle += angleStep;
            }
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