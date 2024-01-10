using System;
using Components.Entity.Character;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Entity.Enemy
{
    public class CharacterTriggerRunner: SerializedMonoBehaviour
    {
        private ICharacterTrigger _trigger;

        [OdinSerialize]
        public ICharacterTrigger Strategy
        {
            get => _trigger;
            set
            {
                if (_trigger == value)
                    return;
                
                if (_trigger is not null)
                {
                    _trigger.CharacterLeavedTrigger -= TriggerOnCharacterLeavedTrigger;
                    _trigger.CharacterTriggered -= TriggerOnCharacterTriggered;
                }

                _trigger = value;
                _trigger.CharacterTriggered += TriggerOnCharacterTriggered;
                _trigger.CharacterLeavedTrigger += TriggerOnCharacterLeavedTrigger;
            }
        }

        public event EventHandler<TriggeredCharacterEnemyEventArgs> CharacterTriggered; 
        
        public event EventHandler<TriggeredCharacterEnemyEventArgs> CharacterLeavedTrigger;
        

        private void TriggerOnCharacterTriggered(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            CharacterTriggered?.Invoke(this, e);
        }

        private void TriggerOnCharacterLeavedTrigger(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            CharacterLeavedTrigger?.Invoke(this, e);
        }
    }

    public class TriggeredCharacterEnemyEventArgs: EventArgs
    {
        public CharacterComponent Character { get; set; }
    }
}