using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Entity.Enemy
{
    public class AttackCharacterOnTriggerComponent: SerializedMonoBehaviour
    {
        [OdinSerialize]
        public CharacterTriggerRunner CharacterTriggerRunner { get; set; }

        [OdinSerialize]
        private EnemyCharacterAttacker _characterAttacker;

        private void Awake()
        {
            CharacterTriggerRunner.CharacterTriggered += OnCharacterTriggered;
            CharacterTriggerRunner.CharacterLeavedTrigger += OnCharacterLeavedCharacterTrigger;
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