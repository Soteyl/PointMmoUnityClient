using System;
using Common;
using Components.Entity.Character;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Entity.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class CharacterColliderTrigger: SerializedMonoBehaviour, ICharacterTrigger
    {
        private readonly CachedComponentResolver<CharacterComponent> _cachedCharacter = new(5);
        
        [OdinSerialize]
        private EnemyComponent _enemy;
        
        private CharacterComponent _currentCharacter;
        
        public event EventHandler<TriggeredCharacterEnemyEventArgs> CharacterTriggered; 

        public event EventHandler<TriggeredCharacterEnemyEventArgs> CharacterLeavedTrigger;

        private void Awake()
        {
            _enemy.Entity.Health.Died += OnDied;
        }

        private void OnDied(object sender, EventArgs e)
        {
            if (_currentCharacter is null)
                return;
            
            CharacterLeavedTrigger?.Invoke(this, new TriggeredCharacterEnemyEventArgs()
            {
                    Character = _currentCharacter
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_currentCharacter is not null || !_cachedCharacter.TryResolve(other, out var character)) return;
            
            _currentCharacter = character;
            CharacterTriggered?.Invoke(this, new TriggeredCharacterEnemyEventArgs
            {
                    Character = character
            });
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!_cachedCharacter.TryResolve(other, out var character) || character != _currentCharacter) return;
            
            _currentCharacter = null;
            CharacterLeavedTrigger?.Invoke(this, new TriggeredCharacterEnemyEventArgs
            {
                    Character = character
            });
        }
    }
}