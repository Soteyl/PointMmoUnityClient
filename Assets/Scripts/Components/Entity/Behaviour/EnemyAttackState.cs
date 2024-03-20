using Components.Entity.Character;
using Components.Entity.Enemy;

namespace Components.Entity.Behaviour
{
    public class EntityAttackState: IEntityBehaviourState
    {
        private readonly CharacterComponent _entity;
        private readonly EnemyBehaviour _context;
        private readonly EnemyHub _hub;

        public EntityAttackState(CharacterComponent entity, EnemyBehaviour context, EnemyHub hub)
        {
            _entity = entity;
            _context = context;
            _hub = hub;
            Attack();
        }
        
        private void Attack()
        {
            _hub.CharacterAttacker.StartAttack(_entity);
            _hub.CharacterTriggerRunner.CharacterLeavedTrigger += FinishAttack;
        }

        private void FinishAttack(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            _hub.CharacterAttacker.StopAttack();
            _context.Idle();
        }

        public void Dispose()
        {
            _hub.CharacterTriggerRunner.CharacterLeavedTrigger -= FinishAttack;
        }
    }
}