namespace Components.Entity.Character
{
    public class CharacterComponent : EntityComponent
    {
        public Business.Entities.Character Character => Entity as Business.Entities.Character;

        public CharacterComponent() : base(new Business.Entities.Character())
        {
        }
    }
}
