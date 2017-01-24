  using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Laba_1_Graph
{
    class Criteria
    {
        public static double Alpha = 0.95;
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
     
        /*      K_D samples     */
        private static void K_Ranks(List<double[]> all_arr, ref List<List<double>> r_all)//change
        {
            List<double> values = new List<double>();
            List<double> index = new List<double>();
            List<int> x_y = new List<int>();

            for(int i = 0; i < all_arr.Count; i++)
            {
                for(int j = 0; j < all_arr[i].Length; j++)
                {
                    values.Add(all_arr[i][j]);
                    x_y.Add(i);
                }
            }
            

            for (int i = 0; i < values.Count; i++)
            {
                for (int j = i; j < values.Count; j++)
                {
                    if (values[j] < values[i])
                    {
                        double buff = values[i];
                        values[i] = values[j];
                        values[j] = buff;

                        int temp = x_y[j];
                        x_y[j] = x_y[i];
                        x_y[i] = temp;
                    }
                }
            }

            double count = 0;
            double start = 0;
            for (int i = 0; i < values.Count - 1; i++)
            {
                if (values[i] == values[i + 1])
                {
                    start = i - count;
                    count++;
                }
                else if (values[i] != values[i + 1] && count > 0)
                {
                    for (int j = 0; j <= count; j++)
                        index.Add((start + i + 2) / 2);
                    count = 0;
                }
                else
                    index.Add(i + 1);
            }
            for (double i = Math.Round(index[index.Count - 1]); i < values.Count; i++)
                index.Add(i + 1);

            for (int i = 0; i < all_arr.Count; i++)
            {
                r_all.Add(new List<double>());
                for (int j = 0; j < x_y.Count; j++)
                {
                    if (x_y[j] == i)
                        r_all[i].Add(index[j]);
                }
            }
        }

        public static bool H_criteria(List<double[]> all_arr, ref double val)
        {
            List<List<double>> r_all = new List<List<double>>();
            K_Ranks(all_arr, ref r_all);
            double[] Wi = new double[r_all.Count];
            double N = 0;

            for (int i = 0; i < r_all.Count; i++)
            {
                for (int j = 0; j < r_all[i].Count; j++)
                {
                    Wi[i] += r_all[i][j];
                }
                Wi[i] /= r_all[i].Count;
                N += r_all[i].Count;
            }

            double H = 0;
            for (int i = 0; i < r_all.Count; i++)
            {
                H += r_all[i].Count * Math.Pow(Wi[i] - ((N + 1) / 2), 2);
                //H += (Math.Pow((Wi[i] - (N + 1) / 2) / Math.Sqrt(((N + 1) * (N - r_all[i].Count)) / 12 * r_all[i].Count), 2)) * (1 - r_all[i].Count / N);
            }
            H = H * 12 / (N * (N + 1));
            val = Math.Round(H, 4);
            if (H <= Quantils.XiSquare(Alpha, r_all.Count - 1))
                return (true);
            return (false);
        } // median

        public static bool Bartlett(List<double[]> all_arrays, ref double val)
        {
            double[] av_all = new double[all_arrays.Count];
            double[] disp_all = new double[all_arrays.Count];
            double S_2;
            double B = 0;
            double C = 0;
            int k = 0;
            double sum_1 = 0;
            double sum_2 = 0;
            double X_2 = 0;

            foreach (double[] arr in all_arrays)
            {
                av_all[k] = Functions.Average(arr.Length, arr);
                disp_all[k] = Functions.Disp(arr.Length, av_all[k], arr);
                k++;
            }
            for (int i = 0; i < disp_all.Length; i++)
            {
                sum_1 += (all_arrays[i].Length - 1) * disp_all[i];
                sum_2 += all_arrays[i].Length - 1;
            }
            S_2 = sum_1 / sum_2;
            for (int i = 0; i < disp_all.Length; i++)
            {
                B += (all_arrays[i].Length - 1) * Math.Log(disp_all[i] / S_2);
                C += 1 / (all_arrays[i].Length - 1);
            }
            C = 1 + (1 / (3 * (all_arrays.Count - 1))) * (C + 1 / sum_2);
            B = -B;
            X_2 = B / C;
            val = Math.Round(X_2, 4);
            if (X_2 <= Quantils.XiSquare(Alpha, all_arrays.Count - 1))
                return (true);
            return (false);
        } // dispersion

        public static bool PreQcheck(List<double[]> all_arrays) // for Q_criteria
        {
            for (int i = 0; i < all_arrays.Count; i++)
            {
                for (int j = 0; j < all_arrays[i].Length; j++)
                {
                    if (all_arrays[i][j] != 0 || all_arrays[i][j] != 1)
                        return (false);
                }
            }
            return (true);
        }

        public static bool Q_criteria(List<double[]> all_arrays, ref double val)
        {
            double[] U = new double[all_arrays.Count];
            double[] T = new double[all_arrays[0].Length];
            for (int i = 0; i < all_arrays.Count; i++)
            {
                for (int j = 0; j < all_arrays[i].Length; j++)
                {
                    if (all_arrays[i][j] == 1)
                        U[i]++;
                }
            }
            for (int i = 0; i < all_arrays[i].Length; i++)
            {
                for (int j = 0; j < all_arrays.Count; j++)
                {
                    if (all_arrays[j][i] == 1)
                        T[i]++;
                }
            }
            double Taver = T.Sum() / all_arrays.Count;
            double sumt = 0;
            double U_2 = 0;
            for (int i = 0; i < T.Length; i++)
                sumt += T[i] - Taver;
            for (int i = 0; i < U.Length; i++)
                U_2 += Math.Pow(U[i], 2);

            double Q = (all_arrays.Count * (all_arrays.Count - 1) * sumt) / (all_arrays.Count * U.Sum() - U_2);
            val = Math.Round(Q, 4);
            if (Q <= Quantils.XiSquare(Alpha, all_arrays.Count - 1))
                return (true);
            return (false);
        } // for binary data

        public static bool AverageK(List<double[]> all_arrays, ref double val)
        {
            double Sm_2 = 1 / (all_arrays.Count);
            double sum = 0;
            double x_a = 0;
            double len = 0;

            for (int i = 0; i < all_arrays.Count; i++)
            {
                x_a += all_arrays[i].Length * Functions.Average(all_arrays[i].Length, all_arrays[i]);
                len += all_arrays[i].Length;
            }
            x_a /= len;
            for (int i = 0; i < all_arrays.Count; i++)
            {
                sum += all_arrays[i].Length * (Functions.Average(all_arrays[i].Length, all_arrays[i]) - x_a);
            }
            Sm_2 *= sum;

            double Sb_2 = 0;
            for (int i = 0; i < all_arrays.Count; i++)
            {
                Sb_2 += (all_arrays[i].Length - 1) * Functions.Disp(all_arrays[i].Length,
                    Functions.Average(all_arrays[i].Length, all_arrays[i]), all_arrays[i]);
            }
            Sb_2 /= len - all_arrays.Count;
            double F = Sm_2 / Sb_2;
            double V1 = all_arrays.Count - 1;
            double V2 = len - all_arrays.Count;
            val = Math.Round(F, 4);
            if (F <= Quantils.Fisher(Alpha, V1, V2))
                return (true);
            return (false);
        } // diff of average

        /*      2_D samples     */
        private static void Ranks(double[] arr_1, double[] arr_2, ref double[] r_1, ref double[] r_2)
        {
            List<double>    values = new List<double>();
            List<double>       index = new List<double>();
            List<int>       x_y = new List<int>();
            r_1 = new double[arr_1.Length];
            r_2 = new double[arr_2.Length];

            foreach (double vl in arr_1)
            {
                values.Add(vl);
                x_y.Add(1);
            }
            foreach (double vl in arr_2)
            {
                values.Add(vl);
                x_y.Add(2);
            }

            for(int i = 0; i < values.Count; i++)
            {
                for(int j = i; j < values.Count; j++)
                {
                    if (values[j] < values[i])
                    {
                        double buff = values[i];
                        values[i] = values[j];
                        values[j] = buff;

                        int temp = x_y[j];
                        x_y[j] = x_y[i];
                        x_y[i] = temp;
                    }
                }
            }

            double count = 0;
            double start = 0;
            for (int i = 0; i < values.Count - 1; i++)
            {
                if (values[i] == values[i + 1])
                {
                    start = i - count;
                    count++;
                }
                else if (values[i] != values[i + 1]  && count > 0)
                {
                    for(int j = 0; j <= count; j++)
                        index.Add((start + i + 2) / 2);
                    count = 0;
                }
                else
                    index.Add(i + 1);
            }
            for(double i = Math.Round(index[index.Count - 1]); i < values.Count; i++)
                index.Add(i + 1);

            int k = 0;
            int n = 0;
            for (int i = 0; i < values.Count; i++)
            {
                if (x_y[i] == 1)
                {
                    r_1[k] = index[i];
                    k++;
                }
                if (x_y[i] == 2)
                {
                    r_2[n] = index[i];
                    n++;
                }
            }
        }

        private static double InterpolateLagrangePolynomial(double x, PointF[] points, int size)
        {
            double lagrange_pol = 0;
            double basics_pol;

            for (int i = 0; i < size; i++)
            {
                basics_pol = 1;
                for (int j = 0; j < size; j++)
                {
                    if (j == i) continue;
                    basics_pol *= (x - points[j].X) / (points[i].X - points[j].X);
                }
                lagrange_pol += basics_pol * points[i].Y;
            }
            return lagrange_pol;
        }

        public static bool Smirnof_Kolmahorof(double[] arr_1b, double[] arr_2b, ref double val)
        {
            PointF[] points1 = new PointF[2 * arr_1b.Length];
            PointF[] points2 = new PointF[2 * arr_1b.Length];
            double[] arr_1 = new double[arr_1b.Length];
            double[] arr_2 = new double[arr_2b.Length];
            for (int qw = 0; qw < arr_1b.Length; qw++)
                arr_1[qw] = arr_1b[qw];
            for (int qw = 0; qw < arr_2b.Length; qw++)
                arr_2[qw] = arr_2b[qw];
            Counts.Sort(arr_1);
            Counts.Sort(arr_2);

            double
                check3 = 0,
                check4 = 0;

            int k = 0;
            double i = arr_1.Min();
            int p = 0;
            while (k < arr_1.Length)
            {
                i = arr_1[k];
                for (int j = 0; j < arr_1.Length; j++)
                {
                    if (arr_1[j] == i)
                    {
                        check3++;
                    }
                }
                k = (int)check3;
                if (check3 > check4)
                {
                    double buff = check3 / arr_1.Length;
                    points1[p].X = Convert.ToSingle(i);
                    points1[p].Y = Convert.ToSingle(buff);
                    check3 *= arr_1.Length;
                    p++;
                }
                check4 = check3;
            }

            check3 = 0;
            check4 = 0;

            k = 0;
            i = arr_2.Min();
            p = 0;
            try
            {
                while (k < arr_2.Length)
                {
                    i = arr_2[k];
                    for (int j = 0; j < arr_2.Length; j++)
                    {
                        if (arr_2[j] == i)
                        {
                            check3++;
                        }
                    }
                    k = (int)check3;
                    if (check3 > check4)
                    {
                        double buff = check3 / arr_2.Length;
                        points2[p].X = Convert.ToSingle(i);
                        points2[p].Y = Convert.ToSingle(buff);
                        p++;
                    }
                    check4 = check3;
                }
            }
            catch(Exception ex)
            {
            }

            int q = 0;
            double diff = 0;
            int w = 0;
            while (w < points1.Length && points2[q].Y < 1 && points1[w].Y < 1)
            {
                if (points2[q].X <= points1[w].X && points2[q + 1].X >= points1[w].X && q < points2.Length - 1)
                {
                    if (diff < Math.Abs( points1[q].Y - (points2[q].Y + points2[q + 1].Y) / 2))
                        diff = Math.Abs(points1[q].Y - (points2[q].Y + points2[q + 1].Y) / 2);
                    w++;
                }
                else if (points2[q].X > points1[w].X && q < points2.Length - 1)
                    w++;
                else if (points2[q].X < points1[w].X && points2[q + 1].X <= points1[w].X && q < points2.Length - 1)
                    q++;
            }
            val = 0;
            double Lz = 0;
            Lz = 1 - Math.Exp(-2 * Math.Pow(diff, 2)) * (1 - (2 * diff) / (3 * Math.Sqrt(arr_1.Length)) +
                ((2 * Math.Pow(diff, 3) / (3 * arr_1.Length)) * (1 - (2 * Math.Pow(diff, 2) / 3)))) +
                ((4 * diff) / (9 * Math.Sqrt(Math.Pow(arr_1.Length, 3)))) * (1 / 5 - (19 * Math.Pow(diff, 2)) / 15 + (2 * Math.Pow(diff, 4)) / 3);
            val = Math.Round(Lz, 4);
            if (1 - Lz > Alpha)
                return (true);
            return (false);
        }

        public static bool Vilkokson(double[] arr_1, double[] arr_2, ref double val)
        {
            double[] r_1 = new double[arr_1.Length];
            double[] r_2 = new double[arr_2.Length];
            double W = 0;

            Ranks(arr_1, arr_2, ref r_1, ref r_2);

            foreach(int rank in r_1)
            {
                W += rank;
            }
            double E = arr_1.Length * (arr_1.Length + arr_2.Length + 1) / 2;
            double D = arr_1.Length * arr_2.Length * (arr_1.Length + arr_2.Length + 1) / 12;

            double w = (W - E) / Math.Sqrt(D);
            val = Math.Round(Math.Abs(w), 4);
            if (Math.Abs(w) <= Quantils.Normal(Alpha))
                return (true);
            return (false);
        } // simetry for math expectation

        public static bool U_criteria(double[] arr_1, double[] arr_2, ref double val)
        {
            double[] r_1 = new double[arr_1.Length];
            double[] r_2 = new double[arr_2.Length];
            double W = 0;
            double U = 0;

            Ranks(arr_1, arr_2, ref r_1, ref r_2);

            foreach (int rank in r_1)
            {
                W += rank;
            }

            U = arr_1.Length * arr_2.Length + (arr_1.Length * (arr_1.Length - 1)) / 2 - W;
            double E = arr_1.Length * arr_2.Length / 2;
            double D = (arr_1.Length * arr_2.Length * ((arr_1.Length + arr_1.Length) + 1)) / 12;
            U = (U - E) / Math.Sqrt(D);
            val = Math.Round(Math.Abs(U), 4);
            if (Math.Abs(U) <= Quantils.Normal(Alpha))
                return (true);
            return (false);
        }

        public static bool Diff_Average(double[] arr_1, double[] arr_2, ref double val)
        {
            double[] r_1 = new double[arr_1.Length];
            double[] r_2 = new double[arr_2.Length];
            double V = 0;
            double av_1 = 0;
            double av_2 = 0;

            Ranks(arr_1, arr_2, ref r_1, ref r_2);

            foreach (int rank in r_1)
                av_1 += rank;
            av_1 /= r_1.Length;

            foreach (int rank in r_2)
                av_2 += rank;
            av_2 /= r_2.Length;
            double N = arr_1.Length + arr_2.Length;
            V = (double)(av_1 - av_2) / (N * Math.Sqrt((N + 1) / (12 * r_1.Length * r_2.Length)));
            val = Math.Round(Math.Abs(V), 4);
            if (Math.Abs(V) <= Quantils.Normal(Alpha))
                return (true);
            return (false);
        } 

        public static bool Sign(double[] arr_1, double[] arr_2, ref double move, ref double val)
        {
            // x y
            List<int> U = new List<int>();
            double[] nb = new double[arr_1.Length];
            for (int i = 0; i < arr_1.Length; i++)
            {
                nb[i] = arr_2[i] - arr_1[i];
                if (nb[i] > 0)
                    U.Add(1);
                else if (nb[i] < 0)
                    U.Add(0);
            }
            int S = U.Sum();
            double S_ = (S - 0.5 - arr_1.Length / 2) / Math.Sqrt(arr_1.Length / 4);
            val = Math.Round(S_, 4);
            if (S_ < Quantils.Normal(1 - Alpha / 2))
                return (true);
            else
            {
                double Q = Functions.Med(nb.Length, nb);
                move = Q;
            }
            return (false);
        }
    }
}