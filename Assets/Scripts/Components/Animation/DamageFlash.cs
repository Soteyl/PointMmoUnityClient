using System.Collections.Generic;
using Business.HealthPoints;
using Components.Entity;
using MEC;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Animation
{
    public class DamageFlash: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private EntityComponent _entity;

        [OdinSerialize]
        private MeshRenderer _meshRenderer;
        
        [OdinSerialize]
        private Color _flashColor = Color.white;
        
        [OdinSerialize]
        private float _flashDuration = 0.2f;
        
        private CoroutineHandle _flashCoroutine;

        private void Start()
        {
            _entity.Entity.Health.CurrentHealthUpdated += EntityOnDamaged;
        }

        private void EntityOnDamaged(object sender, HealthUpdatedArgs e)
        {
            if (e.NewHealth < e.OldHealth)
            {
                _flashCoroutine = Timing.RunCoroutineSingleton(_Flash().CancelWith(this), _flashCoroutine, SingletonBehavior.Abort);
            }
        }
        
        private IEnumerator<float> _Flash()
        {
            var material = _meshRenderer.material;
            var oldColor = material.color;
            float time = 0;
            
            while (time < _flashDuration)
            {
                material.color = Color.Lerp(_flashColor, oldColor, time / _flashDuration);
                time += Time.deltaTime;
                yield return Timing.WaitForOneFrame; 
            }

            material.color = oldColor;
        }
    }
}