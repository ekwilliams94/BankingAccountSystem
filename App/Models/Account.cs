namespace BankingAccountSystem.Models
{
    public class Account(int id, string name, double balance)
    {
        public int Id { get; } = id;
        public string Name { get; } = name;
        public double Balance { get; private set; } = balance;

        public void WithdrawMoney(double amount)
        {
            if (amount < Balance)
            {
                Balance -= amount;
            }
        }

        public void DepositMoney(double amount)
        {
            Balance += amount;
        }

        public override string ToString()
        {
            return $"Account ID: {Id}\nAccount Holder: {Name}\nBalance: {Balance:C}";
        }
    }
}
