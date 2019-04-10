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
        public double Balance { get; set; }
        public readonly int accountId;
        public string Token { get; set; }

        public User(int id, string pass)
        {
            accountId = id;
            Password = pass;
        }
    }
}
