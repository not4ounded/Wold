namespace Game.Interfaces
{
    /// <summary>
    /// Интерфейс получения урона у живых сущностей
    /// </summary>
    public interface IDamageable
    {
        bool IsAlive { get; }
        void TakeDamage(float amount);
    }
}
