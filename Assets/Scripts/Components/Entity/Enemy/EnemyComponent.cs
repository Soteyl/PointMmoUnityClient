using System;
using System.Collections;
using UnityEngine;

namespace Components.Entity.Enemy
{
    public class EnemyComponent: EntityComponent
    {
        public Vector3 SpawnPoint { get; private set; }

        public EnemyComponent() : base(new Business.Entities.Entity())
        {
            Entity.Health.Died += HealthOnDied;
        }

        protected override void Start()
        {
            base.Start();   
            SpawnPoint = transform.position;
        }

        /// <summary>
        /// Workaround for Destroying after applying all events
        /// </summary>
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