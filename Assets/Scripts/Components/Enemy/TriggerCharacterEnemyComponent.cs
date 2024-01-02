using System;
using Business.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Enemy
{
    public class TriggerCharacterEnemyComponent: SerializedMonoBehaviour
    {
        public event EventHandler<TriggeredCharacterEnemyEventArgs> CharacterTriggered; 
        
        public event EventHandler<TriggeredCharacterEnemyEventArgs> CharacterLeavedTrigger; 

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<CharacterComponent>(out var character))
            {
                CharacterTriggered?.Invoke(this, new TriggeredCharacterEnemyEventArgs
                {
                    Character = character
                });
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<CharacterComponent>(out var character))
            {
                CharacterLeavedTrigger?.Invoke(this, new TriggeredCharacterEnemyEventArgs
                {
                    Character = character
                });
            }
        }
    }
    
    public class TriggeredCharacterEnemyEventArgs: EventArgs
    {
        public CharacterComponent Character { get; set; }
    }
}