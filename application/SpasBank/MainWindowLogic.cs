using SpasBank.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using SpasBank.Classes.JsonClasses;

namespace SpasBank
{
    public class MainWindowLogic
    {
        private User ActiveAccount { get; set; }
        private readonly MainWindow Main;
        private HttpClient httpClient;
        private static string baseUrl;

        public MainWindowLogic(MainWindow main)
        {
            Main = main;
            baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            httpClient = new HttpClient();
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
            Main.LoginFailMessage.Visibility = System.Windows.Visibility.Hidden;
            int intId;
            int.TryParse(accountId, out intId);

            var url = baseUrl + "accounts/login";
            var content = new StringContent("{\"id\": " + accountId + " \"password\": " + password + "}");
            var response = PutIt(url, content);


            if (response.IsSuccessStatusCode)
            {
                ActiveAccount = new User(intId, password);

                var jsonObj = JsonConvert.DeserializeObject<Token>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                ActiveAccount.Token = jsonObj.token;

                httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    ActiveAccount.Token.ToString());

                SetView(ViewEnum.MainMenu, ViewEnum.Login);
            }

            else
            {
                Main.LoginFailMessage.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public void ExecuteTransaction(string recipientName, int recipientId, string purpose, int amount)
        {
            //http://api.funbank.vossflorian.de/v1/{accounts/id,accounts/login, accounts/token, get(id im slash oben), set}
            var url = baseUrl + "accounts/transfer";
            var content = new StringContent("{\"sender\": " + ActiveAccount.accountId + "," +
                " \"reciever\": " + recipientId + ", " +
                "\"amount\": " + amount + "}");
        }

        public int[] Withdraw(int amount)
        {
            var balance = GetBalance();
            int[] bills = Atm.GimmeDaMoneh(amount);
            if (bills == null)
            {
                return null;
            }
            if (amount <= balance)
            {
                UpdateBalance(amount * -1);
            }
            return bills;
        }

        public void Deposit(int[] amounts)
        {
            var sum = amounts.Sum();
            Atm.Deposit(amounts);
            UpdateBalance(sum);
        }

        private void UpdateBalance(double amount)
        {
            var url = baseUrl + $"accounts/{ActiveAccount.accountId}/setBalance";
            var content = new StringContent(
                "{\"auth:\" " + ActiveAccount.Token + ", " +
                "\"balance\": " + ActiveAccount.Balance + "}");
            var response = httpClient.PutAsync(url, content);
        }

        //private void SendErrorCode(int error)
        //{
        //    StreamWriter writer = new StreamWriter("InternalLog.txt");
        //    writer.Write(writer.NewLine+error);
        //    writer.Close();
        //    var url = baseUrl + $"atm/addErrorCode";
        //    var content = new StringContent("{error: "+error+"}");
        //    var response = PutIt(url, content);
        //}

        private double GetBalance()
        {
            double balance = 0;
            var url = baseUrl + $"accounts/{ActiveAccount.accountId}/getBalance";
            var content = new StringContent("{auth: " + ActiveAccount.Token + "}");
            httpClient.GetAsync(url).GetAwaiter().GetResult();
            return balance;
        }

        private HttpResponseMessage PutIt(string url, StringContent content)
        {
            var response = httpClient.PutAsync(url, content).GetAwaiter().GetResult();
            return response;
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
