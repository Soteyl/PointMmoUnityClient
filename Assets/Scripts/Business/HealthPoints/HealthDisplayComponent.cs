using Business.Entities;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;

namespace Business.HealthPoints
{
    public class HealthDisplayComponent : SerializedMonoBehaviour
    {
        private Health Health => EntityComponent?.Entity.Health;

        [OdinSerialize]
        public EntityComponent EntityComponent { get; set; }

        [OdinSerialize]
        public TextMeshProUGUI Text { get; set; }

        [ShowInInspector] [ShowIf(nameof(EntityComponent))]
        public float CurrentHealth => Health?.Current ?? 0;

        [ShowInInspector] [ShowIf(nameof(EntityComponent))]
        public float MaxHealth => Health?.Max.GetValue() ?? 0;

        [OdinSerialize] [ShowIf(nameof(EntityComponent))]
        public float DefaultMaxHealth => Health?.Max.DefaultValue ?? 100;

        [Button] [ShowIf(nameof(EntityComponent))]
        public void Heal()
        {
            Health.Current = Health.Max.GetValue();
        }

        private void Start()
        {
            if (EntityComponent && isActiveAndEnabled)
            {
                Subscribe();
                UpdateText(Health);
            }
        }

        private void OnEnable()
        {
            if (EntityComponent)
                Subscribe();
        }

        private void OnDisable()
        {
            if (EntityComponent)
                Unsubscribe();
        }

        private void Subscribe()
        {
            Health.CurrentHealthUpdated += OnHealthUpdated;
            Health.MaxHealthUpdated += OnHealthUpdated;
        }
        
        private void Unsubscribe()
        {
            Health.CurrentHealthUpdated -= OnHealthUpdated;
            Health.MaxHealthUpdated -= OnHealthUpdated;
        }

        private void OnHealthUpdated(object sender, HealthUpdatedArgs e)
        {
            UpdateText(e.Health);
        }

        private void UpdateText(Health health)
        {
            Text.text = $"{(int)health.Current}/{(int)health.Max.GetValue()}";
        }
    }
}