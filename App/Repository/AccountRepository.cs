using BankingAccountSystem.Models;

namespace BankingAccountSystem.Repository
{
    public interface IAccountRepository
    {
        void AddAccount(Account account);
        Account? GetAccountById(int id);
        void UpdateAccount(Account account);
    }
    public class AccountRepository: IAccountRepository
    {
        private static readonly Dictionary<int, Account> _accounts = [];

        public void AddAccount(Account account)
        {
            if (_accounts.ContainsKey(account.Id)) {
                throw new InvalidOperationException("Account with this id already exists");
            }

            _accounts.Add(account.Id, account);
        }

        public Account? GetAccountById(int id)
        {
            _accounts.TryGetValue(id, out var account);
            return account;
        }

        public void UpdateAccount(Account account)
        {
            if (!_accounts.ContainsKey(account.Id))
            {
                throw new InvalidOperationException("Account does not exist");
                
            }
            _accounts[account.Id] = account;
        }
    }
}
