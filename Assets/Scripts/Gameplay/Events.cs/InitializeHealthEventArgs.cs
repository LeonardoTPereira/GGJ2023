namespace Gameplay.Events
{
    public delegate void InitializeHealthEventHandler(object sender, InitializeHealthEventArgs eventArgs);
    public class InitializeHealthEventArgs
    {
        public int MaxHealth { get; }

        public InitializeHealthEventArgs(int maxHealth)
        {
            MaxHealth = maxHealth;
        }
    }
}