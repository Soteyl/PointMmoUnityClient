using Business.HealthPoints;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;

namespace Components
{
    public class HealthDisplayComponent : SerializedMonoBehaviour
    {
        private Health Health => EntityComponent?.Entity.Health;

        [OdinSerialize]
        public EntityComponent EntityComponent { get; set; }

        [OdinSerialize]
        public TextMeshProUGUI Text { get; set; }

        [ShowInInspector] [ShowIf(nameof(EntityComponent))]
        public int CurrentHealth => Health?.CurrentPoints ?? 0;

        [ShowInInspector] [ShowIf(nameof(EntityComponent))]
        public int MaxHealth => Health?.MaxPoints ?? 0;

        [OdinSerialize] [ShowIf(nameof(EntityComponent))]
        public int DefaultMaxHealth
        {
            get => Health?.DefaultPoints ?? 100;
            set => Health?.SetDefaultMaxPoints(value);
        }

        [Button] [ShowIf(nameof(EntityComponent))]
        public void Heal()
        {
            Health.CurrentPoints = Health.MaxPoints;
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
            Text.text = $"{health.CurrentPoints}/{health.MaxPoints}";
        }
    }
}