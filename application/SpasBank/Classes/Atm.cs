
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
        static int[] currentMoney = new int[values.Length];
        static int atmId = 1;

        public static int[] GimmeDaMoneh(int amount)
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

        public static void Deposit(int[] amount)
        {
            //ToDo Use FloApi to get current Money
            for (int i = 0; i < currentMoney.Length; i++)
            {
                currentMoney[i] += amount[i];
            }
            //ToDo use FloApi to update database
        }

        public static void PerformTransaction()
        {
            //ToDo use FloApi to perform a transaction
        }

        private static bool checkPossible(int amount)
        {
            //ToDo Use FloApi to get currentMoney
            if (amount <= currentMoney.Sum())
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
}
