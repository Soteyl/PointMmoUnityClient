using Business.Entities;
using Business.HealthPoints;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UI
{
    public class HealthBarComponent: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private Image _healthBarImage;
        
        [OdinSerialize]
        private EntityComponent _entityComponent;

        [OdinSerialize]
        private Camera _camera;
        
        private void Awake()
        {
            _camera ??= Camera.main;
            _entityComponent.Entity.Health.CurrentHealthUpdated += OnHealthChanged;
            UpdateBar();
        }

        private void Update()
        {
            var rotation = _camera.transform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        }

        private void OnHealthChanged(object sender, HealthUpdatedArgs e)
        {
            UpdateBar();
        }

        private void UpdateBar()
        {
            _healthBarImage.fillAmount = 1f / _entityComponent.Entity.Health.Max.GetValue() * _entityComponent.Entity.Health.Current;
        }
    }
}