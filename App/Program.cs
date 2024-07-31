using BankingAccountSystem.Repository;
using BankingAccountSystem.Services;

namespace BankingAccountSystem
{
    class Program
    {
        private static readonly IAccountRepository _accountRepository = new AccountRepository();
        private static readonly IAccountService _accountService = new AccountService(_accountRepository);
        private const string NO_ID_EXCEPTION = "You must supply an account ID";
        private const string NO_ACCOUNT_NAME_EXCEPTION = "You must supply an account name";
        private static readonly string NO_AMOUNT_EXCEPTION = "You must supply a balance";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nBanking System Menu:");
                Console.WriteLine("1. Add Account");
                Console.WriteLine("2. Deposit Money");
                Console.WriteLine("3. Withdraw Money");
                Console.WriteLine("4. Display Account Details");
                Console.WriteLine("5. Exit");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddAccount();
                        break;
                    case "2":
                        DepositMoney();
                        break;
                    case "3":
                        WithdrawMoney();
                        break;
                    case "4":
                        DisplayAccountDetails();
                        break;
                    case "5":
                        Console.WriteLine("Ending the application");
                        return; // This will exit the program
                    default:
                        Console.WriteLine("Invalid choice. Enter a number from 1 to 5");
                        break;
                }
            }
        }

        static void AddAccount()
        {
            Console.WriteLine("Enter Account ID:");
            var id = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine(NO_ID_EXCEPTION);
                return;
            }

            Console.WriteLine("Enter Account Holder Name:");
            var name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine(NO_ACCOUNT_NAME_EXCEPTION);
                return;
            }

            try
            {
                _accountService.AddAccount(id, name);
                Console.WriteLine("Account added successfully.");
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message.ToString());
            }            
        }

        static void DepositMoney()
        {
            Console.WriteLine("Enter Account ID:");
            var id = Console.ReadLine();

            if(string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine(NO_ID_EXCEPTION);
                return;
            }

            Console.WriteLine("Enter Amount to Deposit:");
            var depositAmount = Console.ReadLine();

            if(string.IsNullOrEmpty(depositAmount)) 
            {
                Console.WriteLine(NO_AMOUNT_EXCEPTION);
                return;
            }

            try
            {
                _accountService.DepositMoney(id, depositAmount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }


        }

        static void WithdrawMoney()
        {
            Console.WriteLine("Enter Account ID:");
            var id = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("Account id must be entered");
                return;
            }


            Console.WriteLine("Enter Amount to Withdraw:");
            var withdrawAmount = Console.ReadLine();

            if (string.IsNullOrEmpty(withdrawAmount)) 
            {
                Console.WriteLine(NO_AMOUNT_EXCEPTION);
                return;
            }

            try
            {
                _accountService.WithdrawMoney(id, withdrawAmount);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            
        }

        static void DisplayAccountDetails()
        {

            Console.WriteLine("Enter Account ID:");
            var id = Console.ReadLine();

            if (id == null) {
                Console.WriteLine(NO_ID_EXCEPTION);
                return;
            }

            try
            {
                var account = _accountService.GetAccountById(id);

                if (account == null)
                {
                    Console.WriteLine("No account with this id exists");
                    return;
                }

                Console.WriteLine(account.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}
