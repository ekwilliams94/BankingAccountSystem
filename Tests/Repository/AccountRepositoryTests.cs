using BankingAccountSystem.Models;
using BankingAccountSystem.Repository;

namespace BankingAccountSystem.Tests.Repository
{
    [TestClass]
    public class AccountRepositoryTests(IAccountRepository sut)
    {
        private readonly IAccountRepository _sut = sut;

        [TestMethod]
        public void AddAccount_AddsAccount_WhenValidAccountProvided()
        {
            var account = new Account(123, "Test Account", 0);

            _sut.AddAccount(account);

            var result = _sut.GetAccountById(123);
            Assert.IsNotNull(result);
            Assert.AreEqual(account.Id, result.Id);
            Assert.AreEqual(account.Name, result.Name);
            Assert.AreEqual(account.Balance, result.Balance);
        }

        [TestMethod]
        public void AddAccount_ThrowsException_WhenAccountWithSameIdAlreadyExists()
        {
            var account = new Account(123, "Test Account", 0);
            _sut.AddAccount(account);

            var ex = Assert.ThrowsException<InvalidOperationException>(() => _sut.AddAccount(account));
            Assert.AreEqual("Account with this id already exists", ex.Message);
        }

        [TestMethod]
        public void GetAccountById_ReturnsAccount_WhenAccountExists()
        {
            var account = new Account(123, "Test Account", 0);
            _sut.AddAccount(account);

            var result = _sut.GetAccountById(123);
            Assert.IsNotNull(result);
            Assert.AreEqual(account.Id, result.Id);
            Assert.AreEqual(account.Name, result.Name);
            Assert.AreEqual(account.Balance, result.Balance);
        }

        [TestMethod]
        public void GetAccountById_ReturnsNull_WhenAccountDoesNotExist()
        {
            var result = _sut.GetAccountById(123);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void UpdateAccount_UpdatesAccount_WhenAccountExists()
        {
            var account = new Account(123, "Test Account", 0);
            _sut.AddAccount(account);

            var updatedAccount = new Account(123, "Updated Account", 100);
            _sut.UpdateAccount(updatedAccount);

            var result = _sut.GetAccountById(123);
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedAccount.Id, result.Id);
            Assert.AreEqual(updatedAccount.Name, result.Name);
            Assert.AreEqual(updatedAccount.Balance, result.Balance);
        }

        [TestMethod]
        public void UpdateAccount_ThrowsException_WhenAccountDoesNotExist()
        {
            var account = new Account(123, "Test Account", 0);

            var ex = Assert.ThrowsException<InvalidOperationException>(() => _sut.UpdateAccount(account));
            Assert.AreEqual("Account does not exist", ex.Message);
        }
    }
}
