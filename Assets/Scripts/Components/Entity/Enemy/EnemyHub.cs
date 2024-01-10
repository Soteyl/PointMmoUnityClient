using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Entity.Enemy
{
    public class EnemyHub: SerializedMonoBehaviour
    {
        [OdinSerialize]
        public EnemyComponent Enemy { get; private set; }

        [OdinSerialize]
        public Movement Movement { get; private set; }

        [OdinSerialize]
        public CharacterTriggerRunner CharacterTriggerRunner { get; private set; }

        [OdinSerialize]
        public EnemyCharacterAttacker CharacterAttacker { get; private set; }
    }
}