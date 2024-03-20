using System;
using Components.Entity.Behaviour;
using Components.Entity.Character;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Entity.Enemy
{
    public class EnemyBehaviour: SerializedMonoBehaviour
    {
        [OdinSerialize]
        public IEntityBehaviourState State { get; private set; }
        
        [OdinSerialize]
        public EnemyHub EnemyHub { get; set; }

        private void Awake()
        {
            State = new EnemyIdleState(this, EnemyHub);
        }

        public void Attack(CharacterComponent entity)
        {
            State?.Dispose();
            State = new EntityAttackState(entity, this, EnemyHub);
        }

        public void Idle()
        {
            State?.Dispose();
            State = new EnemyIdleState(this, EnemyHub);
        }

        private void OnDestroy()
        {
            State?.Dispose();
        }
    }
}