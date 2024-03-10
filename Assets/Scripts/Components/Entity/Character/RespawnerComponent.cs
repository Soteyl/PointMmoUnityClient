using System;
using System.Collections;
using System.Collections.Generic;
using Business.HealthPoints;
using MEC;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

namespace Components.Entity.Character
{
    public class RespawnerComponent : SerializedMonoBehaviour
    {
        [OdinSerialize]
        public CharacterComponent Character { get; set; }
        
        [OdinSerialize]
        public TextMeshProUGUI Text { get; set; }
        
        [OdinSerialize]
        public SpawnPointComponent SpawnPoint { get; set; }
        

        private void Start()
        {
            Character.Entity.Health.Died += CharacterOnDied;
        }

        private void CharacterOnDied(object sender, EventArgs e)
        {
            Timing.RunCoroutine(_WaitBeforeResurrect());
        }

        private IEnumerator<float> _WaitBeforeResurrect()
        {
            for (int i = 3; i > 0; i--)
            {
                Text.text = i.ToString();
                yield return Timing.WaitForSeconds(1);
            }

            Text.text = "";
            Character.Entity.Health.Resurrect();
            Character.transform.position = SpawnPoint.Point;
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
