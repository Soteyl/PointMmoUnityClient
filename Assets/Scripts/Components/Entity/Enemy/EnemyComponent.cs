using System;
using System.Collections;
using Business.Entities;
using UnityEngine;

namespace Components.Entity.Enemy
{
    public class EnemyComponent: EntityComponent
    {
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