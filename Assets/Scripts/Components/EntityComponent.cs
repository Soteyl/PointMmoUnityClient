using Business.Entities;
using Sirenix.OdinInspector;

namespace Components
{
    public abstract class EntityComponent: SerializedMonoBehaviour
    {
        public Entity Entity { get; }

        protected EntityComponent(Entity entity)
        {
            Entity = entity;
        }
    }
}