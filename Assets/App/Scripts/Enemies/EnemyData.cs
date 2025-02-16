namespace App.Scripts.Enemies
{
    public class EnemyData
    {
        public int CurrentHealth;
        public int CurrentSpeed;
        public int CurrentDamage;

        public void InitializeEnemyData(EnemyConfig enemyConfig)
        {
            CurrentHealth =  enemyConfig.Health;
            CurrentSpeed = enemyConfig.Speed;
            CurrentDamage = enemyConfig.Damage;
        }
    }
}