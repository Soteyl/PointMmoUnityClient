using System;
using System.Collections;
using Business.Entities;
using Components;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

namespace Business.HealthPoints
{
    public class RespawnerComponent : SerializedMonoBehaviour
    {
        [OdinSerialize]
        public CharacterComponent Character { get; set; }
        
        [OdinSerialize]
        public TextMeshProUGUI Text { get; set; }
        
        [OdinSerialize]
        public SpawnPointComponent SpawnPoint { get; set; }

        private Rigidbody _characterRigidbody;
        

        private void Start()
        {
            Character.Entity.Health.Died += CharacterOnDied;
            _characterRigidbody = Character.GetComponent<Rigidbody>();
        }

        private void CharacterOnDied(object sender, EventArgs e)
        {
            StartCoroutine(WaitBeforeResurrect());
        }

        private IEnumerator WaitBeforeResurrect()
        {
            for (int i = 3; i > 0; i--)
            {
                Text.text = i.ToString();
                yield return new WaitForSeconds(1);
            }

            Text.text = "";
            Character.Entity.Health.Resurrect();
            Character.transform.position = SpawnPoint.Point;
            _characterRigidbody.velocity = new Vector3();
        }

        private void OnEnable()
        {
            Character.Entity.Health.Died += CharacterOnDied;
        }

        private void OnDisable()
        {
            Character.Entity.Health.Died -= CharacterOnDied;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<SpawnPointComponent>(out var spawnPoint))
            {
                SpawnPoint = spawnPoint;
            }
        }
    }
}