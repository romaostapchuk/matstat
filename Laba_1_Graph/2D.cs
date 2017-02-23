using System;
using System.Collections.Generic;
using System.Linq;

namespace Laba_1_Graph
{
    class _2D
    {
        public static double    Integrate(Func<double, double> Y)
        {
            double x, a, b, h, s;
            int n;

            a = 2;
            b = 5;
            n = 500;

            h = (b - a) / n;
            s = 0; x = a + h;
            while (x < b)
            {
                s = s + 4 * Y(x);
                x = x + h;
                s = s + 2 * Y(x);
                x = x + h;
            }
            s = h / 3 * (s + Y(a) - Y(b));
            return (s);
        }

        public static double    DepDisp    (int amount, double[] Z, double averageZ)    //used in Tdep2D
        {
            double sum = 0;
            for (int i = 0; i < amount; i ++)
                sum = Math.Pow(Z[i] - averageZ, 2);

            double Disp = (double)(1.0 / (amount - 1)) * sum;
            return Disp;
        }
        public static double    TDep2D     (int amount, double[] numb1, double[] numb2) // 
        {
            double[] Z = new double[amount];

            for (int i = 0; i < amount; i++)
            {
                Z[i] = numb1[i] - numb2[i];
            }

            double sumZ = 0;
            for (int i = 0; i < amount; i++)
                sumZ += Z[i];

            double averageZ = (double)(1.0 / amount) * sumZ;

            double D = DepDisp(amount, Z, averageZ);
            if (D == 0)
                return (0);
            double T = (averageZ * Math.Sqrt(amount)) / Math.Sqrt(D);
            return T;
        }
        public static bool      Check_T_Dep(double Tdep, int amount_z)
        {
            if (Math.Abs(Tdep) <= Quantils.Student(amount_z))
                return (true);
            return (false);
        }

        public static double    IndepDisp  (int amount1, int amount2, double Disp1, double Disp2)
        {
            double DispZ = Disp1 / amount1 + Disp2 / amount2;
            return DispZ;
        }
        public static double    TIndep2D   (double average1, double average2, double DispZ)
        {
            if (DispZ == 0)
                return (0);
            double T = (average1 - average2) / DispZ;
            return T;
        }
        public static bool      Check_T_Indep(double TIndep, int amount_z)
        {
            if (Math.Abs(TIndep) <= Quantils.Student(amount_z))
                return (true);
            return (false);
        }
    }

    class Ftest
    {
        public static double F(double Disp1, double Disp2)
        {
            double f = 0;
            if (Disp1 >= Disp2)
                f = Disp1 / Disp2;
            else
                f = Disp2 / Disp1;
            return f;
        }

        public static double Flnyu(int amount1, int amount2)
        {
            double F = 0;
            double
                Nyu1 = amount1 - 1,
                Nyu2 = amount2 - 1;

            double z = Quantils.Fisher(0.95, Nyu1, Nyu2);

            F = Math.Exp(2 * z);
            return F;
        }

        public static bool CheckFtest(double Disp1, double Disp2, int amount1, int amount2, ref double val)
        {
            val = F(Disp1, Disp2);
            if (val <= Flnyu(amount1, amount2))
                return (true);
            return (false);
        }
    }

    class Correlation
    {
        public static double    Alpha = 0.95;

        public static double    Pair(double[] arr_1, double[] arr_2, double average_1, double average_2)
        {
            double ret = 0;
            int N = arr_1.Length;

            double
                sigm_1 = 0,
                sigm_2 = 0;
            sigm_1 = Functions.Sigm(N, average_1, arr_1);
            sigm_2 = Functions.Sigm(N, average_2, arr_2);

            double xy = 0;
            for (int i = 0; i < N; i++)
                xy += arr_1[i] * arr_2[i];
            xy /= N;

            ret = ((double)N / (double)(N - 1)) * ((xy - average_1 * average_2) / (sigm_1 * sigm_2));
            ret = (Math.Round(ret, 4));
            if (ret >= 1 )
                return (1);
            if (ret <= -1)
                return (-1);
            return (ret);
        }

        public static bool      Check_Correlation(double Rxy, int amount)
        {
            double T = (Rxy * Math.Sqrt(amount - 2)) / (Math.Sqrt(1 - Rxy * Rxy));

            double q = Quantils.Student(amount);
            if (Math.Abs(T) <= Quantils.Student(amount - 2))
                return false;
            else
                return true;
        }

        public static double    IntervalBot(double Rxy, double N)
        {
            double ret = 0;
            ret = Rxy + (Rxy * (1 - Rxy * Rxy)) / (2 * N);
            ret -= Quantils.Normal(1 - Alpha / 2);
            ret = Math.Round(ret, 4);
            return (ret);
        }
        public static double    IntervalTop(double Rxy, double N)
        {
            double ret = 0;
            ret = Rxy + (Rxy * (1 - Rxy * Rxy)) / (2 * N);
            ret += Quantils.Normal(1 - Alpha / 2);
            ret = Math.Round(ret, 4);
            return (ret);
        }

        public static bool      Ratio(double[] arr_1, double[] arr_2, double step, double average_2, ref double P)
        {
            double P_2 = 0;

            List<List<double>> Mi_val = new List<List<double>>();
            double prev = arr_1.Min();
            double next = prev + step;
            prev -= 1;

            while (next <= arr_1.Max())
            {
                List<double> Mi = new List<double>();
                for (int i = 0; i < arr_1.Length; i++)
                    if (arr_1[i] > prev && arr_1[i] <= next)
                        Mi.Add(arr_2[i]);

                Mi_val.Add(Mi);

                prev = next;
                next += step;
            }
            double top = 0;
            double bot = 0;
            double bot_sum;

            for (int i = 0; i < Mi_val.Count; i++)
            {
                bot_sum = 0;
                top += Mi_val[i].Count * Math.Pow(Mi_val[i].Average() - average_2, 2);
                for (int j = 0; j < Mi_val[i].Count; j++)
                    bot_sum += Math.Pow(Mi_val[i][j] - average_2, 2);
                bot += bot_sum;
            }
            P_2 = top / bot;
            P = Math.Sqrt(P_2);

            double T = (P * Math.Sqrt(arr_1.Length - 2)) / Math.Sqrt(1 - P * P);
            if (Math.Abs(T) <= Quantils.Student(arr_1.Length - 2))
                return (false);
            return (true);
        }

        public static bool      Spirmen(double[] arr_1, double[] arr_2, ref double Tc)
        {
            int N = arr_1.Length;
            double[,] arr_1B = new double[N, 2];
            double[,] arr_2B = new double[N, 2];

            double[] arr_1R = new double[N];
            double[] arr_2R = new double[N];

            for (int i = 0; i < N; i++)
            {
                arr_1B[i, 0] = arr_1[i];
                arr_1B[i, 1] = i;
            }
            for (int i = 0; i < N; i++)
            {
                arr_2B[i, 0] = arr_2[i];
                arr_2B[i, 1] = i;
            }


            double[,] buff = new double[1, 2];
            for (int i = 0; i < N; i++)
            {
                for (int j = i; j < N; j++)
                {
                    if (arr_1B[i, 0] > arr_1B[j, 0])
                    {
                        buff[0, 0] = arr_1B[i, 0];
                        buff[0, 1] = arr_1B[i, 1];

                        arr_1B[i, 0] = arr_1B[j, 0];
                        arr_1B[i, 1] = arr_1B[j, 1];

                        arr_1B[j, 0] = buff[0, 0];
                        arr_1B[j, 1] = buff[0, 1];
                    }
                    if (arr_2B[i, 0] > arr_2B[j, 0])
                    {
                        buff[0, 0] = arr_2B[i, 0];
                        buff[0, 1] = arr_2B[i, 1];

                        arr_2B[i, 0] = arr_2B[j, 0];
                        arr_2B[i, 1] = arr_2B[j, 1];

                        arr_2B[j, 0] = buff[0, 0];
                        arr_2B[j, 1] = buff[0, 1];
                    }
                }
            }

            double rank = 0;
            double val = 0;

            for (int i = 0; i < N; i++)
            {
                if (i < (N - 1) && arr_1B[i, 0] != arr_1B[i + 1, 0])
                    arr_1R[i] = i;
                else
                {
                    rank = 0;
                    for (int j = i; j < (N - 1) && arr_1B[j, 0] == arr_1B[j + 1, 0]; j++)
                        rank++;
                    val = (i + i + rank) / 2;
                    for (; rank >= 0; i++)
                    {
                        arr_1R[i] = val;
                        rank--;
                    }
                    i--;
                }
            }

            for (int i = 0; i < N; i++)
            {
                if (i < (N - 1) && arr_2B[i, 0] != arr_2B[i + 1, 0])
                    arr_2R[i] = i;
                else
                {
                    rank = 0;
                    for (int j = i; j < (N - 1) && arr_2B[j, 0] == arr_2B[j + 1, 0]; j++)
                        rank++;
                    val = (i + i + rank) / 2;
                    for (; rank >= 0; i++)
                    {
                        arr_2R[i] = val;
                        rank--;
                    }
                    i--;
                }
            }

            double bf = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (arr_1B[i, 1] < arr_1B[j, 1])
                    {
                        bf = arr_1R[i];
                        arr_1R[i] = arr_1R[j];
                        arr_1R[j] = bf;
                    }
                    if (arr_2B[i, 1] < arr_2B[j, 1])
                    {
                        bf = arr_2R[i];
                        arr_2R[i] = arr_2R[j];
                        arr_2R[j] = bf;
                    }
                }
            }

            double Ts = 0;
            for (int i = 0; i < N; i++)
                Ts += Math.Pow(arr_1R[i] - arr_2R[i], 2);
            Ts = (6 * Ts) / (N * (N * N - 1));
            Ts = 1 - Ts;
            Tc = Ts;
            double T = (Ts * Math.Sqrt(N - 2)) / Math.Sqrt(1 - Ts * Ts);
            if (Math.Abs(T) <= Quantils.Student(N - 2))
                return (false);
            return (true);
        }

        public static bool      Kendal(double[] arr_1, double[] arr_2, ref double T)
        {
            int N = arr_1.Length;
            double[,] arr_1B = new double[N, 2];
            double[,] arr_2B = new double[N, 2];

            double[] arr_1R = new double[N];
            double[] arr_2R = new double[N];

            for (int i = 0; i < N; i++)
            {
                arr_1B[i, 0] = arr_1[i];
                arr_1B[i, 1] = i;
            }
            for (int i = 0; i < N; i++)
            {
                arr_2B[i, 0] = arr_2[i];
                arr_2B[i, 1] = i;
            }


            double[,] buff = new double[1, 2];
            for (int i = 0; i < N; i++)
            {
                for (int j = i; j < N; j++)
                {
                    if (arr_1B[i, 0] > arr_1B[j, 0])
                    {
                        buff[0, 0] = arr_1B[i, 0];
                        buff[0, 1] = arr_1B[i, 1];

                        arr_1B[i, 0] = arr_1B[j, 0];
                        arr_1B[i, 1] = arr_1B[j, 1];

                        arr_1B[j, 0] = buff[0, 0];
                        arr_1B[j, 1] = buff[0, 1];
                    }
                    if (arr_2B[i, 0] > arr_2B[j, 0])
                    {
                        buff[0, 0] = arr_2B[i, 0];
                        buff[0, 1] = arr_2B[i, 1];

                        arr_2B[i, 0] = arr_2B[j, 0];
                        arr_2B[i, 1] = arr_2B[j, 1];

                        arr_2B[j, 0] = buff[0, 0];
                        arr_2B[j, 1] = buff[0, 1];
                    }
                }
            }

            double rank = 0;
            double val = 0;

            for (int i = 0; i < N; i++)
            {
                if (i < (N - 1) && arr_1B[i, 0] != arr_1B[i + 1, 0])
                    arr_1R[i] = i;
                else
                {
                    rank = 0;
                    for (int j = i; j < (N - 1) && arr_1B[j, 0] == arr_1B[j + 1, 0]; j++)
                        rank++;
                    val = (i + i + rank) / 2;
                    for (; rank >= 0; i++)
                    {
                        arr_1R[i] = val;
                        rank--;
                    }
                    i--;
                }
            }

            for (int i = 0; i < N; i++)
            {
                if (i < (N - 1) && arr_2B[i, 0] != arr_2B[i + 1, 0])
                    arr_2R[i] = i;
                else
                {
                    rank = 0;
                    for (int j = i; j < (N - 1) && arr_2B[j, 0] == arr_2B[j + 1, 0]; j++)
                        rank++;
                    val = (i + i + rank) / 2;
                    for (; rank >= 0; i++)
                    {
                        arr_2R[i] = val;
                        rank--;
                    }
                    i--;
                }
            }

            double bf = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (arr_1B[i, 1] < arr_1B[j, 1])
                    {
                        bf = arr_1R[i];
                        arr_1R[i] = arr_1R[j];
                        arr_1R[j] = bf;
                    }
                    if (arr_2B[i, 1] < arr_2B[j, 1])
                    {
                        bf = arr_2R[i];
                        arr_2R[i] = arr_2R[j];
                        arr_2R[j] = bf;
                    }
                }
            }

            double S = 0;
            for (int i = 0; i < N - 1; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    if (arr_2R[j] > arr_2R[i])
                        S += 1;
                    else
                        S -= 1;
                }
            }
            double Ts = 2 * S / (N * (N - 1));
            T = Ts;
            double U = (3 * Ts / Math.Sqrt(2 * (2 * N + 5))) * Math.Sqrt(N * (N - 1));
            if (Math.Abs(U) <= Quantils.Normal(Alpha))
                return (false);
            return (true);
        }

        public static void      Concord(double[] arr_1, double[] arr_2, ref double T)
        {
            int N = arr_1.Length;
            double[,] arr_1B = new double[N, 2];
            double[,] arr_2B = new double[N, 2];

            double[] arr_1R = new double[N];
            double[] arr_2R = new double[N];

            for (int i = 0; i < N; i++)
            {
                arr_1B[i, 0] = arr_1[i];
                arr_1B[i, 1] = i;
            }
            for (int i = 0; i < N; i++)
            {
                arr_2B[i, 0] = arr_2[i];
                arr_2B[i, 1] = i;
            }


            double[,] buff = new double[1, 2];
            for (int i = 0; i < N; i++)
            {
                for (int j = i; j < N; j++)
                {
                    if (arr_1B[i, 0] > arr_1B[j, 0])
                    {
                        buff[0, 0] = arr_1B[i, 0];
                        buff[0, 1] = arr_1B[i, 1];

                        arr_1B[i, 0] = arr_1B[j, 0];
                        arr_1B[i, 1] = arr_1B[j, 1];

                        arr_1B[j, 0] = buff[0, 0];
                        arr_1B[j, 1] = buff[0, 1];
                    }
                    if (arr_2B[i, 0] > arr_2B[j, 0])
                    {
                        buff[0, 0] = arr_2B[i, 0];
                        buff[0, 1] = arr_2B[i, 1];

                        arr_2B[i, 0] = arr_2B[j, 0];
                        arr_2B[i, 1] = arr_2B[j, 1];

                        arr_2B[j, 0] = buff[0, 0];
                        arr_2B[j, 1] = buff[0, 1];
                    }
                }
            }

            double rank = 0;
            double val = 0;

            for (int i = 0; i < N; i++)
            {
                if (i < (N - 1) && arr_1B[i, 0] != arr_1B[i + 1, 0])
                    arr_1R[i] = i;
                else
                {
                    rank = 0;
                    for (int j = i; j < (N - 1) && arr_1B[j, 0] == arr_1B[j + 1, 0]; j++)
                        rank++;
                    val = (i + i + rank) / 2;
                    for (; rank >= 0; i++)
                    {
                        arr_1R[i] = val;
                        rank--;
                    }
                    i--;
                }
            }

            for (int i = 0; i < N; i++)
            {
                if (i < (N - 1) && arr_2B[i, 0] != arr_2B[i + 1, 0])
                    arr_2R[i] = i;
                else
                {
                    rank = 0;
                    for (int j = i; j < (N - 1) && arr_2B[j, 0] == arr_2B[j + 1, 0]; j++)
                        rank++;
                    val = (i + i + rank) / 2;
                    for (; rank >= 0; i++)
                    {
                        arr_2R[i] = val;
                        rank--;
                    }
                    i--;
                }
            }

            double bf = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (arr_1B[i, 1] < arr_1B[j, 1])
                    {
                        bf = arr_1R[i];
                        arr_1R[i] = arr_1R[j];
                        arr_1R[j] = bf;
                    }
                    if (arr_2B[i, 1] < arr_2B[j, 1])
                    {
                        bf = arr_2R[i];
                        arr_2R[i] = arr_2R[j];
                        arr_2R[j] = bf;
                    }
                }
            }

            double Rk = 0;
            double sum = 0;
            for (int i = 0; i < N; i++)
            {
                Rk = arr_1R[i] + arr_2R[i];
                Rk -= (N - 1);
                sum += Rk * Rk;
            }
            sum = 12 * sum / (4 * (N * N * N - N));
            T = sum;
        }
    }

    class Tables
    {
        private static double   Alpha = 0.95;

        public static double    Fehner(double[] arr_1, double[] arr_2, double average_1, double average_2)
        {
            double N = arr_1.Length;
            double V = 0;
            double W = 0;

            for (int i = 0; i < N; i++)
            {
                if (arr_1[i] - average_1 >= 0 && arr_2[i] - average_2 >= 0)
                    V++;
                else if (arr_1[i] - average_1 < 0 && arr_2[i] - average_2 < 0)
                    V++;
                else
                    W++;
            }
            double I = (V - W) / (V + W);
            return (I);
        }

        public static bool      Fi(double[] arr_1, double[] arr_2, double average_1, double average_2, ref double Val)
        {
            double N = arr_1.Length;
            double
                N00 = 0,
                N01 = 0,
                N10 = 0,
                N11 = 0;

            double
                N0 = 0,
                N1 = 0,
                M0 = 0,
                M1 = 0,
                N_ = 0;

            for (int i = 0; i < N; i++)
            {
                if (arr_1[i] < average_1 && arr_2[i] < average_2)
                    N00++;
                else if (arr_1[i] >= average_1 && arr_2[i] < average_2)
                    N01++;
                else if (arr_1[i] < average_1 && arr_2[i] >= average_2)
                    N10++;
                else if (arr_1[i] >= average_1 && arr_2[i] >= average_2)
                    N11++;
            }
            N0 = N00 + N01;
            N1 = N10 + N11;
            M0 = N00 + N10;
            M1 = N01 + N11;
            N_ = N0 + N1;

            double F = (N00 * N11 - N01 * N10) / Math.Sqrt(N0 * N1 * M0 * M1);
            Val = F;
            double Xi = N * F * F;
            if (Xi >= Quantils.XiSquare(Alpha, 1))
                return (true);
            return (false);
        }

        public static bool      Yula(double[] arr_1, double[] arr_2, double average_1, double average_2, ref double Val_1, ref double Val_2)
        {
            double N = arr_1.Length;
            double
                N00 = 0,
                N01 = 0,
                N10 = 0,
                N11 = 0;

            double
                N0 = 0,
                N1 = 0,
                M0 = 0,
                M1 = 0,
                N_ = 0;

            for (int i = 0; i < N; i++)
            {
                if (arr_1[i] < average_1 && arr_2[i] < average_2)
                    N00++;
                else if (arr_1[i] >= average_1 && arr_2[i] < average_2)
                    N01++;
                else if (arr_1[i] < average_1 && arr_2[i] >= average_2)
                    N10++;
                else if (arr_1[i] >= average_1 && arr_2[i] >= average_2)
                    N11++;
            }
            N0 = N00 + N01;
            N1 = N10 + N11;
            M0 = N00 + N10;
            M1 = N01 + N11;
            N_ = N0 + N1;

            double Q = (N00 * N11 - N01 * N10) / (N00 * N11 + N01 * N10);
            double Y = (Math.Sqrt(N00 * N11) - Math.Sqrt(N01 * N10)) /
                (Math.Sqrt(N00 * N11) + Math.Sqrt(N01 * N10));

            Val_1 = Q;
            Val_2 = Y;

            double Sq = 0.5 * (1 - Q * Q) * Math.Sqrt(1 / N00 + 1 / N01 + 1 / N11 + 1 / N10);
            double Sy = 0.25 * (1 - Y * Y) * Math.Sqrt(1 / N00 + 1 / N01 + 1 / N11 + 1 / N10);

            double Uq = Q / Sq;
            double Uy = Y / Sy;

            if (Math.Abs(Uq) <= Quantils.Normal(Alpha) && Math.Abs(Uy) <= Quantils.Normal(Alpha))
                return (false);
            return (true);
        }

        public static bool      MxN_Pirson(double[] arr_1, double[] arr_2, double step_1, double step_2, ref double Val, ref double X)
        {
            int N = arr_1.Length;
            double start_1 = arr_1.Min();
            double start_2 = arr_2.Min();

            double next_1 = 0;
            double next_2 = 0;

            int x = 0;
            int y = 0;
            
            next_1 = start_1 + step_1;
            next_2 = start_2 + step_2;
            while (next_2 <= arr_2.Max()) // y
            {
                y++;
                next_2 += step_2;
            }
            while (next_1 <= arr_1.Max()) // x
            {
                x++;
                next_1 += step_1;
            }

            double[,] n = new double[y, x];
            start_1 = arr_1.Min();
            start_2 = arr_2.Min();
            next_1 = start_1 + step_1;
            next_2 = start_2 + step_2;
            start_2 -= 1;

            int i = 0;
            int j = 0;
            while (next_2 <= arr_2.Max())
            {
                start_1 = arr_1.Min();
                next_1 = start_1 + step_1;
                start_1 -= 1;
                j = 0;
                while (next_1 <= arr_1.Max())
                {
                    for (int p = 0; p < N; p++)
                    {
                        if (arr_1[p] > start_1 && arr_1[p] <= next_1)
                            if (arr_2[p] > start_2 && arr_2[p] <= next_2)
                                n[i, j]++;
                    }
                    start_1 = next_1;
                    next_1 += step_1;
                    j++;
                }
                start_2 = next_2;
                next_2 += step_2;
                i++;
            }

            double[] n_ = new double[i];
            double[] m_ = new double[j];

            for (int p = 0; p < i; p++)
                for (int q = 0; q < j; q++)
                    n_[p] += n[p, q];

            for (int q = 0; q < j; q++)
                for (int p = 0; p < i; p++)
                    m_[q] += n[p, q];

            double X2 = 0;
            double Nij = 0;
            for (int p = 0; p < i; p++)
            {
                for (int q = 0; q < j; q++)
                {
                    Nij = (n_[p] * m_[q]) / N;
                    if (Nij == 0)
                        Nij = 1;
                    X2 += Math.Pow(n[p, q] - Nij, 2) / Nij;
                }
            }
            X = X2;
            double C = Math.Sqrt(X2 / (N + X2));
            Val = C;
            if (Math.Abs(X2) <= Quantils.XiSquare(Alpha, (i - 1) * (j - 1)))
                return (false);
            return (true);
        }

        public static bool      Kendall(double[] arr_1, double[] arr_2, double step_1, double step_2, ref double Val)
        {
            int N = arr_1.Length;
            double start_1 = arr_1.Min();
            double start_2 = arr_2.Min();

            double next_1 = 0;
            double next_2 = 0;

            int x = 0;
            int y = 0;

            next_1 = start_1 + step_1;
            next_2 = start_2 + step_2;
            while (next_2 <= arr_2.Max()) // y
            {
                y++;
                next_2 += step_2;
            }
            while (next_1 <= arr_1.Max()) // x
            {
                x++;
                next_1 += step_1;
            }

            double[,] n = new double[y, x];
            start_1 = arr_1.Min();
            start_2 = arr_2.Min();
            next_1 = start_1 + step_1;
            next_2 = start_2 + step_2;
            start_2 -= 1;

            int i = 0;
            int j = 0;
            while (next_2 <= arr_2.Max())
            {
                start_1 = arr_1.Min();
                next_1 = start_1 + step_1;
                start_1 -= 1;
                j = 0;
                while (next_1 <= arr_1.Max())
                {
                    for (int p = 0; p < N; p++)
                    {
                        if (arr_1[p] > start_1 && arr_1[p] <= next_1)
                            if (arr_2[p] > start_2 && arr_2[p] <= next_2)
                                n[i, j]++;
                    }
                    start_1 = next_1;
                    next_1 += step_1;
                    j++;
                }
                start_2 = next_2;
                next_2 += step_2;
                i++;
            }

            double[] n_ = new double[i];
            double[] m_ = new double[j];

            for (int p = 0; p < i; p++)
                for (int q = 0; q < j; q++)
                    n_[p] += n[p, q];

            for (int q = 0; q < j; q++)
                for (int p = 0; p < i; p++)
                    m_[q] += n[p, q];

            double
                P = 0,
                Q = 0,
                T1 = 0,
                T2 = 0;

            double sum = 0;
            for (int p = 0; p < i; p++)
                for (int q = 0; q < j; q++)
                {
                    sum = 0;
                    for (int w = p; w < i; w++)
                        for (int e = q; e < j; e++)
                            sum += n[w, e];
                    P += sum * n[p, q];

                    for (int w = p; w < i; w++)
                        for (int e = 0; e <= q; e++)
                            sum += n[w, e];
                    Q += sum * n[p, q];
                }
            for (int p = 0; p < i; p++)
                T1 += n_[p] * (n_[p] - 1);
            for (int p = 0; p < j; p++)
                T2 += m_[p] * (m_[p] - 1);
            T1 /= 2;
            T2 /= 2;

            double Tb = (P - Q) / Math.Sqrt(((double)i / 2 * (i - 1) - T1) * ((double)i / 2 * (i - 1) - T2));
            Val = Tb;
            if (i == j)
            {
                //double U = (3 * Tb / Math.Sqrt(2 * (2 * N + 5))) * Math.Sqrt(N * (N - 1));
                //if (Math.Abs(U) > Quantils.Normal(Alpha))
                    return (true);
            }
            return (false);
        }

        public static bool      Styuart(double[] arr_1, double[] arr_2, double step_1, double step_2, ref double Val)
        {
            int N = arr_1.Length;
            double start_1 = arr_1.Min();
            double start_2 = arr_2.Min();

            double next_1 = 0;
            double next_2 = 0;

            int x = 0;
            int y = 0;

            next_1 = start_1 + step_1;
            next_2 = start_2 + step_2;
            while (next_2 <= arr_2.Max()) // y
            {
                y++;
                next_2 += step_2;
            }
            while (next_1 <= arr_1.Max()) // x
            {
                x++;
                next_1 += step_1;
            }

            double[,] n = new double[y, x];
            start_1 = arr_1.Min();
            start_2 = arr_2.Min();
            next_1 = start_1 + step_1;
            next_2 = start_2 + step_2;
            start_2 -= 1;

            int i = 0;
            int j = 0;
            while (next_2 <= arr_2.Max())
            {
                start_1 = arr_1.Min();
                next_1 = start_1 + step_1;
                start_1 -= 1;
                j = 0;
                while (next_1 <= arr_1.Max())
                {
                    for (int p = 0; p < N; p++)
                    {
                        if (arr_1[p] > start_1 && arr_1[p] <= next_1)
                            if (arr_2[p] > start_2 && arr_2[p] <= next_2)
                                n[i, j]++;
                    }
                    start_1 = next_1;
                    next_1 += step_1;
                    j++;
                }
                start_2 = next_2;
                next_2 += step_2;
                i++;
            }

            double[] n_ = new double[i];
            double[] m_ = new double[j];

            for (int p = 0; p < i; p++)
                for (int q = 0; q < j; q++)
                    n_[p] += n[p, q];

            for (int q = 0; q < j; q++)
                for (int p = 0; p < i; p++)
                    m_[q] += n[p, q];

            double
                P = 0,
                Q = 0;

            double sum = 0;
            for (int p = 0; p < i; p++)
                for (int q = 0; q < j; q++)
                {
                    sum = 0;
                    for (int w = p; w < i; w++)
                        for (int e = q; e < j; e++)
                            sum += n[w, e];
                    P += sum * n[p, q];

                    for (int w = p; w < i; w++)
                        for (int e = 0; e <= q; e++)
                            sum += n[w, e];
                    Q += sum * n[p, q];
                }
            double Tst = (2 * (P - Q) * Math.Min(i, j)) / (i * i * (Math.Min(i, j) - 1));
            Val = Tst;

            if (i != j)
                return (true);
            return (false);
        }
    }

    class Regression
    {
        private static double   Alpha = 0.95;


        public static class Linear
        {
            public static bool Bartlet(double[] arr_1, double[] arr_2, double step_1)
            {
                int N = arr_1.Length;

                double start_1 = arr_1.Min();
                double next_1 = start_1 + step_1;

                int k = 0;
                while (next_1 <= arr_1.Max())
                {
                    start_1 = next_1;
                    next_1 += step_1;
                    k++;
                }
                double step_2 = (arr_2.Max() - arr_2.Min()) / k;
                double start_2 = arr_2.Min();
                double next_2 = start_2 + step_2;

                start_1 = arr_1.Min();
                next_1 = start_1 + step_1;
                start_1 -= 1;
                start_2 -= 1;

                List<List<double>> Mi = new List<List<double>>();
                List<double> X = new List<double>();

                while (next_1 <= arr_1.Max())
                {
                    List<double> Mij = new List<double>();
                    for (int i = 0; i < N; i++)
                    {
                        if (arr_2[i] > start_2 && arr_2[i] <= next_2)
                            Mij.Add(arr_2[i]);
                    }
                    Mi.Add(Mij);
                    start_1 = next_1;
                    next_1 += step_1;
                    start_2 = next_2;
                    next_2 += step_2;
                    X.Add((start_1 - step_1 + next_1 - step_1) / 2);
                }

                double L = 0;
                double S2 = 0;

                for (int i = 0; i < Mi.Count; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < Mi[i].Count; j++)
                        sum += Math.Pow(Mi[i][j] - Mi[i].Average(), 2);
                    if (sum == 0)
                        sum = 1;
                    double S2xi = sum / Math.Max((Mi[i].Count - 1), 1);

                    S2 += (Mi[i].Count - 1) * S2xi;
                }
                S2 /= (N - Mi.Count);

                double C = 0;

                for (int i = 0; i < Mi.Count; i++)
                    C += 1.0 / Mi[i].Count;
                C -= 1 / N;
                C /= 3 * (Mi.Count - 1);
                C += 1;

                for (int i = 0; i < Mi.Count; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < Mi[i].Count; j++)
                        sum += Math.Pow(Mi[i][j] - Mi[i].Average(), 2);
                    if (sum == 0)
                        sum = 1;
                    double S2xi = sum / Math.Max((Mi[i].Count - 1), 1);

                    L += Mi[i].Count * Math.Log(S2xi / S2);
                }
                L /= -C;

                if (Math.Abs(L) > Quantils.XiSquare(Alpha, Mi.Count - 1))
                    return (false);
                return (true);
            }

            public static double MNK(double[] arr_1, double[] arr_2, ref double a, ref double b)
            {
                int N = arr_1.Length;

                double x2 = 0;
                double xy = 0;

                double x = arr_1.Average();
                double y = arr_2.Average();

                for (int i = 0; i < N; i++)
                {
                    x2 += arr_1[i] * arr_1[i];
                    xy += arr_1[i] * arr_2[i];
                }
                x2 /= N;
                xy /= N;

                a = (y * x2 - x * xy) / (x2 - x * x);
                b = (xy - x * y) / (x2 - x * x);


                double S2 = 0;
                for (int i = 0; i < N; i++)
                    S2 += Math.Pow(arr_2[i] - a - b * arr_1[i], 2);
                S2 /= (N - 2);

                return (S2);
            }

            public static double Determination(double[] arr_1, double[] arr_2)
            {
                int N = arr_1.Length;

                double av_1 = Functions.Average(N, arr_1);
                double av_2 = Functions.Average(N, arr_2);

                double Rxy = Laba_1_Graph.Correlation.Pair(arr_1, arr_2, av_1, av_2);

                double R_2 = Rxy * Rxy * 100;
                return R_2;
            }

            public static bool Model(double S_2, double[] arr_2)
            {
                double f = 0;
                f = S_2 / Functions.Disp(arr_2.Length, arr_2.Average(), arr_2);
                double F = Quantils.Fisher(Alpha, arr_2.Length - 1, arr_2.Length - 3);
                if (f <= F)
                    return (true);
                return (false);
            }

            public static double Interval(double[] arr_1, int index, double S_2)
            {
                double S_ = Math.Sqrt(S_2);
                int N = arr_1.Length;
                double Sa = 0;
                double Sb = 0;

                Sa = S_ * Math.Sqrt(1.0 / (double)N + Math.Pow(arr_1.Average(), 2) / (Functions.Disp(N, arr_1.Average(), arr_1) * (N - 1)));
                Sb = S_ / (Functions.Sigm(N, arr_1.Average(), arr_1) * Math.Sqrt(N - 1));

                double Syx = 0;
                Syx = Math.Sqrt(S_2 * (/*1 + */1.0 / (double)N) + Sb * Sb * Math.Pow(arr_1[index] - arr_1.Average(), 2));
                return (Syx);
            }
        }

        public static class Parabol
        {
            public static double MNK1(double[] arr_1, double[] arr_2, double rxy, ref double a, ref double b, ref double c)
            {
                int N = arr_1.Length;
                double x_4 = 0,
                        x_2 = 0,
                        x_3 = 0,
                        s_1 = 0,
                        s_2 = 0,
                        yy = 0;

                for (int i = 0; i < N; i++)
                {
                    x_2 += Math.Pow(arr_1[i], 2);
                    x_3 += Math.Pow(arr_1[i], 3);
                    x_4 += Math.Pow(arr_1[i], 4);
                }
                x_2 /= N;
                x_3 /= N;
                x_4 /= N;

                s_1 = Functions.Sigm(N, arr_1.Average(), arr_1);
                s_2 = Functions.Sigm(N, arr_2.Average(), arr_2);

                for (int i = 0; i < N; i++)
                    yy += (arr_1[i] * arr_1[i] - x_2) * (arr_2[i] - arr_2.Average());
                yy /= N;

                b = ((x_4 - Math.Pow(x_2, 2)) * rxy * s_1 * s_2 - (x_3 - x_2 * arr_1.Average()) * yy) /
                    (Math.Pow(s_1, 2) * (x_4 - Math.Pow(x_2, 2)) - Math.Pow((x_3 - x_2 * arr_1.Average()), 2));

                c = (Math.Pow(s_1, 2) * yy - (x_3 - x_2 * arr_1.Average()) * rxy * s_1 * s_2) /
                    (Math.Pow(s_1, 2) * (x_4 - Math.Pow(x_2, 2)) - Math.Pow((x_3 - x_2 * arr_1.Average()), 2));

                a = arr_2.Average() - b * arr_1.Average() - c * x_2;

                double S_2 = 0;
                for (int i = 0; i < N; i++)
                    S_2 += Math.Pow(arr_2[i] - a - b * arr_1[i] - c * arr_1[i] * arr_1[i], 2);
                S_2 /= (N - 3);
                S_2 *= Math.Min(a, Math.Min(b, c));

                return (S_2);
            }

            public static double MNK2(double[] arr_1, double[] arr_2, double rxy, ref double a, ref double b, ref double c)
            {
                int     N = arr_1.Length;
                double  x_2 = 0,
                        x_3 = 0,  
                        f1 = 0,
                        f2 = 0;

                for (int i = 0; i < N; i++)
                {
                    x_2 += Math.Pow(arr_1[i], 2);
                    x_3 += Math.Pow(arr_1[i], 3);
                }
                x_2 /= N;
                x_3 /= N;


                a = arr_2.Average();
                for (int i = 0; i < N; i++)
                    b += (arr_1[i] - arr_1.Average()) * arr_2[i];

                double bot = 0;
                for (int i = 0; i < N; i++)
                    bot += Math.Pow(arr_1[i] - arr_1.Average(), 2);
                b /= bot;

                bot = 0;
                for (int i = 0; i < N; i++)
                {
                    f1 = arr_1[i] - arr_1.Average();
                    f2 = arr_1[i] * arr_1[i] - 
                        ((x_3 - arr_1.Average() * x_2) / (x_2 - N * Math.Pow(arr_1.Average(), 2)))
                        * (arr_1[i] - arr_1.Average());

                    c += (f2 * arr_2[i]);
                    bot += f2 * f2;
                }
                c /= bot;

                double S_2 = 0;
                for (int i = 0; i < N; i++)
                {
                    f1 = arr_1[i] - arr_1.Average();
                    f2 = arr_1[i] * arr_1[i] - 
                        ((x_3 - arr_1.Average() * x_2) / (x_2 - N * Math.Pow(arr_1.Average(), 2)))
                        * (arr_1[i] - arr_1.Average());

                    S_2 += Math.Pow(arr_2[i] - a - b * f1 - c * f2, 2);
                }
                S_2 /= (N - 3);
                S_2 *= Math.Min(a, Math.Min(b, c));
                
                return (S_2);
            }

            public static bool   Check2(double[] arr_1, double[] arr_2, double S2, double[] abc)
            {
                int N = arr_1.Length;
                double x_4 = 0,
                        x_2 = 0,
                        x_3 = 0,
                        f1 = 0,
                        f2 = 0,
                        ta = 0,
                        tb = 0,
                        tc = 0;

                for (int i = 0; i < N; i++)
                {
                    x_2 += Math.Pow(arr_1[i], 2);
                    x_3 += Math.Pow(arr_1[i], 3);
                    x_4 += Math.Pow(arr_1[i], 4);
                }
                x_2 /= N;
                x_3 /= N;
                x_4 /= N;


                for (int i = 0; i < N; i++)
                {
                    f1 = arr_1[i] - arr_1.Average();
                    f2 = arr_1[i] * arr_1[i] -
                        ((x_3 - arr_1.Average() * x_2) / (x_2 - N * Math.Pow(arr_1.Average(), 2)))
                        * (arr_1[i] - arr_1.Average());

                    tb += f1 * f1;
                    tc += f2 * f2;
                }
                ta = (abc[0]) / S2 * Math.Sqrt(N);
                tb = (abc[1]) / S2 * Math.Sqrt(tb);
                tc = (abc[2]) / S2 * Math.Sqrt(tc);
                double t = Quantils.Student(N - 3);
                if (Math.Abs(ta) <= t && Math.Abs(tb) <= t && Math.Abs(tc) <= t)
                    return (true);
                return (false);
            }
        }
    }
}