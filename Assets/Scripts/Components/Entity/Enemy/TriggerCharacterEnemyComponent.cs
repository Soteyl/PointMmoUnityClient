using System;
using Common;
using Components.Entity.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Entity.Enemy
{
    public class TriggerCharacterEnemyComponent: SerializedMonoBehaviour
    {
        private readonly CachedComponentResolver<CharacterComponent> _cachedCharacter = new();

        public event EventHandler<TriggeredCharacterEnemyEventArgs> CharacterTriggered; 
        
        public event EventHandler<TriggeredCharacterEnemyEventArgs> CharacterLeavedTrigger; 

        private void OnTriggerEnter(Collider other)
        {
            if (_cachedCharacter.TryResolve(other, out var character))
            {
                CharacterTriggered?.Invoke(this, new TriggeredCharacterEnemyEventArgs
                {
                    Character = character
                });
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (_cachedCharacter.TryResolve(other, out var character))
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