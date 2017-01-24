using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Laba_1_Graph
{
    /// <summary>
    /// Class of pre-work computing
    /// </summary>
    public static class Counts
    {
        public static double FuncEmperic(double x, double Lbd)
        {
            double ret = Math.Exp(-Lbd * x);
            return 1 - ret;
        }

        public static double FuncNorm(double x, double m, double s)
        {
            double u = (x - m) / s;
            
            if (u < 0)
                return 1 - FuncNorm(Math.Abs(u), 0, 1);
            double
                p = 0.2316419,
                b1 = 0.31938153,
                b2 = -0.356563782,
                b3 = 1.781477937,
                b4 = -1.821255978,
                b5 = 1.330274429;

            double t = 1 / (1 + p * u);

            double F = 1 - (1 / (Math.Sqrt(2 * Math.PI))) * Math.Exp(-1 * (Math.Pow(u, 2) / 2)) * (b1 * t + b2 * Math.Pow(t, 2)
                + b3 * Math.Pow(t, 3) + b4 * Math.Pow(t, 4) + b5 * Math.Pow(t, 5));
            return F;
        }

        public static double FuncExtreme(double x, double B, double Bt)
        {
            double F = 1 - Math.Exp(-(Math.Exp(B * Math.Pow(x, Bt)) - 1));//**********
            //double F = Math.Exp(-Math.Exp((A - x)/B));
            return (F);
        }

        
        public static void Sort(double[] numb)
        {
            for(int i = 0; i < numb.Length; i++)
            {
                for (int j = 0; j < numb.Length; j++)
                {
                    if(numb[i] < numb[j])
                    {
                        double buf = numb[i];
                        numb[i] = numb[j];
                        numb[j] = buf;
                    }
                }
            }
        }
        
        public static void Init(TextBox TxtB1, TextBox TxtBx2,TextBox TxtBx5,TextBox TxtBx6, TextBox TxtBx7, int amount, double[] numb, bool normal, bool exponential, bool extreme,ref double Alpha, ref double Bt)
        {
            double avarage = 100;
            double averageSimple = 100;
            if (TxtBx2.TextLength > 0 )
            {
                avarage = 1 / Convert.ToDouble(TxtBx2.Text);
            }
            if (TxtBx5.TextLength > 0)
            {
                averageSimple = Convert.ToDouble(TxtBx5.Text);
            }
            double std = 100;
            if (TxtB1.TextLength > 0)
            {
                std = Convert.ToInt32(TxtB1.Text);
            }
            if(normal)
            {
                for (int i = 0; i < amount; i++)
                {
                    numb[i] = TestSimpleRNG.SimpleRNG.GetNormal(averageSimple, std);
                }
            }
            else if(exponential)
            {
                for (int i = 0; i < amount; i++)
                {
                    numb[i] = TestSimpleRNG.SimpleRNG.GetExponential(avarage);
                }
            }
            else if (extreme)
            {
                Random r = new Random();
                Bt = 1;
                Alpha = 1;

                if (TxtBx7.TextLength > 0)  //  B
                    Bt = Math.Abs(Convert.ToDouble(TxtBx7.Text));
                Alpha = 1;
                if (TxtBx6.TextLength > 0)  //  A
                    Alpha = Convert.ToDouble(TxtBx6.Text);
                
                double B = 1/Alpha;

                double X = 0;
                for(int i =0; i < amount; i++)
                {
                    X = Math.Exp((Math.Log(Math.Log(1 + Math.Log(1/(1 - r.NextDouble())))) - Math.Log(B)) /Bt); //***********
                    //X = A - B * (Math.Log(-Math.Log(r.NextDouble())));
                    numb[i] = X;
                }
            }
        }
        
        public static double Step(double TxtB, int amount, double min, double max)
        {
            int classes;
            double M = 0;
            if (TxtB > 0)
            {
                classes = Convert.ToInt32(TxtB);
                return ((max - min)/(classes-0.5));
            }
            else
            {
                try
                {
                    double buf = new double();
                    if (amount <= 100 && amount > 2)
                    {
                        if (amount % 2 == 0)
                            M = Convert.ToInt32(Math.Round(Math.Sqrt(amount)) - 1);
                        else
                            M = Convert.ToInt32(Math.Round(Math.Sqrt(amount)));
                        buf = M;
                    }
                    else if (amount > 100)
                    {
                        if (amount % 2 == 0)
                            M = Convert.ToInt32(Math.Round(Math.Pow(amount, 0.33333333)) - 1);
                        else
                            M = Convert.ToInt32(Math.Round(Math.Pow(amount, 0.33333333)));
                        buf = M;
                    }
                    else
                    {
                        return 1;
                    }
                    return ((max - min) / buf);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return 1;
                }
            }
        }

    }
}