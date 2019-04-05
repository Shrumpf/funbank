using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        }

        Regex IntEx = new Regex(@"^\d*$");

        Regex DoublEx = new Regex(@"^\d*?.\d{0,2}$");

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
            WindowLogic.SetView(ViewEnum.Deposit, ViewEnum.MainMenu);
        }

        private void WithdrawMenuButton_Click(object sender, RoutedEventArgs e)
        {
            WindowLogic.SetView(ViewEnum.Transaction, ViewEnum.MainMenu);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            WindowLogic.Authenticate(AccountNumberField.Text, PasswordField.Password);
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
            WindowLogic.SetView(ViewEnum.Login, ViewEnum.MainMenu);
        }

        private void WithdrawActionButton_Click(object sender, RoutedEventArgs e)
        {
            //ToDo
        }

        private void DepositActionButton_Click(object sender, RoutedEventArgs e)
        {
            //ToDo
        }

        private void TransactionActionButton_Click(object sender, RoutedEventArgs e)
        {
            //ToDo
        }

        private void BalanceMenuButton_Click(object sender, RoutedEventArgs e)
        {
            WindowLogic.SetView(ViewEnum.Balance, ViewEnum.MainMenu);
        }
    }
}
