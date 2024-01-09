using System;

namespace Components.Entity.Enemy
{
    public interface ICharacterTrigger
    {
        event EventHandler<TriggeredCharacterEnemyEventArgs> CharacterTriggered; 
        
        event EventHandler<TriggeredCharacterEnemyEventArgs> CharacterLeavedTrigger; 
    }
}