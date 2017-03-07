using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Laba_1_Graph
{
    public static class Graphs
    {
        public delegate double FunctionX(double x);
        public static void Histogram(Chart chart, int amount, double min, double max, ref double[] numb, double step, List<double> Xi_Value)
        {
            chart.Series.Add("histogram");
            chart.Series["histogram"].Color = Color.DarkBlue;
            chart.ChartAreas[0].AxisX.Minimum = Math.Round(min - step , 4);
            chart.ChartAreas[0].AxisX.Maximum = (max + 2 * step);
            chart.Series["histogram"].ChartType = SeriesChartType.Column;
            chart.Series["histogram"]["PointWidth"] = "1";
            chart.ChartAreas[0].AxisX.Interval = Math.Round(step, 4);
            double check1 = 0;
            if (step == 0)
                step = 1;
            double i = min;
            while (i <= max)
            {
                for (int j = 0; j < amount; j++)
                {
                    if (numb[j] >= i && numb[j] < i + step)
                    {
                        check1++;
                    }
                }
                check1 /= amount;

                Xi_Value.Add(check1);     // Used for Xi^2 of pirson

                chart.Series["histogram"].Points.AddXY(i+ step/2, check1);
                check1 = 0;
                i += step;
            }
        }
        
        public static void EmpericClass(Chart chart, int amount, double min, double max, ref double[] numb, double step, List<double> Xi_Borders)
        {
            double check2 = 0;
            chart.ChartAreas[0].AxisX.Maximum = (max + step);

            chart.Series.Add("0");
            chart.Series["0"].Color = Color.Crimson;
            chart.Series["0"].ChartType = SeriesChartType.Line;
            chart.Series["0"].Points.AddXY(min - step, 0);
            chart.Series["0"].Points.AddXY(min , 0);
            chart.ChartAreas[0].AxisX.Interval = Math.Round(step, 4);

            int k = 0;
            if(step  == 0)
                step = 1;
            double i = min;
            while(i <= max)
            {
                try
                {
                    for (int j = 0; j < amount; j++)
                    {
                        if (numb[j] >= i &&numb[j] < i + step)
                        {
                            check2++;
                        }
                    }
                    check2 /= amount;

                    Xi_Borders.Add(i);      // Used for Xi^2 of pirson

                    k++;
                    chart.Series.Add(Convert.ToString(k));
                    chart.Series[Convert.ToString(k)].Color = Color.Red;
                    chart.Series[Convert.ToString(k)].MarkerSize = 3;
                    chart.Series[Convert.ToString(k)].ChartType = SeriesChartType.Line;
                    chart.Series[Convert.ToString(k)].Points.AddXY(i , check2);
                    chart.Series[Convert.ToString(k )].Points.AddXY(i + step, check2);
                    check2 *= amount;
                }
                catch(Exception exeption)
                {
                    MessageBox.Show(exeption.Message);
                }
                i += step;
            }
        }
        
        public static void EmpericNorm(Chart chart, int amount, double min, double max  , ref double[] numb, double step)
        {
            chart.ChartAreas[0].AxisX.Minimum = Math.Round(min - step, 4);
            chart.ChartAreas[0].AxisY.Maximum = 1;
            chart.ChartAreas[0].AxisY.Minimum = 0;
            chart.Series.Add("NoClass");
            chart.Series["NoClass"].ChartType = SeriesChartType.Point;
            chart.Series["NoClass"].Color = Color.DarkGoldenrod;
            chart.ChartAreas[0].AxisX.Interval = Math.Round(step,4);
            double 
                check3 = 0,
                check4 = 0;

            int k = 0;
            double i = min;
            while(k < amount)
            {
                i = numb[k];
                for (int j = 0; j < amount; j++)
                {
                    if (numb[j] == i)
                    {
                        check3++;
                    }
                }
                k = (int)check3;
                if (check3 > check4)
                {
                    check3 /= amount;
                    chart.Series["NoClass"].Points.AddXY(i, check3);
                    check3 *= amount;
                }
                check4 = check3;
            }
        }

        public static void _2D_EmpericNorm(Chart chart, double[] numb1, double[] numb2)
        {
            //chart.ChartAreas[0].AxisX.Minimum = numb1.Min();
            //chart.ChartAreas[0].AxisX.Maximum = numb1.Max();
            //chart.ChartAreas[0].AxisY.Minimum = numb2.Min();
            //chart.ChartAreas[0].AxisY.Maximum = numb2.Max();
            chart.Series.Add("NoClass");
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "###,##0.000";
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "###,##0.000";

            chart.Series["NoClass"].ChartType = SeriesChartType.Point;
            chart.Series["NoClass"].Color = Color.Blue;
            for (int i = 0; i < numb1.Length && i < numb2.Length; i++)
                    chart.Series["NoClass"].Points.AddXY(numb1[i], numb2[i]);
        }

        public static void _2D_RegressionLinear(Chart chart, double[] numb1, double[] numb2, double a, double b, double sigm)
        {
            //chart.ChartAreas[0].AxisX.Minimum = numb1.Min();
            //chart.ChartAreas[0].AxisX.Maximum = numb1.Max();
            //chart.ChartAreas[0].AxisY.Minimum = numb2.Min();
            //chart.ChartAreas[0].AxisY.Maximum = numb2.Max();

            chart.ChartAreas[0].AxisX.LabelStyle.Format = "###,##0.000";
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "###,##0.000";

            chart.Series.Add("Line");
            chart.Series["Line"].ChartType = SeriesChartType.Line;
            chart.Series["Line"].BorderWidth = 2;
            chart.Series["Line"].Color = Color.Black;
            //      Tolerant        //
            chart.Series.Add("Line1");
            chart.Series["Line1"].ChartType = SeriesChartType.Line;
            chart.Series["Line1"].BorderWidth = 2;
            chart.Series["Line1"].Color = Color.Red;

            chart.Series.Add("Line2");
            chart.Series["Line2"].ChartType = SeriesChartType.Line;
            chart.Series["Line2"].BorderWidth = 2;
            chart.Series["Line2"].Color = Color.Red;
            //      Itervals for next regression        //
            chart.Series.Add("Line3");
            chart.Series["Line3"].ChartType = SeriesChartType.Spline;
            chart.Series["Line3"].BorderWidth = 2;
            chart.Series["Line3"].Color = Color.Green;

            chart.Series.Add("Line4");
            chart.Series["Line4"].ChartType = SeriesChartType.Spline;
            chart.Series["Line4"].BorderWidth = 2;
            chart.Series["Line4"].Color = Color.Green;




            chart.Series["Line"].Points.Add(new DataPoint(numb1.Min(), a + b * numb1.Min()));
            chart.Series["Line"].Points.Add(new DataPoint(numb1.Max(), a + b * numb1.Max()));

            chart.Series["Line1"].Points.Add(new DataPoint(numb1.Min(), a + b * numb1.Min() - Quantils.Student(numb1.Length - 2) * sigm));
            chart.Series["Line1"].Points.Add(new DataPoint(numb1.Max(), a + b * numb1.Max() - Quantils.Student(numb1.Length - 2) * sigm));
            
            chart.Series["Line2"].Points.Add(new DataPoint(numb1.Min(), a + b * numb1.Min() + Quantils.Student(numb1.Length - 2) * sigm));
            chart.Series["Line2"].Points.Add(new DataPoint(numb1.Max(), a + b * numb1.Max() + Quantils.Student(numb1.Length - 2) * sigm));
            
            double[] arr_1 = new double[numb1.Length];
            for (int i = 0; i < numb1.Length; i++)
                arr_1[i] = numb1[i];
            Counts.Sort(arr_1);

            for (int i = 0; i < arr_1.Length; i++)
            {
                chart.Series["Line3"].Points.Add(new DataPoint(arr_1[i],
                    a + b * arr_1[i] - Quantils.Student(arr_1.Length - 2) * Regression.Linear.Interval(arr_1, i, sigm * sigm)));

                chart.Series["Line4"].Points.Add(new DataPoint(arr_1[i],
                    a + b * arr_1[i] + Quantils.Student(arr_1.Length - 2) * Regression.Linear.Interval(arr_1, i, sigm * sigm)));
            }
        }

        public static void _2D_RegressionParabol(Chart chart, double[] arr1, double[] arr2, double[] abc, double S2_P, double S2_P2)
        {
            int N = arr1.Length;

            double[] arr_1 = new double[N];
            for (int i = 0; i < N; i++)
                arr_1[i] = arr1[i];
            Counts.Sort(arr_1);
            //chart.ChartAreas[0].AxisX.Minimum = arr_1.Min();
            //chart.ChartAreas[0].AxisX.Maximum = arr_1.Max();
            //chart.ChartAreas[0].AxisY.Minimum = arr2.Min();
            //chart.ChartAreas[0].AxisY.Maximum = arr2.Max();
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "###,##0.000";
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "###,##0.000";

            chart.Series.Add("Line");
            chart.Series["Line"].ChartType = SeriesChartType.Spline;
            chart.Series["Line"].BorderWidth = 2;
            chart.Series["Line"].Color = Color.Black;
            //      Tolerant        //
            chart.Series.Add("Line1");
            chart.Series["Line1"].ChartType = SeriesChartType.Spline;
            chart.Series["Line1"].BorderWidth = 2;
            chart.Series["Line1"].Color = Color.Red;

            chart.Series.Add("Line2");
            chart.Series["Line2"].ChartType = SeriesChartType.Spline;
            chart.Series["Line2"].BorderWidth = 2;
            chart.Series["Line2"].Color = Color.Red;
            //      Itervals for next regression        //
            chart.Series.Add("Line3");
            chart.Series["Line3"].ChartType = SeriesChartType.Spline;
            chart.Series["Line3"].BorderWidth = 2;
            chart.Series["Line3"].Color = Color.Green;

            chart.Series.Add("Line4");
            chart.Series["Line4"].ChartType = SeriesChartType.Spline;
            chart.Series["Line4"].BorderWidth = 2;
            chart.Series["Line4"].Color = Color.Green;

            double x_2 = 0,
                    x_3 = 0,
                    f1 = 0,
                    f2 = 0;
            double sigm_1 = 0, sigm_2 = 0;

            for (int i = 0; i < N; i++)
            {
                x_2 += Math.Pow(arr_1[i], 2);
                x_3 += Math.Pow(arr_1[i], 3);
            }
            x_2 /= N;
            x_3 /= N;

            double f2_average = 0;
            for (int i = 0; i < N; i++)
            {
                f2 = arr_1[i] * arr_1[i] -
                    ((x_3 - arr_1.Average() * x_2) / (x_2 - N * Math.Pow(arr_1.Average(), 2)))
                    * (arr_1[i] - arr_1.Average());
                f2_average += f2 * f2;
            }

            double Syx = 0;
            sigm_1 = Functions.Sigm(N, arr1.Average(), arr1);
            sigm_2 = Functions.Sigm(N, arr2.Average(), arr2);
            for (int i = 0; i < N; i++)
            {
                f1 = arr_1[i] - arr_1.Average();
                f2 = arr_1[i] * arr_1[i] -
                    ((x_3 - arr_1.Average() * x_2) / (x_2 - N * Math.Pow(arr_1.Average(), 2)))
                    * (arr_1[i] - arr_1.Average());

                chart.Series["Line"].Points.Add(new DataPoint(arr_1[i], abc[0] + abc[1] * arr_1[i] + abc[2] * arr_1[i] * arr_1[i]));

                chart.Series["Line1"].Points.Add(new DataPoint(arr_1[i], abc[0] + abc[1] * arr_1[i] + abc[2] * arr_1[i] * arr_1[i] - Quantils.Student(N - 3) * Math.Sqrt(S2_P2)));
                chart.Series["Line2"].Points.Add(new DataPoint(arr_1[i], abc[0] + abc[1] * arr_1[i] + abc[2] * arr_1[i] * arr_1[i] + Quantils.Student(N - 3) * Math.Sqrt(S2_P2)));

                Syx = (S2_P2 / Math.Sqrt(N)) * Math.Sqrt(1 + (f1 * f1) / (sigm_1 * sigm_1) + (f2 * f2) / (f2_average));
                chart.Series["Line3"].Points.Add(new DataPoint(arr_1[i], abc[0] + abc[1] * arr_1[i] + abc[2] * arr_1[i] * arr_1[i] - Quantils.Student(N - 3) * Math.Sqrt(Syx)));
                chart.Series["Line4"].Points.Add(new DataPoint(arr_1[i], abc[0] + abc[1] * arr_1[i] + abc[2] * arr_1[i] * arr_1[i] + Quantils.Student(N - 3) * Math.Sqrt(Syx)));
            }
        }

        public static void _2D_RegressionKvazilinear(Chart chart, double[] arr1, double[] arr2, double[] ab, double S2_K,
                FunctionX Fx, int model)
        {
            int N = arr1.Length;

            double w = arr2.Average();
            double[] arr_1 = new double[N];
            for (int i = 0; i < N; i++)
                arr_1[i] = arr1[i];
            Counts.Sort(arr_1);
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "###,##0.000";
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "###,##0.000";

            chart.Series.Add("Line");
            chart.Series["Line"].ChartType = SeriesChartType.Spline;
            chart.Series["Line"].BorderWidth = 2;
            chart.Series["Line"].Color = Color.Black;
            //      Tolerant        //
            chart.Series.Add("Line1");
            chart.Series["Line1"].ChartType = SeriesChartType.Spline;
            chart.Series["Line1"].BorderWidth = 2;
            chart.Series["Line1"].Color = Color.Red;

            chart.Series.Add("Line2");
            chart.Series["Line2"].ChartType = SeriesChartType.Spline;
            chart.Series["Line2"].BorderWidth = 2;
            chart.Series["Line2"].Color = Color.Red;
            if (model == 11)
                for (int i = 0; i < N; i++)
                {
                    chart.Series["Line"].Points.Add(new DataPoint(arr_1[i], 1 / (ab[0] + ab[1] * Fx(arr_1[i]))));
                    chart.Series["Line1"].Points.Add(new DataPoint(arr_1[i], 1 / (ab[0] + ab[1] * Fx(arr_1[i])) - Quantils.Student(N - 3) * Math.Sqrt(S2_K)));
                    chart.Series["Line2"].Points.Add(new DataPoint(arr_1[i], 1 / (ab[0] + ab[1] * Fx(arr_1[i])) + Quantils.Student(N - 3) * Math.Sqrt(S2_K)));
                }
            else if (model == 2)
                for (int i = 0; i < N; i++)
                {
                    chart.Series["Line"].Points.Add(new DataPoint(arr_1[i], Math.Sqrt(ab[0] + ab[1] * Fx(arr_1[i]))));
                    chart.Series["Line1"].Points.Add(new DataPoint(arr_1[i], Math.Sqrt(ab[0] + ab[1] * Fx(arr_1[i]))- Quantils.Student(N - 3) * Math.Sqrt(S2_K)));
                    chart.Series["Line2"].Points.Add(new DataPoint(arr_1[i], Math.Sqrt(ab[0] + ab[1] * Fx(arr_1[i])) + Quantils.Student(N - 3) * Math.Sqrt(S2_K)));
        }
            else if (model == 3)
                for (int i = 0; i < N; i++)
                {
                    chart.Series["Line"].Points.Add(new DataPoint(arr_1[i], (ab[0] + ab[1] * Fx(arr_1[i]))));
                    chart.Series["Line1"].Points.Add(new DataPoint(arr_1[i], (ab[0] + ab[1] * Fx(arr_1[i])) - Quantils.Student(N - 3) * Math.Sqrt(S2_K)));
                    chart.Series["Line2"].Points.Add(new DataPoint(arr_1[i], (ab[0] + ab[1] * Fx(arr_1[i])) + Quantils.Student(N - 3) * Math.Sqrt(S2_K)));
                }
            else if (model == 4)
                for (int i = 0; i < N; i++)
                {
                    chart.Series["Line"].Points.Add(new DataPoint(arr_1[i], 1 / (ab[0] + ab[1] * Fx(arr_1[i]))));
                    chart.Series["Line1"].Points.Add(new DataPoint(arr_1[i], 1 / (ab[0] + ab[1] * Fx(arr_1[i])) - Quantils.Student(N - 3) * Math.Sqrt(S2_K)));
                    chart.Series["Line2"].Points.Add(new DataPoint(arr_1[i], 1 / (ab[0] + ab[1] * Fx(arr_1[i])) + Quantils.Student(N - 3) * Math.Sqrt(S2_K)));
                }
            else if (model == 6)
                for (int i = 0; i < N; i++)
                {
                    chart.Series["Line"].Points.Add(new DataPoint(arr_1[i], (ab[0] + ab[1] * Fx(arr_1[i]))));
                    chart.Series["Line1"].Points.Add(new DataPoint(arr_1[i], (ab[0] + ab[1] * Fx(arr_1[i])) - Quantils.Student(N - 3) * Math.Sqrt(S2_K)));
                    chart.Series["Line2"].Points.Add(new DataPoint(arr_1[i], (ab[0] + ab[1] * Fx(arr_1[i])) + Quantils.Student(N - 3) * Math.Sqrt(S2_K)));
                }
            else if (model == 7)
                for (int i = 0; i < N; i++)
                {
                    chart.Series["Line"].Points.Add(new DataPoint(arr_1[i], Math.Exp(ab[0] + ab[1] * Fx(arr_1[i]))));
                    chart.Series["Line1"].Points.Add(new DataPoint(arr_1[i], Math.Exp(ab[0] + ab[1] * Fx(arr_1[i])) - Quantils.Student(N - 3) * Math.Sqrt(S2_K)));
                    chart.Series["Line2"].Points.Add(new DataPoint(arr_1[i], Math.Exp(ab[0] + ab[1] * Fx(arr_1[i])) + Quantils.Student(N - 3) * Math.Sqrt(S2_K)));
                }
        }

        public static void _2D_Histogram(Chart chart, double[] arr_1, double[] arr_2, double step_1, double step_2)
        {
            //chart.ChartAreas[0].AxisX.Minimum = arr_1.Min();
            //chart.ChartAreas[0].AxisX.Maximum = arr_1.Max();
            //chart.ChartAreas[0].AxisY.Minimum = arr_2.Min();
            //chart.ChartAreas[0].AxisY.Maximum = arr_2.Max();
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "###,##0.000";
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "###,##0.000";
            chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            int[,] F = new int
                [
                Convert.ToInt32((arr_2.Max() - arr_2.Min()) / step_2) + 2,
                Convert.ToInt32((arr_1.Max() - arr_1.Min()) / step_1) + 2
                ];
            int m = 0, n = 0;
            bool found = false;
            for (int i = 0; i < arr_1.Length && i < arr_2.Length; i++)
            {
                m = 0;
                found = false;
                for (double p = arr_1.Min(); p < arr_1.Max(); p += step_1)
                {
                    n = 0;
                    for (double k = arr_2.Min(); k < arr_2.Max(); k += step_2)
                    {
                        if (arr_1[i] >= p && arr_1[i] < p + step_1 &&
                            arr_2[i] >= k && arr_2[i] < k + step_2)
                        {
                            F[m, n]++;
                            found = true;
                            break;
                        }
                        n++;
                    }
                    if (found)
                        break;
                    m++;
                }
            }

            double col = 0;
            for (int i = 0; i < F.GetLength(0); i++)
                for (int j = 0; j < F.GetLength(1); j++)
                    col = Math.Max(col, Math.Abs(F[i, j]));
            string str2 = "";
            m = 0;
            for (double p = arr_1.Min(); p < arr_1.Max() && m < F.GetLength(0); p += step_1)
            {
                n = 0;
                for (double k = arr_2.Min(); k < arr_2.Max() && n < F.GetLength(1); k += step_2)
                {
                    str2 = "f" + p.ToString() + k.ToString();
                    chart.Series.Add(str2);
                    chart.Series[str2].ChartType = SeriesChartType.Point;
                    chart.Series[str2].MarkerStyle = MarkerStyle.Square;
                    chart.Series[str2].IsVisibleInLegend = false;
                    chart.Series[str2].MarkerSize = (chart.Size.Width - 20 + Convert.ToInt32(step_2) * 10) / F.GetLength(0);
                    
                    chart.Series[str2].Color = Color.FromArgb(
                        255 - (int)(255 / col) * F[m, n],
                        255 - (int)(255 / col) * F[m, n],
                        255);

                    chart.Series[str2]["StackedGroupName"] = "Group" + k.ToString();
                    chart.Series[str2].Points.AddXY(p + step_1, k + step_2);
                    n++;
                }
                m++;
            }
        }
    }
}