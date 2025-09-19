namespace TowerDefense.Interfaces
{
    /// <summary>
    /// An interface for any object that can be affected by tower effects,
    /// such as taking damage or being slowed.
    /// </summary>
    public interface IEffectable
    {
        void ApplySpeedModifier(float multiplier, float duration);
        void TakeDamage(float amount);
    }
}