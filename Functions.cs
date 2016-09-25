using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Laba_1_Graph
{

    /// <summary>
    /// Class of functions used to give out an assessment of point values
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Returns double value that represents Average value in array
        /// </summary>
        /// <param name="amount">Length of your array</param>
        /// <param name="numb">Name of your  array</param>
        /// <returns></returns>
        public static double Average(int amount, double[] numb)
        {
            double Sum_of_X = 0;
            for (int i = 0; i < amount; i++)
            {
                Sum_of_X += numb[i];
            }
            return Sum_of_X / amount;
        }
        /// <summary>
        /// Returns int values that represents medium value in array
        /// </summary>
        /// <param name="amount">Length of your array</param>
        /// <param name="numb">Name of your  array</param>
        /// <returns></returns>
        public static double Med(int amount, double[] numb)   // med
        {
            try
            {
                Counts.Sort(numb);
                double med = new double();
                if (amount % 2 == 0)
                    med = (numb[amount / 2] + numb[amount / 2 - 1]) / 2;
                else if (amount % 2 != 0)
                    med = numb[(amount-1) / 2 ];
                return Math.Round(med,4);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }
        }
        /// <summary>
        /// Returns int value that represents the most common values in array
        /// </summary>
        /// <param name="amount">Length of your array</param>
        /// <param name="min">Min value of array</param>
        /// <param name="max">Max value of array</param>
        /// <param name="numb">Name of your aray</param>
        /// <returns></returns>
        public static double Mod(int amount, double min, double max, double[] numb) // mod
        {
            double mod = 0;
            int pl = 0;
            int buf = 0;
            for (int i = 0; i < amount; i++)
            {
                buf = 0;
                for (int j = 0; j < amount; j++)
                {
                    if (numb[i] == numb[j])
                        buf++;
                }
                if (buf > pl)
                {
                    mod = numb[i];
                    pl = buf;
                }
            }
            return Math.Round(mod,4);
        }
        /// <summary>
        /// Returns int value that represents absolute medium value in array
        /// </summary>
        /// <param name="amount">Length of your array</param>
        /// <param name="med">Medium value in array</param>
        /// <param name="numb">Name of your aray</param>
        /// <returns></returns>
        public static double Mad(int amount, double med, double[] numb)
        {
            double[] mad = new double[amount];
            for(int i = 0; i< amount; i++)
            {
                mad[i] = Math.Abs(numb[i] - Med(amount, numb));
            }
            Counts.Sort(mad);
            double MAD = 1.483 * Med(amount, mad);
            return Math.Round(MAD,4);
        }
        /// <summary>
        /// Returns int value that represents the index of dispersion of values in array
        /// </summary>
        /// <param name="amount">Length of your array</param>
        /// <param name="averageX">Average value in array</param>
        /// <param name="numb">Name of your aray</param>
        /// <returns></returns>
        public static double Disp(int amount, double averageX, double[] numb)
        {
            double qwe = 0;
            for (int i = 0; i < amount; i++)
            {
                qwe += Math.Pow(numb[i] - averageX, 2);
            }
            return Math.Round(qwe / (amount - 1), 4);
        }
        /// <summary>
        /// Returns int value that represents the index of specified mean of values in array
        /// </summary>
        /// <param name="amount">Length of your array</param>
        /// <param name="averageX">Average value in array</param>
        /// <param name="numb">Name of your aray</param>
        /// <returns></returns>
        public static double Sigm(int amount, double averageX, double[] numb)
        {
            double qwe = 0;
            for (int i = 0; i < amount; i++)
            {
                qwe += Math.Pow(numb[i] - averageX, 2);
            }
            return Math.Round(Math.Sqrt(qwe / (amount - 1)), 4);
        }
        /// <summary>
        /// Returns double value that represents the index of asymmetry of values in array
        /// </summary>
        /// <param name="amount">Length of your array</param>
        /// <param name="averageX">Average value in array</param>
        /// <param name="numb">Name of your aray</param>
        /// <returns></returns>
        public static double Asim(int amount, double averageX, double[] numb) // ratio of asimetry
        {
            double A = (Math.Sqrt(amount * (amount - 1)) * (Moments.Central(amount, numb, 3)))
                / ((amount - 2)* (Math.Pow(Math.Sqrt(Moments.Central(amount, numb, 2)),3)));
            return Math.Round(A, 4);
        }
        /// <summary>
        /// Returns double value that represents the index of excess of values in array
        /// </summary>
        /// <param name="amount">Length of your array</param>
        /// <param name="averageX">Average value in array</param>
        /// <param name="numb">Name of your aray</param>
        /// <returns></returns>
        public static double Exces(int amount, double averageX, double[] numb)    // coef of exces
        {
            double qwe = 0;
            for (int i = 0; i < amount; i++)
            {
                qwe += Math.Pow(numb[i] - averageX, 2);
            }
            qwe /= amount;

            double qwe_4 = 0;
            for (int i = 0; i < amount; i++)
            {
                qwe_4 += Math.Pow(numb[i] - averageX, 4);
            }
            double Ē = ((Math.Pow(amount, 2) - 1) * (((qwe_4 / (amount * Math.Pow(qwe, 2))) - 3) + (6 / (amount + 1)))) / ((amount - 2) * (amount - 3));
            return Math.Round(Ē, 4);
        }
        /// <summary>
        /// Returns int values that represents medium value in array(stable to abnormal values)
        /// </summary>
        /// <param name="amount">Length of your array</param>
        /// <param name="numb">Name of your aray</param>
        /// <returns></returns>
        public static double MED_Uol(int amount, double[] numb)
        {
            List<double> xl = new List<double>();
            int p = 0;
            try
            {
                for (int j = 0; j < amount; j++)
                {
                    for (int i = 0; i <= j; i++)
                    {
                        xl.Add(((numb[i] + numb[j]) / 2));
                        p++;
                    }
                }
                double sum = 0;
                foreach (double element in xl)
                    sum += element;
                return Math.Round(sum / p , 4);
            }
            catch(Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
                return 0;
            }
        }
        /// <summary>
        /// Returns double value that represents nonparametric ratio of variety
        /// </summary>
        /// <param name="MAD">Absolute medium value in array</param>
        /// <param name="MED">Medium value in array</param>
        /// <returns></returns>
        public static double NRV(double MAD, double MED)
        {
            double NRV = MAD / MED;
            return Math.Round(NRV, 4);
        }
        /// <summary>
        /// Function returns double value of a qantil
        /// </summary>
        /// <param name="amount">Length of your array</param>
        /// <param name="index">Index of your qantil(0,05 ; 0,95)</param>
        /// <returns></returns>
        public static double Qantil(double[] numb, double index)
        {
            int Qntl = new int();
            Qntl = Convert.ToInt32(Math.Round((numb.Length -1) * index));
            return Math.Round(numb[Qntl],4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="numb"></param>
        /// <returns></returns>
        public static double CutAverage(int amount, double[] numb)
        {
            //MessageBox.Show("Введіть \"α\" для Усіченого середнього\nВ межах [0 ; 0,5]");
            //double l = Form2.pwr();
            double l =0.3;
            int k = Convert.ToInt32(amount * l);
            double sum = 0;
            for(int i = k + 1; i < amount - k; i++)
            {
                sum += numb[i];
            }
            double Value = sum /(amount - 2*k);
            return Math.Round(Value , 4);
        }
    }
}