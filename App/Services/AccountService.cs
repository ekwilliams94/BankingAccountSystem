using BankingAccountSystem.Models;
using BankingAccountSystem.Repository;

namespace BankingAccountSystem.Services
{
    public interface IAccountService
    {
        public Account? GetAccountById(string id);
        public void AddAccount (string id, string name);
        public void DepositMoney (string id, string amount );
        public void WithdrawMoney (string id, string ammount);

    }

    public class AccountService(IAccountRepository repository) : IAccountService
    {
        private readonly IAccountRepository _accountRepository = repository;

        public void AddAccount(string id, string name)
        {
            var accountId = ConvertStringToPositiveNumber(id, "Account Id");

            var account = new Account(accountId, name, 0);
            _accountRepository.AddAccount(account);
        }

        public Account? GetAccountById(string id)
        {
            var accountId = ConvertStringToPositiveNumber(id, "Account Id");

            return _accountRepository.GetAccountById(accountId);
        }

        public void WithdrawMoney(string id, string amount)
        {
            var accountId = ConvertStringToPositiveNumber(id, "Account Id");

            var withdrawAmount = ConvertStringToPositiveNumber(amount, "Withdraw Amount");

            var account = _accountRepository.GetAccountById(accountId) ?? throw new Exception("No account with this id");

            if (account.Balance < withdrawAmount) {
                throw new Exception("Insufficient balance to withdraw this amount");
            }

            account.WithdrawMoney(withdrawAmount);
            _accountRepository.UpdateAccount(account);
        }

        public void DepositMoney(string id, string amount)
        {
            var accountId = ConvertStringToPositiveNumber(id, "Account Id");

            var depositAmount = ConvertStringToPositiveNumber(amount, "Withdraw Amount");

            var account = _accountRepository.GetAccountById(accountId) ?? throw new Exception("No account with this id");

            account.DepositMoney(depositAmount);
            _accountRepository.UpdateAccount(account);
        }

        private static int ConvertStringToPositiveNumber(string property, string propertyName)
        {
            if(!int.TryParse(property, out var positiveNumber) || positiveNumber < 0)
            {
                throw new ArgumentException($"{propertyName} needs to be a positive number");
            }

            return positiveNumber;
        }
    }
}
