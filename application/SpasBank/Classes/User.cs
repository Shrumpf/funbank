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
        public readonly string Id;
        public readonly string Password;
        public readonly double Balance;

        public User(string id, string pass)
        {
            Id = id;
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

        private bool UpdateBalance(double amount)
        {
            return false;
        }

        private double GetBalance()
        {
            var sql = new MySqlCompiler().Compile(new SqlKata.Query("account as acc")
                .Select("balance")
                .Where("humanId", this.Id)
                .Join("user", "human.id", "acc.humanid"));

            double balance;
            using (var con = new SqlConnection())
            {
                balance = con(sql.Sql);
            }
            return balance;
        }
    }
}
