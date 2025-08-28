using System;

namespace ATMSimulator
{
    class Program
    {
        // Account details
        private static decimal balance = 1000.00m; // Starting balance
        private static string accountNumber = "1234567890";
        private static string pin = "1234"; // Default PIN
        private static string accountHolderName = "Hamza Rabie";

        // Transaction limits
        private static decimal dailyWithdrawLimit = 500.00m;
        private static decimal dailyWithdrawn = 0.00m;

        static void Main(string[] args)
        {
            Console.WriteLine("===================================");
            Console.WriteLine("    WELCOME TO SECURE ATM");
            Console.WriteLine("===================================");

            // Authenticate user
            if (AuthenticateUser())
            {
                ShowMainMenu();
            }
            else
            {
                Console.WriteLine("\nAccess Denied! Exiting...");
            }

            Console.WriteLine("\nThank you for using our ATM service!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static bool AuthenticateUser()
        {
            int attempts = 0;
            const int maxAttempts = 3;

            while (attempts < maxAttempts)
            {
                Console.Write("\nEnter your Account Number: ");
                string inputAccount = Console.ReadLine();

                Console.Write("Enter your PIN: ");
                string inputPin = ReadPassword();

                if (inputAccount == accountNumber && inputPin == pin)
                {
                    Console.WriteLine($"\nWelcome, {accountHolderName}!");
                    return true;
                }
                else
                {
                    attempts++;
                    Console.WriteLine($"\nInvalid credentials! Attempts remaining: {maxAttempts - attempts}");

                    if (attempts == maxAttempts)
                    {
                        Console.WriteLine("Maximum attempts exceeded. Account locked!");
                        return false;
                    }
                }
            }
            return false;
        }

        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        static void ShowMainMenu()
        {
            bool continueTransaction = true;

            while (continueTransaction)
            {
                Console.WriteLine("\n===================================");
                Console.WriteLine("           MAIN MENU");
                Console.WriteLine("===================================");
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("2. Withdraw Money");
                Console.WriteLine("3. Deposit Money");
                Console.WriteLine("4. Change PIN");
                Console.WriteLine("5. Mini Statement");
                Console.WriteLine("6. Exit");
                Console.WriteLine("===================================");
                Console.Write("Please select an option (1-6): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CheckBalance();
                        break;
                    case "2":
                        WithdrawMoney();
                        break;
                    case "3":
                        DepositMoney();
                        break;
                    case "4":
                        ChangePIN();
                        break;
                    case "5":
                        ShowMiniStatement();
                        break;
                    case "6":
                        continueTransaction = false;
                        break;
                    default:
                        Console.WriteLine("\nInvalid option! Please try again.");
                        break;
                }

                if (continueTransaction)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        static void CheckBalance()
        {
            Console.WriteLine("\n===================================");
            Console.WriteLine("        BALANCE INQUIRY");
            Console.WriteLine("===================================");
            Console.WriteLine($"Account Number: {accountNumber}");
            Console.WriteLine($"Account Holder: {accountHolderName}");
            Console.WriteLine($"Current Balance: ${balance:F2}");
            Console.WriteLine($"Available for Withdrawal: ${Math.Min(balance, dailyWithdrawLimit - dailyWithdrawn):F2}");
            Console.WriteLine("===================================");
        }

        static void WithdrawMoney()
        {
            Console.WriteLine("\n===================================");
            Console.WriteLine("         CASH WITHDRAWAL");
            Console.WriteLine("===================================");

            Console.Write("Enter amount to withdraw: $");

            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                if (amount <= 0)
                {
                    Console.WriteLine("Please enter a valid positive amount!");
                    return;
                }

                if (amount > balance)
                {
                    Console.WriteLine("Insufficient balance!");
                    return;
                }

                if (dailyWithdrawn + amount > dailyWithdrawLimit)
                {
                    Console.WriteLine($"Daily withdrawal limit exceeded!");
                    Console.WriteLine($"Daily limit: ${dailyWithdrawLimit:F2}");
                    Console.WriteLine($"Already withdrawn today: ${dailyWithdrawn:F2}");
                    Console.WriteLine($"Remaining limit: ${dailyWithdrawLimit - dailyWithdrawn:F2}");
                    return;
                }

                if (amount % 10 != 0)
                {
                    Console.WriteLine("Please enter an amount in multiples of $10!");
                    return;
                }

                balance -= amount;
                dailyWithdrawn += amount;

                Console.WriteLine("\n===================================");
                Console.WriteLine("      TRANSACTION SUCCESSFUL");
                Console.WriteLine("===================================");
                Console.WriteLine($"Amount Withdrawn: ${amount:F2}");
                Console.WriteLine($"Remaining Balance: ${balance:F2}");
                Console.WriteLine($"Transaction Date: {DateTime.Now}");
                Console.WriteLine("Please take your cash!");
            }
            else
            {
                Console.WriteLine("Invalid amount entered!");
            }
        }

        static void DepositMoney()
        {
            Console.WriteLine("\n===================================");
            Console.WriteLine("         CASH DEPOSIT");
            Console.WriteLine("===================================");

            Console.Write("Enter amount to deposit: $");

            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                if (amount <= 0)
                {
                    Console.WriteLine("Please enter a valid positive amount!");
                    return;
                }

                if (amount > 10000)
                {
                    Console.WriteLine("Maximum deposit limit is $10,000 per transaction!");
                    return;
                }

                balance += amount;

                Console.WriteLine("\n===================================");
                Console.WriteLine("      TRANSACTION SUCCESSFUL");
                Console.WriteLine("===================================");
                Console.WriteLine($"Amount Deposited: ${amount:F2}");
                Console.WriteLine($"New Balance: ${balance:F2}");
                Console.WriteLine($"Transaction Date: {DateTime.Now}");
            }
            else
            {
                Console.WriteLine("Invalid amount entered!");
            }
        }

        static void ChangePIN()
        {
            Console.WriteLine("\n===================================");
            Console.WriteLine("          CHANGE PIN");
            Console.WriteLine("===================================");

            Console.Write("Enter current PIN: ");
            string currentPin = ReadPassword();

            if (currentPin != pin)
            {
                Console.WriteLine("Incorrect current PIN!");
                return;
            }

            Console.Write("Enter new PIN (4 digits): ");
            string newPin = ReadPassword();

            if (newPin.Length != 4 || !IsNumeric(newPin))
            {
                Console.WriteLine("PIN must be exactly 4 digits!");
                return;
            }

            Console.Write("Confirm new PIN: ");
            string confirmPin = ReadPassword();

            if (newPin != confirmPin)
            {
                Console.WriteLine("PIN confirmation does not match!");
                return;
            }

            pin = newPin;
            Console.WriteLine("\nPIN changed successfully!");
        }

        static bool IsNumeric(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        static void ShowMiniStatement()
        {
            Console.WriteLine("\n===================================");
            Console.WriteLine("        MINI STATEMENT");
            Console.WriteLine("===================================");
            Console.WriteLine($"Account Number: {accountNumber}");
            Console.WriteLine($"Account Holder: {accountHolderName}");
            Console.WriteLine($"Statement Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Recent Transactions:");
            Console.WriteLine($"Current Balance: ${balance:F2}");
            Console.WriteLine($"Daily Withdrawal Used: ${dailyWithdrawn:F2}");
            Console.WriteLine($"Daily Withdrawal Limit: ${dailyWithdrawLimit:F2}");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Note: This is a simplified statement.");
            Console.WriteLine("For detailed transactions, visit branch.");
            Console.WriteLine("===================================");
        }
    }
}