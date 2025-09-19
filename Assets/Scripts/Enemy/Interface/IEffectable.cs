namespace TowerDefense.Interfaces
{
    /// <summary>
    /// An interface for any object that can be affected by tower effects,
    /// such as taking damage or being slowed.
    /// </summary>
    public interface IEffectable
    {
        void ApplySpeedModifier(float multiplier, float duration);
        
        // We add TakeDamage here to decouple projectiles from concrete enemy classes.
        void TakeDamage(float amount);
    }
}