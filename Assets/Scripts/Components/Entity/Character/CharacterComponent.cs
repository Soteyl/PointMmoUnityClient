namespace Components.Entity.Character
{
    public class CharacterComponent : EntityComponent
    {
        private readonly Business.Entities.Character _character;

        public CharacterComponent() : base(new Business.Entities.Character())
        {
            _character = Entity as Business.Entities.Character;
        }
    }
}
