namespace Business.Multipliers
{
    public class DefaultValueMultiplier: IValueMultiplier
    {
        private readonly float _multiplier;

        public DefaultValueMultiplier(float multiplier)
        {
            _multiplier = multiplier;
        }
    
        public float Apply(float multiplyValue)
        {
            return multiplyValue * _multiplier;
        }
    }
}