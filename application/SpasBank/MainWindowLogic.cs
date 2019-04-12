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
                    Main.LoginFailMessage.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case ViewEnum.Balance:
                    Main.BalanceGrid.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case ViewEnum.Deposit:
                    Main.DepositGrid.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case ViewEnum.MainMenu:
                    Main.MainMenuGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.MainMenuSuccessLabel.Visibility = System.Windows.Visibility.Hidden;
                    Main.MainMenuFailedLabel.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case ViewEnum.Transaction:
                    Main.TransactionGrid.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case ViewEnum.Withdraw:
                    Main.WithdrawGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.TakeMoneyLabel.Visibility = System.Windows.Visibility.Hidden;
                    Main.TakeMoneyFailed.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case ViewEnum.Waiting:
                    Main.WaitingGrid.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                default:
                    Main.LoginGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.BalanceGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.DepositGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.MainMenuGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.TransactionGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.WithdrawGrid.Visibility = System.Windows.Visibility.Collapsed;
                    Main.WaitingGrid.Visibility = System.Windows.Visibility.Collapsed;
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
                case ViewEnum.Waiting:
                    Main.WaitingGrid.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    //ToDo Logout user when this happens.
                    Main.LoginGrid.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }

        public async Task<bool> Authenticate(string accountId, string password)
        {
            Main.LoginFailMessage.Visibility = System.Windows.Visibility.Hidden;
            try
            {
                int intId;
                if (accountId == "" || password == "")
                {
                    Main.LoginFailMessage.Visibility = System.Windows.Visibility.Visible;
                    return false;
                }
                int.TryParse(accountId, out intId);

                var url = baseUrl + "accounts/login";
                var content = new StringContent("{\"id\": " + accountId +
                    ", \"password\": " + password + "}", Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    ActiveAccount = new User(intId, password);

                    var jsonObj = JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());
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
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> ExecuteTransaction(string recipientName, string recipientId, string purpose, string amount)
        {
            try
            {
                if (recipientName == "" || recipientId == "" || amount == "")
                {
                    Main.MainMenuFailedLabel.Visibility = System.Windows.Visibility.Visible;
                    SetView(ViewEnum.MainMenu, ViewEnum.Transaction);
                    return false;
                }
                var balance = await GetBalance();
                var intId = int.Parse(recipientId);
                var doubleAmount = double.Parse(amount);
                if (doubleAmount > balance)
                {
                    Main.MainMenuFailedLabel.Visibility = System.Windows.Visibility.Visible;
                    SetView(ViewEnum.MainMenu, ViewEnum.Transaction);
                    return false;
                }
                var url = baseUrl + "accounts/transfer";
                var content = new StringContent("{\"sender\": " + ActiveAccount.accountId + "," +
                    " \"reciever\": " + intId + ", " +
                    "\"amount\": " + doubleAmount + "}", Encoding.UTF8, "application/json");
                
                var res = await httpClient.PostAsync(url, content);
                if (res.IsSuccessStatusCode)
                {
                    Main.MainMenuSuccessLabel.Visibility = System.Windows.Visibility.Visible;
                    SetView(ViewEnum.MainMenu, ViewEnum.Transaction);
                }
                else
                {
                    Main.MainMenuFailedLabel.Visibility = System.Windows.Visibility.Visible;
                    SetView(ViewEnum.MainMenu, ViewEnum.Transaction);
                }
                return res.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Main.MainMenuFailedLabel.Visibility = System.Windows.Visibility.Visible;
                SetView(ViewEnum.MainMenu, ViewEnum.Transaction);
                return false;
            }
        }

        public async Task<int[]> Withdraw(string amountString)
        {
            try
            {
                Main.TakeMoneyFailed.Visibility = System.Windows.Visibility.Hidden;
                Main.TakeMoneyLabel.Visibility = System.Windows.Visibility.Hidden;
                var amount = int.Parse(amountString);
                var balance = await GetBalance();
                await GetAtmBalance();
                int[] bills = Atm.GimmeDaMoneh(amount);
                amount = Atm.GetValueOfBills(bills);
                if (bills == null)
                {
                    throw new Exception();
                }

                if (amount <= balance)
                {
                    await UpdateBalance(balance - amount);
                    await UpdateAtmBalance();
                    Main.TakeMoneyLabel.Visibility = System.Windows.Visibility.Visible;
                    Main.WithdrawAmountBox.Text = amount.ToString();
                }
                else
                {
                    Main.TakeMoneyFailed.Visibility = System.Windows.Visibility.Visible;
                }

                return bills;

            }
            catch (AtmEmptyException e)
            {
                SendErrorCode(1);
                Main.TakeMoneyLabel.Visibility = System.Windows.Visibility.Hidden;
                return new int[] { 0, 0, 0, 0, 0, 0, 0 };
            }
            catch (Exception e)
            {
                var code = new Random().Next(2, 999);
                Main.TakeMoneyLabel.Visibility = System.Windows.Visibility.Hidden;
                SendErrorCode(code);
                return new int[] { 0, 0, 0, 0, 0, 0, 0 };
            }
        }

        public async Task<bool> Deposit(string[] amountsString)
        {
            try
            {
                var balance = await GetBalance();
                await GetAtmBalance();
                var sum = balance + Atm.Deposit(amountsString);
                await UpdateBalance(sum);
                await UpdateAtmBalance();
                Main.MainMenuSuccessLabel.Visibility = System.Windows.Visibility.Visible;
                SetView(ViewEnum.MainMenu, ViewEnum.Deposit);
                return true;
            }
            catch (Exception e)
            {
                Main.MainMenuFailedLabel.Visibility = System.Windows.Visibility.Visible;
                SetView(ViewEnum.MainMenu, ViewEnum.Deposit);
                var code = new Random().Next(2, 999);
                SendErrorCode(code);
                return false;
            }
        }

        public async Task<bool> GetAtmBalance()
        {
            try
            {
                var url = baseUrl + "atm/" + Atm.atmId;
                var response = await httpClient.GetAsync(url);
                var jsonString = await response.Content.ReadAsStringAsync();

                var atmContainer = JsonConvert.DeserializeObject<AtmContainer>(jsonString);
                Atm.CurrentMoney = atmContainer.bills;
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                var code = new Random().Next(2, 999);
                SendErrorCode(code);
                return false;
            }
        }

        public async Task<bool> UpdateAtmBalance()
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
                await PutIt(url, content);
                return true;
            }
            catch (Exception e)
            {
                var code = new Random().Next(2, 999);
                SendErrorCode(code);
                return false;
            }
        }

        private async Task<bool> UpdateBalance(double amount)
        {
            try
            {
                ActiveAccount.Balance = amount;
                var url = baseUrl + $"accounts/{ActiveAccount.accountId}/setBalance";
                var content = new StringContent(
                    "{\"balance\": " + ActiveAccount.Balance + "}", Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                var code = new Random().Next(2, 999);
                SendErrorCode(code);
                return false;
            }
        }

        private async Task<bool> SendErrorCode(int error)
        {
            StreamWriter writer = new StreamWriter("InternalLog.txt");
            writer.Write(writer.NewLine + error);
            writer.Close();
            var url = baseUrl + $"atm/" + Atm.atmId + "/addErrorCode";
            var content = new StringContent("{error: " + error + "}");
            var response = await PutIt(url, content);
            return response.IsSuccessStatusCode;

        }

        public async Task<double> GetBalance()
        {
            var url = baseUrl + $"accounts/{ActiveAccount.accountId}/getBalance";
            var jsonString = (await httpClient.GetAsync(url))
                .Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var balance = JsonConvert.DeserializeObject<Balance>(jsonString);
            return balance.balance;
        }

        private async Task<HttpResponseMessage> PutIt(string url, StringContent content)
        {
            try
            {
                var response = await httpClient.PutAsync(url, content);
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void ClearDepositFields()
        {
            Main.FiveHundredBox.Text = "";
            Main.TwoHundredBox.Text = "";
            Main.OneHundredBox.Text = "";
            Main.FiftyBox.Text = "";
            Main.TwentyBox.Text = "";
            Main.TenBox.Text = "";
            Main.FiveBox.Text = "";
        }

        public void ClearTransactionFields()
        {
            Main.RecipientIdBox.Text = "";
            Main.RecipientNameBox.Text = "";
            Main.TransactionAmountBox.Text = "";
            Main.PurposeBox.Text = "";
        }
    }

    public enum ViewEnum
    {
        None,
        Back,
        Login,
        MainMenu,
        Deposit,
        Withdraw,
        Transaction,
        Balance,
        Waiting,

    };
}
