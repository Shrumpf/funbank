using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public static class atm
    {
        static double[] values = { 500, 200, 100, 50, 20, 10, 5, 2, 1, 0.5, 0.2, 0.1, 0.05, 0.02, 0.01 };
        static int[] currentMoney = new int[15];

        public static int[] GimmeDaMoneh(int amount, )
        {
            if (!checkPossible(amount))
            {
                return null;
            }

            int[] moneyOut = new int[15];
            int currentValueIndex = 0;

            while (amount > 0)
            {
                moneyOut[currentValueIndex] = Math.Max(currentMoney[currentValueIndex],
                    currentMoney[currentValueIndex] - (int)(amount / values[currentValueIndex]));

                amount -= (int)(moneyOut[currentValueIndex] * values[currentValueIndex]);
                currentMoney[currentValueIndex] -= moneyOut[currentValueIndex];
                currentValueIndex++;
            }

            return moneyOut;
        }

        public static bool checkPossible(int amount)
        {
            if (amount <= currentMoney.Sum())
                return true;
            else
                return false;
        }

        public static void printTransactions(int accountID)
        {
            //TransactionPartner HFE    URH ")Use   Date   Amount
        }
    }
}
