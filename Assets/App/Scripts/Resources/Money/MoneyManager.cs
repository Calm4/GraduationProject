namespace App.Scripts.Resources
{
    public class MoneyManager
    {
        public MoneyConfig MoneyConfig { get; private set; }

        public MoneyManager(MoneyConfig moneyConfig)
        {
            this.MoneyConfig = moneyConfig;
        }

        public void AddMoney(int amount)
        {
            MoneyConfig.currentAmount += amount;
        }

        public void ReduceMoney(int amount)
        {
            var currentMoney = MoneyConfig.currentAmount;
            if (currentMoney < amount)
            {
                MoneyConfig.currentAmount = 0;
            }
            MoneyConfig.currentAmount -= amount;
        }
    }
}