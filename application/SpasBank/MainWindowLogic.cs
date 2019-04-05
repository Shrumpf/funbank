using SpasBank.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpasBank
{
    public class MainWindowLogic
    {
        private User ActiveAccount { get; set; }
        private readonly MainWindow Main;

        public MainWindowLogic(MainWindow main)
        {
            Main = main;
        }

        public void SetView(ViewEnum newView, ViewEnum oldView)
        {
            switch (oldView)
            {
                case ViewEnum.Login:
                    Main.LoginGrid.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case ViewEnum.Balance:
                    Main.BalanceGrid.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case ViewEnum.Deposit:
                    Main.DepositGrid.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case ViewEnum.MainMenu:
                    Main.MainMenuGrid.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case ViewEnum.Transaction:
                    Main.TransactionGrid.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case ViewEnum.Withdraw:
                    Main.WithdrawGrid.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                default:
                    Main.LoginGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.BalanceGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.DepositGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.MainMenuGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.TransactionGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.WithdrawGrid.Visibility = System.Windows.Visibility.Collapsed;
                    break;

            }
            if (newView == ViewEnum.Login || newView == ViewEnum.MainMenu)
            {
                Main.BackGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                Main.BackGrid.Visibility = System.Windows.Visibility.Visible;
            }
            switch (newView)
            {
                case ViewEnum.Login:
                    Main.LoginGrid.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ViewEnum.Balance:
                    Main.BalanceGrid.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ViewEnum.Deposit:
                    Main.DepositGrid.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ViewEnum.MainMenu:
                    Main.MainMenuGrid.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ViewEnum.Transaction:
                    Main.TransactionGrid.Visibility = System.Windows.Visibility.Visible;
                    break;
                case ViewEnum.Withdraw:
                    Main.WithdrawGrid.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    //ToDo Logout user when this happens.
                    Main.LoginGrid.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }

        public void Authenticate(string accountId, string password)
        {
            try
            {
                Main.LoginFailMessage.Visibility = System.Windows.Visibility.Hidden;
                int intId;
                int.TryParse(accountId, out intId);
                ActiveAccount = new User(intId, password);
                //user.authenticate;
                SetView(ViewEnum.MainMenu, ViewEnum.Login);
            }
            catch
            {
                //ToDo: Auth failed message
                Main.LoginFailMessage.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public void ExecuteTransaction(string recipientName, int recipientId, string purpose, int amount)
        {
            //ToDo: Talk to the flo Api
        }

        public int[] Withdraw(int amount)
        {
            var balance = GetBalance();
            //ToDo: Use FloApi to get balance
            int[] bills = Atm.GimmeDaMoneh(amount);
            if (bills == null)
            {
                return null;
            }
            if (amount <= balance)
            {
                UpdateBalance(amount * -1);
            }
            return null;
        }

        public void Deposit(int[] amounts)
        {
            var sum = amounts.Sum();
            Atm.Deposit(amounts);
            UpdateBalance(sum);
        }

        private bool UpdateBalance(double amount)
        {
            //ToDo: use FloApi to update balance
            return false;
        }

        private double GetBalance()
        {
            double balance = 0;
            //ToDo: Use FloApi to getbalance for account
            return balance;
        }
    }

    public enum ViewEnum
    {
        Back,
        Login,
        MainMenu,
        Deposit,
        Withdraw,
        Transaction,
        Balance

    };
}
