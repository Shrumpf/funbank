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
            var content = new StringContent("{\"id\": " + accountId + ", \"password\": " + password + "}", Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = httpClient.PostAsync(url, content).GetAwaiter().GetResult();


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

        public void ExecuteTransaction(string recipientName, int recipientId, string purpose, double amount)
        {
            //http://api.funbank.vossflorian.de/v1/{accounts/id,accounts/login, accounts/token, get(id im slash oben), set}
            var url = baseUrl + "accounts/transfer";
            var content = new StringContent("{\"sender\": " + ActiveAccount.accountId + "," +
                " \"reciever\": " + recipientId + ", " +
                "\"amount\": " + amount + "}", Encoding.UTF8, "application/json");
        }

        public int[] Withdraw(string amountString)
        {
            try
            {
                var amount = int.Parse(amountString);
                var balance = GetBalance();
                GetAtmBalance();
                int[] bills = Atm.GimmeDaMoneh(amount);
                if (bills == null)
                {
                    throw new Exception();
                }
                if (amount <= balance)
                {
                    UpdateBalance(amount * -1);
                    UpdateAtmBalance();
                }
                return bills;
            }
            catch (AtmEmptyException e)
            {
                SendErrorCode(1);
                return new int[] { 0, 0, 0, 0, 0, 0, 0 };
            }
            catch (Exception e)
            {
                var code = new Random().Next(2, 999);
                SendErrorCode(code);
                return new int[] { 0, 0, 0, 0, 0, 0, 0 };
            }
        }

        public void Deposit(string[] amountsString)
        {
            try
            {
                var balance = GetBalance();
                GetAtmBalance();
                var sum = balance + Atm.Deposit(amountsString);
                UpdateBalance(sum);
                UpdateAtmBalance();
            }
            catch (Exception e)
            {
                var code = new Random().Next(2, 999);
                SendErrorCode(code);
            }
        }

        public void GetAtmBalance()
        {
            try
            {
                var url = baseUrl + "atm/" + Atm.atmId;
                var jsonString = httpClient.GetAsync(url).GetAwaiter().GetResult()
                    .Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var atmContainer = JsonConvert.DeserializeObject<AtmContainer>(jsonString);
                Atm.CurrentMoney = atmContainer.bills;
            }
            catch (Exception e)
            {
                var code = new Random().Next(2, 999);
                SendErrorCode(code);
            }
        }

        public void UpdateAtmBalance()
        {
            try
            {
                var url = baseUrl + "atm/" + Atm.atmId;
                var container = new AtmContainer();
                container.id = Atm.atmId;
                container.zip = Atm.zip;
                container.bills = Atm.CurrentMoney;
                var jsonString = JsonConvert.SerializeObject(container);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                PutIt(url, content);
            }
            catch (Exception e)
            {
                var code = new Random().Next(2, 999);
                SendErrorCode(code);
            }
        }

        private void UpdateBalance(double amount)
        {
            try
            {
                ActiveAccount.Balance = amount;
                var url = baseUrl + $"accounts/{ActiveAccount.accountId}/setBalance";
                var content = new StringContent(
                    "{\"balance\": " + ActiveAccount.Balance +"}", Encoding.UTF8, "application/json");
                var response = httpClient.PutAsync(url, content).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                var code = new Random().Next(2, 999);
                SendErrorCode(code);
            }
        }

        private void SendErrorCode(int error)
        {
            StreamWriter writer = new StreamWriter("InternalLog.txt");
            writer.Write(writer.NewLine + error);
            writer.Close();
            var url = baseUrl + $"atm/" + Atm.atmId + "/addErrorCode";
            var content = new StringContent("{error: " + error + "}");
            var response = PutIt(url, content);
        }

        public double GetBalance()
        {
            var url = baseUrl + $"accounts/{ActiveAccount.accountId}/getBalance";
            var jsonString = httpClient.GetAsync(url).GetAwaiter().GetResult()
                .Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var balance = JsonConvert.DeserializeObject<Balance>(jsonString);
            return balance.balance;
        }

        private HttpResponseMessage PutIt(string url, StringContent content)
        {
            try
            {
                var response = httpClient.PutAsync(url, content).GetAwaiter().GetResult();
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
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
