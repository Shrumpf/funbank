using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SqlKata;
using SqlKata.Compilers;


namespace SpasBank.Classes
{
    public class User
    {
        public readonly string Password;
        public readonly double Balance;
        public readonly int accountId;

        public User(int id, string pass)
        {
            accountId = id;
            Password = pass;
        }

        public int[] Withdraw(int amount)
        {
            int[] bills = Atm.GimmeDaMoneh(amount);
            if (bills == null)
            {
                return null;
            }
            if (amount <= Balance)
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
}
