namespace Gameplay.Events
{
    public delegate void TakeDamageEventHandler(object sender, TakeDamageEventArgs eventArgs);

    public class TakeDamageEventArgs
    {
        public int Damage { get; }

        public TakeDamageEventArgs(int damage)
        {
            Damage = damage;
        }
    }
}