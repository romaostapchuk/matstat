using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_1_Graph
{
    class Criteria
    {

        public static double Pirson(int amount, List<double> Xi_Value, List<double> Xi_Borders, double average, double[] numb, bool exponential, bool normal)
        {
            double X = 0;
            double x_l = 0,
                x_r = 0,
                ni = 0,
                ni0 = 0;
            if (exponential)
            {
                double lyambda = 1 / average;
                for (int i = 0; i < Xi_Value.Count - 1; i++)
                {
                    x_l = Xi_Borders[i];
                    x_r = Xi_Borders[i + 1];
                    ni = Xi_Value[i];


                    ni0 = amount * (Counts.FuncEmperic(x_r, lyambda) - Counts.FuncEmperic(x_l, lyambda)); // care

                    X += Math.Pow(ni - ni0, 2) / ni0;
                }
            }
            else if (normal)
            {
                double m = average;
                double S = 0;

                double Sum_of_X2 = 0;
                for (int i = 0; i < amount; i++)
                {
                    Sum_of_X2 += Math.Pow(numb[i], 2);
                }
                Sum_of_X2 /= amount;
                S = (amount / (amount - 1)) * Math.Sqrt(Sum_of_X2 - Math.Pow(average, 2));

                for (int i = 0; i < Xi_Value.Count - 1; i++)
                {
                    x_l = Xi_Borders[i];
                    x_r = Xi_Borders[i + 1];
                    ni = Xi_Value[i];


                    ni0 = amount * (Counts.FuncNorm(x_r, m, S) - Counts.FuncNorm(x_l, m, S)); // care ======

                    X += Math.Pow(ni - ni0, 2) / ni0;
                }
            }

            return Math.Round(X, 4);
        }

        public static double Kolmahorov(int amount, double[] numb, double average,bool normal, bool exponential, bool extreme, double A, double B)
        {
            Counts.Sort(numb);
            double D_Plus = 0;
            double D_Minus = 0;
            if(normal)
            {
                double m = average;
                double S = 0;

                double Sum_of_X2 = 0;
                for (int i = 0; i < amount; i++)
                {
                    Sum_of_X2 += Math.Pow(numb[i], 2);
                }
                Sum_of_X2 /= amount;
                S = (amount / (amount - 1)) * Math.Sqrt(Sum_of_X2 - Math.Pow(average, 2));

                for (int i = 0; i < numb.Length; i++)
                {
                    D_Plus = Math.Max(Math.Abs((double)i / numb.Length - Counts.FuncNorm(numb[i], m, S)), D_Plus);
                }

                double ty = 0;
                for (int i = 1; i < numb.Length; i++)
                {
                    ty = Counts.FuncNorm(numb[i - 1], m, S);
                    D_Minus = Math.Max(Math.Abs((double)i / numb.Length - Counts.FuncNorm(numb[i - 1], m, S)), D_Minus);
                }
            } 
            if(exponential)
            {
                double lyambda = 1 / average;
                for (int i = 1; i < numb.Length; i++)
                {
                    D_Plus = Math.Max(Math.Abs((double)i /numb.Length - Counts.FuncEmperic(numb[i], lyambda)), D_Plus);
                }
                for (int i = 1; i < numb.Length; i++)
                {
                    D_Minus = Math.Max(Math.Abs((double)i /numb.Length - Counts.FuncEmperic(numb[i - 1], lyambda)), D_Minus);
                }
            }
            else if(extreme)
            {
                for (int i = 1; i < numb.Length; i++)
                {
                    D_Plus = Math.Max(Math.Abs((double)i / numb.Length - Counts.FuncExtreme(numb[i], A, B)), D_Plus);
                }
                for (int i = 1; i < numb.Length; i++)
                {
                    D_Minus = Math.Max(Math.Abs((double)i / numb.Length - Counts.FuncExtreme(numb[i - 1], A, B)), D_Minus);
                }
            }
            double z = Math.Sqrt(amount) * Math.Max(D_Plus, D_Minus);
            double Sum = 0;
            for(int k =1; k < 100; k++)
            {
                double a = Math.Pow(-1, k) * Math.Exp(-2 * Math.Pow(k, 2) * Math.Pow(z, 2)) * 
                    (1 - ((2 * Math.Pow(k, 2) * z) / (3 * Math.Sqrt(amount))) - 1 / (18 * amount) * 
                    (((Math.Pow(k, 2) - 0.5 * (1 - Math.Pow(-1, k))) - 4 * ((Math.Pow(k, 2) - 0.5 * 
                    (1 - Math.Pow(-1, k))) + 3)) * (Math.Pow(k, 2) * Math.Pow(z, 2)) + 
                    8 * Math.Pow(k, 4) * Math.Pow(z, 4)) + ((Math.Pow(k,2) * z)/(27*Math.Sqrt(Math.Pow(amount,3))))*
                    ((Math.Pow(5*Math.Pow(k,2) + 22 - 7.5*(1 -Math.Pow(-1, k)),2))/5-
                    (4*(5 * Math.Pow(k, 2) + 22 - 7.5 * (1 - Math.Pow(-1, k)) + 45)* Math.Pow(k,2) * 
                    Math.Pow(z,2))/15 +8*Math.Pow(k,4)*Math.Pow(z,4)));
                Sum += a;
            }
            double K = 1 + 2*Sum;
            return Math.Round(1 - K , 4);
        }
    }
}
