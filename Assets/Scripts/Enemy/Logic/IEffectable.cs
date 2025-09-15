namespace TowerDefense.Interfaces
{
    /// <summary>
    /// An interface for any object that can have temporary status effects applied to it, like a slow.
    /// </summary>
    public interface IEffectable
    {
        void ApplySpeedModifier(float multiplier, float duration);
    }
}
