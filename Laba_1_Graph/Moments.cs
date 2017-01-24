using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_1_Graph
{
    /// <summary>
    /// Class of moments
    /// Counts Starting and Central moments
    /// </summary>
    public static class Moments
    {
        /// <summary>
        /// Return double value that represents starting value with index index
        /// </summary>
        /// <param name="amount">Length of your array</param>
        /// <param name="numb">Array of your numbers</param>
        /// <param name="index">Index of your moment</param>
        /// <returns></returns>
        public static double Starting(int amount, double[] numb, int index)
        {
            double Mmnt = new double();
            for (int i = 0; i < amount; i++)
            {
                Mmnt += Math.Pow(numb[i], index);
            }
            Mmnt /= amount;
            return Mmnt;
        }
        /// <summary>
        /// Returns double value that represents central moment with index index
        /// </summary>
        /// <param name="amount">Length of your aaray</param>
        /// <param name="numb">Array of numbers</param>
        /// <param name="index">Index of your moment</param>
        /// <returns></returns>
        public static double Central(int amount, double[] numb, int index)
        {
            // Counting starting moments to use them further
            double MmntS = new double();
            double MmntC = new double();
            for (int j = 0; j < amount; j++)
            {
                MmntS += numb[j];
            }
            MmntS /= amount;
            double p = MmntS;

            // Counting central moments
            for (int i = 0; i < amount; i++)
            {
                MmntC += Math.Pow(numb[i] - MmntS, index);
            }
            MmntC /= amount;
            return MmntC;

        }
    }
}