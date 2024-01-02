using System;
using System.Collections;
using UnityEngine;

namespace Business.Entities
{
    public class EnemyComponent: EntityComponent
    {
        public EnemyComponent() : base(new Entity())
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