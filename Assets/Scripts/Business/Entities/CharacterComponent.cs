using Components;

namespace Business.Entities
{
    public class CharacterComponent : EntityComponent
    {
        private readonly Character _character;

        public CharacterComponent() : base(new Character())
        {
            _character = Entity as Character;
        }
    }
}
