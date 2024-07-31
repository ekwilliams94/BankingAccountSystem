using AutoFixture;
using BankingAccountSystem.Models;
using BankingAccountSystem.Repository;
using BankingAccountSystem.Services;
using Moq;

namespace BankingAccountSystem.Tests.Services
{
    [TestClass]
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly IAccountService _sut;
        private readonly IFixture _fixture = new Fixture();

        public AccountServiceTests() {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _sut = new AccountService(_accountRepositoryMock.Object);
        }

        [TestMethod]
        public void AddAccount_AddsAccount_WhenValidInputsProvided()
        {
            var id = "123";
            var accountName = _fixture.Create<string>();

            var expectedAccount = new Account(123, accountName, 0);

            _sut.AddAccount(id, accountName);

            _accountRepositoryMock.Verify(a => a.AddAccount(It.Is<Account>(a =>
            a.Id == expectedAccount.Id &&
            a.Name == expectedAccount.Name &&
            a.Balance == expectedAccount.Balance)), Times.Once);
        }

        [TestMethod]
        [DataRow("abc")]
        [DataRow("-1")]
        [DataRow("-10000")]
        [DataRow(null)]
        public void AddAccount_ThrowsException_WhenInvalidIdProvided(string id)
        {
            var accountName = _fixture.Create<string>();

            var ex = Assert.ThrowsException<ArgumentException>(() => _sut.AddAccount(id, accountName));
            Assert.AreEqual("Account Id needs to be a positive number", ex.Message);
        }

        [TestMethod]
        public void GetAccountById_RetrievesAccount_WhenValidInputsProvided()
        {
            var id = "123";
            var accountName = _fixture.Create<string>();
            var accountBalance = _fixture.Create<int>();

            var expectedAccount = new Account(123, accountName, accountBalance);

            _accountRepositoryMock.Setup(a => a.GetAccountById(It.IsAny<int>())).Returns(expectedAccount);

            var result = _sut.GetAccountById(id);
            Assert.AreEqual(expectedAccount, result);
        }

        [TestMethod]
        [DataRow("abc")]
        [DataRow("-1")]
        [DataRow("-10000")]
        [DataRow(null)]
        public void GetAccountById_ThrowsException_WhenInvalidIdProvided(string id)
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => _sut.GetAccountById(id));
            Assert.AreEqual("Account Id needs to be a positive number", ex.Message);
        }

        [TestMethod]
        public void GetAccountById_ReturnsNull_WhenNoAccountFound()
        {
            var id = "123";

            _accountRepositoryMock.Setup(a => a.GetAccountById(It.IsAny<int>())).Returns<Account>(null);

            var result = _sut.GetAccountById(id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DepositMoney_DepositsMoney_WhenValidInputsProvided()
        {
            var id = "123";
            var amount = "100";
            var accountName = _fixture.Create<string>();
            var initialBalance = 200;
            var expectedBalance = initialBalance + int.Parse(amount);

            var account = new Account(123, accountName, initialBalance);

            _accountRepositoryMock.Setup(a => a.GetAccountById(It.IsAny<int>())).Returns(account);

            _sut.DepositMoney(id, amount);

            _accountRepositoryMock.Verify(a => a.UpdateAccount(It.Is<Account>(a =>
            a.Id == account.Id &&
            a.Name == account.Name &&
            a.Balance == expectedBalance)), Times.Once);
        }

        [TestMethod]
        [DataRow("abc")]
        [DataRow("-1")]
        [DataRow("-10000")]
        [DataRow(null)]
        public void DepositMoney_ThrowsException_WhenInvalidIdProvided(string id)
        {
            var amount = "100";

            var ex = Assert.ThrowsException<ArgumentException>(() => _sut.DepositMoney(id, amount));
            Assert.AreEqual("Account Id needs to be a positive number", ex.Message);
        }

        [TestMethod]
        [DataRow("abc")]
        [DataRow("-1")]
        [DataRow("-10000")]
        [DataRow(null)]
        public void DepositMoney_ThrowsException_WhenInvalidAmountProvided(string amount)
        {
            var id = "123";

            var ex = Assert.ThrowsException<ArgumentException>(() => _sut.DepositMoney(id, amount));
            Assert.AreEqual("Withdraw Amount needs to be a positive number", ex.Message);
        }

        [TestMethod]
        public void WithdrawMoney_WithdrawsMoney_WhenValidInputsProvided()
        {
            var id = "123";
            var amount = "100";
            var accountName = _fixture.Create<string>();
            var initialBalance = 200;
            var expectedBalance = initialBalance - int.Parse(amount);

            var account = new Account(123, accountName, initialBalance);

            _accountRepositoryMock.Setup(a => a.GetAccountById(It.IsAny<int>())).Returns(account);

            _sut.WithdrawMoney(id, amount);

            _accountRepositoryMock.Verify(a => a.UpdateAccount(It.Is<Account>(a =>
            a.Id == account.Id &&
            a.Name == account.Name &&
            a.Balance == expectedBalance)), Times.Once);
        }

        [TestMethod]
        [DataRow("abc")]
        [DataRow("-1")]
        [DataRow("-10000")]
        [DataRow(null)]
        public void WithdrawMoney_ThrowsException_WhenInvalidIdProvided(string id)
        {
            var amount = "100";

            var ex = Assert.ThrowsException<ArgumentException>(() => _sut.WithdrawMoney(id, amount));
            Assert.AreEqual("Account Id needs to be a positive number", ex.Message);
        }

        [TestMethod]
        [DataRow("abc")]
        [DataRow("-1")]
        [DataRow("-10000")]
        [DataRow(null)]
        public void WithdrawMoney_ThrowsException_WhenInvalidAmountProvided(string amount)
        {
            var id = "123";

            var ex = Assert.ThrowsException<ArgumentException>(() => _sut.WithdrawMoney(id, amount));
            Assert.AreEqual("Withdraw Amount needs to be a positive number", ex.Message);
        }

        [TestMethod]
        public void WithdrawMoney_ThrowsException_WhenInsufficientBalance()
        {
            var id = "123";
            var amount = "100";
            var accountName = _fixture.Create<string>();
            var initialBalance = 50;

            var account = new Account(123, accountName, initialBalance);

            _accountRepositoryMock.Setup(a => a.GetAccountById(It.IsAny<int>())).Returns(account);

            var ex = Assert.ThrowsException<Exception>(() => _sut.WithdrawMoney(id, amount));
            Assert.AreEqual("Insufficient balance to withdraw this amount", ex.Message);
        }

        [TestMethod]
        public void WithdrawMoney_ThrowsException_WhenAccountNotFound()
        {
            var id = "123";
            var amount = "100";

            _accountRepositoryMock.Setup(a => a.GetAccountById(It.IsAny<int>())).Returns<Account>(null);

            var ex = Assert.ThrowsException<Exception>(() => _sut.WithdrawMoney(id, amount));
            Assert.AreEqual("No account with this id", ex.Message);
        }

        [TestMethod]
        public void DepositMoney_ThrowsException_WhenAccountNotFound()
        {
            var id = "123";
            var amount = "100";

            _accountRepositoryMock.Setup(a => a.GetAccountById(It.IsAny<int>())).Returns<Account>(null);

            var ex = Assert.ThrowsException<Exception>(() => _sut.DepositMoney(id, amount));
            Assert.AreEqual("No account with this id", ex.Message);
        }
    }
}
