using Data.ScriptableObjects;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Business.Entities
{
    public abstract class EntityComponent: SerializedMonoBehaviour
    {
        public Entity Entity { get; }

        [OdinSerialize]
        private EntityCharacteristic Characteristic { get; set; }

        protected EntityComponent(Entity entity)
        {
            Entity = entity;
        }

        private void Start()
        {
            Characteristic?.Apply(Entity);
        }
    }
}