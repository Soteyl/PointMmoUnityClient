using Data.ScriptableObjects;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Entity
{
    public abstract class EntityComponent: SerializedMonoBehaviour
    {
        public Business.Entities.Entity Entity { get; }

        [OdinSerialize]
        private EntityCharacteristic Characteristic { get; set; }

        protected EntityComponent(Business.Entities.Entity entity)
        {
            Entity = entity;
        }

        protected virtual void Start()
        {
            Characteristic?.Apply(Entity);
        }
    }
}