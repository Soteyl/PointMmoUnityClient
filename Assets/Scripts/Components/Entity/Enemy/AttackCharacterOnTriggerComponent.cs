using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Entity.Enemy
{
    public class AttackCharacterOnTriggerComponent: SerializedMonoBehaviour
    {
        private ICharacterTrigger _characterTrigger;

        [OdinSerialize]
        public ICharacterTrigger CharacterTrigger
        {
            get => _characterTrigger;
            set
            {
                if (_characterTrigger == value || value == null) return;

                if (_characterTrigger != null)
                {
                    _characterTrigger.CharacterTriggered -= OnCharacterTriggered;
                    _characterTrigger.CharacterLeavedTrigger -= OnCharacterLeavedCharacterTrigger;
                }

                _characterTrigger = value;
                _characterTrigger.CharacterTriggered += OnCharacterTriggered;
                _characterTrigger.CharacterLeavedTrigger += OnCharacterLeavedCharacterTrigger;
            }
        }

        [OdinSerialize]
        private EnemyCharacterAttacker _characterAttacker;
        

        private void Awake()
        {
            CharacterTrigger.CharacterTriggered += OnCharacterTriggered;
            CharacterTrigger.CharacterLeavedTrigger += OnCharacterLeavedCharacterTrigger;
        }

        private void OnCharacterLeavedCharacterTrigger(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            _characterAttacker.StopAttack();
        }

        private void OnCharacterTriggered(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            _characterAttacker.StartAttack(e.Character);
        }
        
        
    }
}