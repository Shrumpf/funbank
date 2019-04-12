using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpasBank
{
    public partial class MainWindow : Window
    {
        public MainWindowLogic WindowLogic;
        public MainWindow()
        {
            WindowLogic = new MainWindowLogic(this);

            InitializeComponent();
            WindowLogic.SetView(ViewEnum.Login, ViewEnum.None);
        }

        Regex IntEx = new Regex(@"^\d*$");

        Regex DoublEx = new Regex(@"^\d*,?\d{0,2}$");

        private void IntegerBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Accepts only positive integers
            string text = (sender as TextBox).Text + e.Text;
            e.Handled = !IntEx.IsMatch(text);
        }

        private void DoubleBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Accepts only positive doubles
            string text = (sender as TextBox).Text + e.Text;
            e.Handled = !DoublEx.IsMatch(text);
        }


        private void DepositMenuButton_Click(object sender, RoutedEventArgs e)
        {
            WindowLogic.ClearDepositFields();
            WindowLogic.SetView(ViewEnum.Deposit, ViewEnum.MainMenu);
        }

        private void WithdrawMenuButton_Click(object sender, RoutedEventArgs e)
        {
            WindowLogic.SetView(ViewEnum.Withdraw, ViewEnum.MainMenu);
        }

        private  async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;
            await WindowLogic.Authenticate(AccountNumberField.Text, PasswordField.Password);
            AccountNumberField.Text = "";
            PasswordField.Password = "";
            LoginButton.IsEnabled = true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

            WindowLogic.SetView(ViewEnum.MainMenu, ViewEnum.Back);
        }

        private void TransactionMenuButton_Click(object sender, RoutedEventArgs e)
        {
            WindowLogic.SetView(ViewEnum.Transaction, ViewEnum.MainMenu);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            WindowLogic.ClearTransactionFields();
            WindowLogic.SetView(ViewEnum.Login, ViewEnum.MainMenu);
        }

        private async void WithdrawActionButton_Click(object sender, RoutedEventArgs e)
        {
            WithdrawActionButton.IsEnabled = false;
            await WindowLogic.Withdraw(WithdrawAmountBox.Text);
            WithdrawActionButton.IsEnabled = true;
        }

        private async void DepositActionButton_Click(object sender, RoutedEventArgs e)
        {
           DepositActionButton.IsEnabled = false;
           await WindowLogic.Deposit(new string[] {
                FiveHundredBox.Text,
                TwoHundredBox.Text,
                OneHundredBox.Text,
                FiftyBox.Text,
                TwentyBox.Text,
                TenBox.Text,
                FiveBox.Text});
            WindowLogic.ClearDepositFields();
            DepositActionButton.IsEnabled = true;
        }

        private async void TransactionActionButton_Click(object sender, RoutedEventArgs e)
        {
            TransactionActionButton.IsEnabled = false;
            await WindowLogic.ExecuteTransaction(
                RecipientNameBox.Text,
                RecipientIdBox.Text,
                PurposeBox.Text,
                TransactionAmountBox.Text);
            TransactionActionButton.IsEnabled = true;
        }

        private void ClearTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            WindowLogic.ClearTransactionFields();
        }

        private async void BalanceMenuButton_Click(object sender, RoutedEventArgs e)
        {
            BalanceBox.Text = (await WindowLogic.GetBalance()).ToString();
            WindowLogic.SetView(ViewEnum.Balance, ViewEnum.MainMenu);   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
