
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpasBank.Classes
{
    public static class Atm
    {
        static double[] values = { 500, 200, 100, 50, 20, 10, 5 /*, 2, 1, 0.5, 0.2, 0.1, 0.05, 0.02, 0.01 */};
        public static int[] CurrentMoney = new int[values.Length];
        public static readonly int atmId = 1;
        public static string zip { get; set; }

        public static int[] GimmeDaMoneh(int amount)
        {
            //if (!checkPossible(amount))
            //{
            //    //ToDo: WriteErrorLog
            //    throw new AtmEmptyException();
            //}

            int[] moneyOut = new int[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                if(amount >= values[i]&& CurrentMoney[i]!=0)
                {
                    //Höchstens so viel wie drin ist 
                    moneyOut[i] = (Math.Min(CurrentMoney[i],
                        (int)(amount / values[i])));

                    amount -= (int)(moneyOut[i] * values[i]);
                    
                }
                CurrentMoney[i] -= moneyOut[i];
            }

            return moneyOut;
        }

        public static int GetValueOfBills(int[] bills)
        {
            int sum = 0;
            for (int i = 0; i < bills.Length; i++)
            {
                sum += bills[i] * (int)values[i];
            }
            return sum;
        }

        public static int Deposit(string[] amountsString)
        {
            int sum = 0;
            var amounts = new int[amountsString.Length];
            for (int i = 0; i < amountsString.Length; i++)
            {
                int amount;
                if (int.TryParse(amountsString[i], out amount))
                {
                    amounts[i] = amount;
                }
                else
                {
                    amounts[i] = 0;
                }
                sum += amounts[i] * (int)values[i];
                CurrentMoney[i] += amounts[i];
            }
            return sum;
        }

        private static bool checkPossible(int amount)
        {

            if (amount <= GetValueOfBills(CurrentMoney))
                return true;
            else
                return false;
        }

        private static void sendErrorCode()
        {
            var error = new Random().Next(0, 9999);
            //ToDo Use flo Api to send error
        }
    }
    public class AtmEmptyException : Exception
    {

    }
}
