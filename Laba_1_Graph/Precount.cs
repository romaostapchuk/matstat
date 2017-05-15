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
            for (int i = 0; i < numb.Length; i++)
            {
                for (int j = 0; j < numb.Length; j++)
                {
                    if (numb[i] < numb[j])
                    {
                        double buf = numb[i];
                        numb[i] = numb[j];
                        numb[j] = buf;
                    }
                }
            }
        }

        public static void Init(TextBox TxtB1, TextBox TxtBx2, TextBox TxtBx5, TextBox TxtBx6, TextBox TxtBx7, int amount, double[] numb, bool normal, bool exponential, bool extreme, ref double Alpha, ref double Bt)
        {
            double avarage = 100;
            double averageSimple = 100;
            if (TxtBx2.TextLength > 0)
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
            if (normal)
            {
                for (int i = 0; i < amount; i++)
                {
                    numb[i] = TestSimpleRNG.SimpleRNG.GetNormal(averageSimple, std);
                }
            }
            else if (exponential)
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

                double B = 1 / Alpha;

                double X = 0;
                for (int i = 0; i < amount; i++)
                {
                    X = Math.Exp((Math.Log(Math.Log(1 + Math.Log(1 / (1 - r.NextDouble())))) - Math.Log(B)) / Bt); //***********
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
                return ((max - min) / (classes - 0.5));
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

        public static double DET(double[,] arr, int n)
        {
            int i, j, k;
            double[,] a = new double[n, n];
            for (i = 0; i < n; i++)
                for (j = 0; j < n; j++)
                    a[i, j] = arr[i, j];

            double det = 0;
            for (i = 0; i < n - 1; i++)
            {
                for (j = i + 1; j < n; j++)
                {
                    det = a[j, i] / a[i, i];
                    for (k = i; k < n; k++)
                        a[j, k] = a[j, k] - det * a[i, k]; // Here's exception
                }
            }
            det = 1;
            for (i = 0; i < n; i++)
                det = det * a[i, i];
            return det;
        }


        public static double[] VectorMinusVector(double[] a, double[] b)
        {
            double[] c = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
                c[i] = a[i] - b[i];
            return (c);
        }

        public static double VectorMultVector(double[] a, double[] b)
        {
            double c = 0;
            for (int i = 0; i < a.Length; i++)
                c += a[i] * b[i];
            return (c);
        }

        public static double[] VectorPlusVector(double[] a, double[] b)
        {
            double[] c = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
                c[i] = a[i] + b[i];
            return (c);
        }

        public static double[,] VV_Matrix(double[] a, double[] b)
        {
            double[,] c = new double[a.Length, a.Length];
            for (int i = 0; i < a.Length; i++)
                for (int j = 0; j < a.Length; j++)
                    c[i, j] = a[i] * b[j];
            return (c);
        }

        public static double[,] MatrixPlus(double[,] a, double[,] b, int dim)
        {
            double[,] c = new double[dim, dim];
            for (int i = 0; i < dim; i++)
                for (int j = 0; j < dim; j++)
                    c[i, j] = a[i, j] + b[i, j];
            return (c);
        }

        public static double[,] MatrixMultMatrix(double[,] a, double[,] b, int dim)
        {
            double[,] c = new double[dim, dim];
            for (int i = 0; i < dim; i++)
                for (int j = 0; j < dim; j++)
                    c[i, j] += a[i, j] * b[j, i];
            return (c);
        }

        public static double[,] MatrixMultNumber(double[,] a, double b, int dim)
        {
            double[,] c = new double[dim, dim];
            for (int i = 0; i < dim; i++)
                for (int j = 0; j < dim; j++)
                    c[i, j] = a[i, j] * b;
            return (c);
        }

        /// <summary>
        /// Returns vetor
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double[] MatrixMultVector(double[,] a, double[] b)
        {
            double[] c = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
                for (int j = 0; j < b.Length; j++)
                    c[i] += a[i, j] * b[j];
            return (c);
        }

        static public double[,] InverseMatrix(double[,] A)
        {
            double[,] a;
            double[,] M = new double[A.GetLength(0), A.GetLength(1)];
            for (int col = 0; col < A.GetLength(1); col++)
                for (int row = 0; row < A.GetLength(0); row++)
                    M[row, col] = Math.Pow(-1, row + col) * DET(a = SubMatrix(A, row, col), a.GetLength(0));
            M = TranspMatrix(M);
            double det = DET(A, A.GetLength(1));
            M = MatrixMultNumber(M, 1 / det, M.GetLength(1));
            return M;
        }

        static public double[,] TranspMatrix(double[,] A)
        {
            double[,] rez = new double[A.GetLength(1), A.GetLength(0)];
            for (int i1 = 0; i1 < A.GetLength(1); i1++)
                for (int i2 = 0; i2 < A.GetLength(0); i2++)
                    rez[i1, i2] = A[i2, i1];
            return rez;
        }

        static public double[,] SubMatrix(double[,] A, int row, int col)
        {
            double[,] rez = new double[A.GetLength(0) - 1, A.GetLength(1) - 1];
            for (int i1 = 0, i2 = 0; i1 < A.GetLength(0); i1++, i2++)
            {
                if (i1 == row)
                {
                    i2--;
                    continue;
                }
                for (int i3 = 0, i4 = 0; i3 < A.GetLength(1); i3++)
                {
                    if (i3 != col)
                    {
                        rez[i2, i4++] = A[i1, i3];
                    }
                }
            }
            return rez;
        }
    }
}