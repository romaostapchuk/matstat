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

    /// <summary>
    /// Class of graphics used to build 3 different graphics 
    /// </summary>
    public static class Graphs
    {
        /// <summary>
        /// Builds a histogram
        /// </summary>
        /// <param name="chart">Chart for your graph</param>
        /// <param name="amount">Length of your array</param>
        /// <param name="min">Min value of array</param>
        /// <param name="max">Max value of array</param>
        /// <param name="numb">Name of your aray</param>
        /// <param name="step">Step of your function</param>
        /// <param name="Xi_Value">Array of values used for Xi square</param>
        /// <param name="Xi_Borders">Array of border values used for Xi square</param>
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
        /// <summary>
        /// Build an emperic classified  function
        /// </summary>
        /// <param name="chart">Chart for your graph</param>
        /// <param name="amount">Length of your array</param>
        /// <param name="min">Min value of array</param>
        /// <param name="max">Max value of array</param>
        /// <param name="numb">Name of your aray</param>
        /// <param name="step">Step of your function</param>
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
        /// <summary>
        /// Builds an emperic function
        /// </summary>
        /// <param name="chart">Chart for your graph</param>
        /// <param name="amount">Length of your array</param>
        /// <param name="min">Min value of array</param>
        /// <param name="max">Max value of array</param>
        /// <param name="numb">Name of your aray</param>
        /// <param name="step">Step of your function</param>
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
    }
}