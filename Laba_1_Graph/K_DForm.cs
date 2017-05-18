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
using System.Windows.Input;


namespace Laba_1_Graph
{
    public partial class K_DForm : Form
    {
        public K_DForm()
        {
            InitializeComponent();
            this.tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
        }

        public void GetValues(List<double[]> arr)
        {
            DataStorage.N_size = false;
            DataStorage.Data = new List<double[]>(arr);
            DataStorage.DC = new List<List<double>>();
            DataStorage.R = new List<List<double>>();

            CountAll();
            ShowData(0);
        }
        public void Get_N_Values(List<List<double[]>> arr)
        {
            DataStorage.N_Data = new List<List<double[]>>(arr);
            DataStorage.N_size = true;

            button4.Visible = false;
            button5.Visible = false;
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;

            CountN();
            ShowDataN(0);
        }

        private void CountN()
        {
            DataStorage.N_Compares = new List<List<double>>();
            DataStorage.N_Names = new List<string>();
            List<double> Temp = new List<double>();

            bool check = false;
            if (DataStorage.N_Data.Count == 2)      //average for 2 
            {
                double V2 = Comparing.Compare2Averages(DataStorage.N_Data[0], DataStorage.N_Data[1], ref check);

                Temp = new List<double>();
                Temp.Add(V2);
                Temp.Add(check == true ? 1 : -1);

                DataStorage.N_Compares.Add(Temp);
                DataStorage.N_Names.Add("Порівняння середніх 2");
            }
            if (DataStorage.N_Data.Count >= 2)      // average for >= 2
            {
                double Vk = Comparing.CompareKAverages(DataStorage.N_Data, ref check);

                Temp = new List<double>();
                Temp.Add(Vk);
                Temp.Add(check == true ? 1 : -1);

                DataStorage.N_Compares.Add(Temp);
                DataStorage.N_Names.Add("Порівняння середніх k");
            }
            if (DataStorage.N_Data.Count >= 2)      // DC matrix comparing
            {
                double VDC = Comparing.CompareKDC(DataStorage.N_Data, ref check);

                Temp = new List<double>();
                Temp.Add(VDC);
                Temp.Add(check == true ? 1 : -1);

                DataStorage.N_Compares.Add(Temp);
                DataStorage.N_Names.Add("Порівняння матриць DC");
            }
        }
        private void CountAll()
        {
            DataStorage.Params = new List<List<double>>();
            DataStorage.ParamNames = new List<string>();
            List<double> Temp = new List<double>();

            // Dispersions [0]
            for (int i = 0; i < DataStorage.Data.Count; i++)
                Temp.Add(Functions.Disp(DataStorage.Data[i].Length, DataStorage.Data[i].Average(), DataStorage.Data[i]));
            DataStorage.Params.Add(Temp);
            DataStorage.ParamNames.Add("Дисперсія");
            Temp = new List<double>();

            // Dispersions [0]
            for (int i = 0; i < DataStorage.Data.Count; i++)
                Temp.Add(DataStorage.Data[i].Average());
            DataStorage.Params.Add(Temp);
            DataStorage.ParamNames.Add("Мат. Сподівання");
            Temp = new List<double>();

            for (int i = 0; i < DataStorage.Data.Count; i++)
            {
                Temp = new List<double>();
                for (int j = 0; j < DataStorage.Data.Count; j++)
                {
                    Temp.Add(Functions.Sigm(DataStorage.Data[i].Length, DataStorage.Data[i].Average(), DataStorage.Data[i]) *
                        Functions.Sigm(DataStorage.Data[j].Length, DataStorage.Data[j].Average(), DataStorage.Data[j]) *
                        Correlation.Pair(DataStorage.Data[i], DataStorage.Data[j], DataStorage.Data[i].Average(), DataStorage.Data[j].Average()));
                }
                DataStorage.DC.Add(Temp);
            }
            Temp = new List<double>();

            for (int i = 0; i < DataStorage.Data.Count; i++)
            {
                Temp = new List<double>();
                for (int j = 0; j < DataStorage.Data.Count; j++)
                {
                    Temp.Add(Correlation.Pair(DataStorage.Data[i], DataStorage.Data[j], DataStorage.Data[i].Average(), DataStorage.Data[j].Average()));
                }
                DataStorage.R.Add(Temp);
            }
            Temp = new List<double>();
        }
        
        
        private void ShowData(int tab)
        {
            if (tab == 0)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.ColumnCount = DataStorage.Data.Count;

                for (int i = 0; i < DataStorage.Data.Count; i++)
                    dataGridView1.Columns[i].Width = (dataGridView1.Width - dataGridView1.RowHeadersWidth) /
                        DataStorage.Data.Count - DataStorage.Data.Count;

                double[] data = new double[DataStorage.Data.Count];
                if (DataStorage.Data.Count > 0)
                    for (int i = 0; i < DataStorage.Data[0].Length; i++)
                    {
                        dataGridView1.Rows.Add();
                        for (int j = 0; j < DataStorage.Data.Count; j++)
                        {
                            data[j] = DataStorage.Data[j][i];
                            dataGridView1[j, i].Value = Math.Round(DataStorage.Data[j][i], 4);
                            dataGridView1[j, i].Style.BackColor = DG_Color(DataStorage.Data[j][i], j);
                        }
                        dataGridView1.Rows[i].HeaderCell.Value = (i + 1);
                    }
            }
            else if (tab == 1)
            {
                chart1.Series.Clear();
                chart1.ChartAreas.Clear();

                DataStorage.step = Convert.ToInt32(comboBox1.Text);
                int ind = 0;
                for (int i = 0; i < DataStorage.step; i++)
                {
                    chart1.ChartAreas.Add(i.ToString());
                    chart1.Series.Add(i.ToString());
                    chart1.Series[i.ToString()].ChartArea = i.ToString();
                    chart1.Series[i.ToString()].ChartType = SeriesChartType.Radar;
                    chart1.ChartAreas[i.ToString()].AxisY.MajorGrid.Enabled = false;
                    chart1.ChartAreas[i.ToString()].AxisY.LabelStyle.Enabled = false;
                    chart1.ChartAreas[i.ToString()].AxisY.Interval = 1;
                    chart1.ChartAreas[i.ToString()].AxisY.Maximum = 1;
                    chart1.ChartAreas[i.ToString()].AxisY.MajorTickMark.Enabled = false;
                    chart1.Series[i.ToString()].Color = Color.DarkBlue;
                    ind = i+1;
                }

                for (int i = DataStorage.index; i < DataStorage.Data[0].Length &&
                i < DataStorage.step + DataStorage.index; i++)
                    for (int j = 0; j < DataStorage.Data.Count; j++)
                    {
                        chart1.Series[(i - DataStorage.index).ToString()].Points.AddY(
                            (DataStorage.Data[j][i] - DataStorage.Data[j].Min()) / (DataStorage.Data[j].Max() - DataStorage.Data[j].Min()));
                    }
                label2.Text = ("[" + (DataStorage.index + 1) + "-" + Math.Min(DataStorage.Data[0].Length, DataStorage.index + DataStorage.step) + "]");
            }
            else if (tab == 2)
            {
                chart2.Series.Clear();
                chart2.ChartAreas[0].AxisY.Maximum = 1;
                chart2.ChartAreas[0].AxisX.Maximum = 1;
                chart2.ChartAreas[0].AxisX.Minimum = 0;
                chart2.ChartAreas[0].AxisX.LabelStyle.Format = "###,##0.000";
                chart2.ChartAreas[0].AxisY.LabelStyle.Format = "###,##0.000";

                for (int i = 0; i < DataStorage.Data[0].Length; i++)
                {
                    chart2.Series.Add(i.ToString());
                    chart2.Series[i.ToString()].ChartType = SeriesChartType.Line;
                    chart2.Series[i.ToString()].Color = Color.DarkBlue;
                }

                for (int i = 0; i < DataStorage.Data[0].Length; i++)
                    for (int j = 0; j < DataStorage.Data.Count; j++)
                    {
                        chart2.Series[i.ToString()].Points.AddXY(
                            (double)(j) / (DataStorage.Data.Count - 1), 
                            (DataStorage.Data[j][i] - DataStorage.Data[j].Min()) / (DataStorage.Data[j].Max() - DataStorage.Data[j].Min()));
                    }
            }
            else if (tab == 3)
            {
                chart3.Series.Clear();
                chart3.ChartAreas.Clear();

                for (int i = 0; i < DataStorage.Data.Count; i++)
                {
                    for (int j = 0; j < DataStorage.Data.Count; j++)
                    {
                        chart3.ChartAreas.Add(i.ToString() + "_" + j.ToString());

                        chart3.Series.Add(i.ToString() + "_" + j.ToString());
                        chart3.Series.Add(i.ToString() + "_" + j.ToString() + "L");

                        chart3.Series[i.ToString() + "_" + j.ToString()].ChartArea = (i.ToString() + "_" + j.ToString());
                        chart3.Series[i.ToString() + "_" + j.ToString() + "L"].ChartArea = (i.ToString() + "_" + j.ToString());

                        chart3.Series[i.ToString() + "_" + j.ToString()].ChartType = SeriesChartType.Point;
                        chart3.Series[i.ToString() + "_" + j.ToString()].Color = Color.Blue;

                        chart3.Series[i.ToString() + "_" + j.ToString() + "L"].ChartType = SeriesChartType.Line;
                        chart3.Series[i.ToString() + "_" + j.ToString() + "L"].Color = Color.Red;
                    }
                }
                double a = 0;
                double b = 0;
                for (int i = 0; i < DataStorage.Data.Count; i++)
                {
                    for (int j = 0; j < DataStorage.Data.Count; j++)
                    {
                        chart3.ChartAreas[i.ToString() + "_" + j.ToString()].AxisX.LabelStyle.Enabled = false;
                        chart3.ChartAreas[i.ToString() + "_" + j.ToString()].AxisY.LabelStyle.Enabled = false;
                        if (i != j)
                        {
                            chart3.ChartAreas[i.ToString() + "_" + j.ToString()].AxisX.Minimum = DataStorage.Data[i].Min();
                            chart3.ChartAreas[i.ToString() + "_" + j.ToString()].AxisX.Maximum = DataStorage.Data[i].Max();
                            chart3.ChartAreas[i.ToString() + "_" + j.ToString()].AxisY.Minimum = DataStorage.Data[j].Min();
                            chart3.ChartAreas[i.ToString() + "_" + j.ToString()].AxisY.Maximum = DataStorage.Data[j].Max();

                            Regression.Linear.MNK(DataStorage.Data[i], DataStorage.Data[j], ref a, ref b);
                            chart3.Series[i.ToString() + "_" + j.ToString() + "L"].Points.Add(new DataPoint(DataStorage.Data[i].Min(), a + b * DataStorage.Data[i].Min()));
                            chart3.Series[i.ToString() + "_" + j.ToString() + "L"].Points.Add(new DataPoint(DataStorage.Data[i].Max(), a + b * DataStorage.Data[i].Max()));
                            for (int p = 0; p < DataStorage.Data[i].Length; p++)
                                chart3.Series[i.ToString() + "_" + j.ToString()].Points.Add(new DataPoint(DataStorage.Data[i][p], DataStorage.Data[j][p]));
                        }
                        else
                        {
                            double min = DataStorage.Data[i].Min();
                            double max = DataStorage.Data[i].Max();
                            int amount = DataStorage.Data[i].Length;
                            double step = Counts.Step(0, amount, min, max);


                            chart3.Series[i.ToString() + "_" + j.ToString()].ChartType = SeriesChartType.Column;
                            chart3.Series[i.ToString() + "_" + j.ToString()]["PointWidth"] = "1";
                            chart3.ChartAreas[0].AxisX.Minimum = Math.Round(min - step, 4);
                            chart3.ChartAreas[0].AxisX.Maximum = (max + 2 * step);
                            chart3.ChartAreas[0].AxisX.Interval = Math.Round(step, 4);
                            double check1 = 0;
                            if (step == 0)
                                step = 1;
                            double p = min;
                            while (p <= max)
                            {
                                for (int q = 0; q < amount; q++)
                                {
                                    if (DataStorage.Data[i][q] >= p && DataStorage.Data[i][q] < p + step)
                                    {
                                        check1++;
                                    }
                                }
                                check1 /= amount;

                                chart3.Series[i.ToString() + "_" + j.ToString()].Points.AddXY(p + step / 2, check1);
                                check1 = 0;
                                p += step;
                            }
                        }
                    }
                }
            }
            else if (tab == 4)
            {
                //dataGridView3.Rows.Clear();
                //dataGridView3.Columns.Clear();
                //dataGridView3.ColumnCount = 4;

                //for (int i = 0; i < dataGridView3.ColumnCount; i++)
                //    dataGridView3.Columns[i].Width = (dataGridView3.Width - dataGridView3.RowHeadersWidth) /
                //        dataGridView3.ColumnCount - dataGridView3.ColumnCount;

                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
                dataGridView2.ColumnCount = DataStorage.Params[0].Count;

                for (int i = 0; i < DataStorage.Params[0].Count; i++)
                    dataGridView2.Columns[i].Width = (dataGridView2.Width - dataGridView2.RowHeadersWidth) /
                        DataStorage.Params[0].Count - DataStorage.Params[0].Count;
                
                for (int i = 0; i < DataStorage.Params[0].Count; i++)
                {
                    dataGridView2.Rows.Add();
                    for (int j = 0; j < DataStorage.Params[0].Count; j++)
                    {
                        if (radioButton4.Checked == true)
                            dataGridView2[j, i].Value = Math.Round(DataStorage.DC[i][j], 4);
                        else if (radioButton3.Checked == true)
                            dataGridView2[j, i].Value = Math.Round(DataStorage.R[i][j], 4);
                    }
                }
            }
        }
        private void ShowCompares(int tab)
        {
            if (tab == 0)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
            }
            else if (tab == 1)
            {
                chart1.Series.Clear();
                chart1.ChartAreas.Clear();
            }
            else if (tab == 2)
            {
                chart2.Series.Clear();
                chart2.ChartAreas.Clear();
            }
            else if (tab == 3)
            {
                chart3.Series.Clear();
                chart3.ChartAreas.Clear();
            }
            else if (tab == 4)
            {
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
                dataGridView2.ColumnCount = DataStorage.DC.Count;

                for (int i = 0; i < DataStorage.Params[0].Count; i++)
                    dataGridView2.Columns[i].Width = (dataGridView2.Width - dataGridView2.RowHeadersWidth) /
                        DataStorage.Params[0].Count - DataStorage.Params[0].Count;


                dataGridView3.Rows.Clear();
                dataGridView3.Columns.Clear();
                dataGridView3.ColumnCount = DataStorage.Params[0].Count + 1;

                for (int i = 0; i < dataGridView3.ColumnCount; i++)
                    dataGridView3.Columns[i].Width = (dataGridView3.Width - dataGridView3.RowHeadersWidth) /
                        dataGridView3.ColumnCount - dataGridView3.ColumnCount;

                for (int i = 0; i < DataStorage.Params.Count; i++)
                {
                    dataGridView3.Rows.Add();
                    dataGridView3[0, i].Value = DataStorage.ParamNames[i];
                    for (int j = 0; j < DataStorage.Params[i].Count; j++)
                    {
                        dataGridView3[j + 1, i].Value = Math.Round(DataStorage.Params[i][j], 4);
                    }
                }
            }
        }
        private void ShowDataN(int tab)
        {
            if (tab == 0)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.ColumnCount = DataStorage.N_Compares[0].Count + 1;

                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                    dataGridView1.Columns[i].Width = (dataGridView1.Width - dataGridView1.RowHeadersWidth) /
                        dataGridView1.ColumnCount - dataGridView1.ColumnCount;
                dataGridView1.Columns[1].HeaderText = "значення";

                for (int i = 0; i < DataStorage.N_Compares.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1[0, i].Value = DataStorage.N_Names[i];
                    for (int j = 0; j < DataStorage.N_Compares[i].Count; j++)
                    {
                        dataGridView1[j + 1, i].Value = Math.Round(DataStorage.N_Compares[i][j], 4);
                        if (j == 1)
                            dataGridView1[j + 1, i].Value = DataStorage.N_Compares[i][j] == 0 ? "" : DataStorage.N_Compares[i][j] == 1 ? "true" : "false";
                    }
                }
            }
            else if (tab == 1)
            {
                chart1.Series.Clear();
                chart1.ChartAreas.Clear();
            }
            else if (tab == 2)
            {
                chart2.Series.Clear();
                chart2.ChartAreas.Clear();
            }
            else if (tab == 3)
            {
                chart3.Series.Clear();
                chart3.ChartAreas.Clear();
            }
            else if (tab == 4)
            {
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
            }
        }

        private Color DG_Color(double value, int index)
        {
            Color ret;

            double n = (value - DataStorage.Data[index].Min()) /
                (DataStorage.Data[index].Max() - DataStorage.Data[index].Min());
            ret = Color.FromArgb(255 - (int)(n * 255), 255 - (int)(n * 255), 255);
            return (ret);
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true && DataStorage.N_size == false)
            {
                ShowData(tabControl1.SelectedIndex);
            }
            else if (radioButton2.Checked == true && DataStorage.N_size == false)
            {
                ShowCompares(tabControl1.SelectedIndex);
            }
            else if (radioButton1.Checked == true && DataStorage.N_size == true)
            {
                ShowDataN(tabControl1.SelectedIndex);
            }
            else if (radioButton2.Checked == true && DataStorage.N_size == true)
            {
                MessageBox.Show("Nothing to show");
            }
        } 


        private void button1_Click(object sender, EventArgs e)
        {
            if (DataStorage.step == 0)
                DataStorage.step = Convert.ToInt32(comboBox1.Text);
            if (DataStorage.index > 0)
            {
                DataStorage.index -= DataStorage.step;
                if (DataStorage.index < 0)
                    DataStorage.index = 0;
            }

            radioButton1_CheckedChanged(sender, e);
        }
        private void button2_Click(object sender, EventArgs e)
        {

            if (DataStorage.step == 0)
                DataStorage.step = Convert.ToInt32(comboBox1.Text);
            if (DataStorage.index >= 0 && DataStorage.index < DataStorage.Data[0].Length)
            {
                DataStorage.index += DataStorage.step;
                if (DataStorage.index >= DataStorage.Data[0].Length)
                    DataStorage.index = DataStorage.Data[0].Length - 1;
            }

            radioButton1_CheckedChanged(sender, e);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DataStorage.index = 0;
            DataStorage.step = 1;
            this.Close();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            radioButton1_CheckedChanged(sender, e);
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            radioButton1_CheckedChanged(sender, e);
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1_CheckedChanged(sender, e);
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
        }


        private class Comparing
        {
            public static double Compare2Averages(List<double[]> x, List<double[]> y, ref bool check)
            {
                double V = 0;

                int
                    N1 = x[0].Length,
                    N2 = y[0].Length;
                int n = x.Count;


                double[,] S0 = new double[n, n];
                double[,] S1 = new double[n, n];

                double S0i = 0, S1i = 0;
                double temp = 0;
                double buff;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        temp = 0;
                        for (int l = 0; l < N2; l++)
                            temp += y[j][l];
                        S0i = temp;
                        temp = 0;
                        for (int l = 0; l < N1; l++)
                            temp += x[j][l];
                        S0i += temp;
                        temp = 0;
                        for (int l = 0; l < N2; l++)
                            temp += y[i][l];
                        buff = temp;
                        temp = 0;
                        for (int l = 0; l < N1; l++)
                            temp += x[i][l];
                        temp += buff;
                        S0i *= temp;
                        S0i /= -(N1 + N2);
                        temp = 0;
                        for (int l = 0; l < N2; l++)
                            temp += y[i][l] * y[j][l];
                        S0i += temp;
                        temp = 0;
                        for (int l = 0; l < N1; l++)
                            temp += x[i][l] * x[j][l];
                        S0i += temp;
                        temp = 0;
                        S0i /= (N1 + N2 - 2);

                        S0[i, j] = S0i; //add to matrix

                        for (int l = 0; l < N2; l++)
                            temp += y[j][l];
                        S1i = temp;
                        temp = 0;
                        for (int l = 0; l < N2; l++)
                            temp += y[i][l];
                        S1i *= temp;
                        S1i /= (-N2);
                        temp = 0;

                        for (int l = 0; l < N1; l++)
                            temp += x[j][l];
                        buff = temp;
                        temp = 0;
                        for (int l = 0; l < N1; l++)
                            temp += x[i][l];
                        temp *= buff;
                        temp /= (-N1);

                        S1i += temp;
                        temp = 0;
                        for (int l = 0; l < N2; l++)
                            temp += y[i][l] * y[j][l];
                        S1i += temp;
                        temp = 0;
                        for (int l = 0; l < N1; l++)
                            temp += x[i][l] * x[j][l];
                        S1i += temp;
                        temp = 0;
                        S1i /= (N1 + N2 - 2);

                        S1[i, j] = S1i; //add to matrix
                    }
                }
                double S0_d = Counts.DET(S0, n);
                double S1_d = Counts.DET(S1, n);

                V = -(N1 + N2 - 2 - (n / 2)) * Math.Log(Math.Abs(S1_d) / Math.Abs(S0_d));
                check = false;
                if (V <= Quantils.XiSquare(0.95, n))
                    check = true;
                return (V);
            }

            public static double CompareKAverages(List<List<double[]>> Data, ref bool check)
            {
                double V = 0;

                int k = Data.Count;
                int n = Data[0].Count;

                double[] xd;
                double[] Xld;
                double[,] Sd;
                double[] x;

                double[] buff;
                double[,] MBuff;

                double[,] Mbuff_1 = new double[n, n];
                double[] buff_1 = new double[n];
                double temp = 0;

                for (int d = 0; d < k; d++)
                {
                    xd = new double[n];
                    Sd = new double[n, n];

                    for (int i = 0; i < n; i++)
                        xd[i] = Data[d][i].Average();
                    for (int i = 0; i < Data[d][0].Length; i++)
                    {
                        Xld = new double[n];
                        for (int j = 0; j < n; j++)
                        {
                            Xld[j] = Data[d][j][i];
                        }
                        buff = Counts.VectorMinusVector(Xld, xd);
                        MBuff = Counts.VV_Matrix(buff, buff);
                        Sd = Counts.MatrixPlus(Sd, MBuff, n);
                    }
                    Sd = Counts.MatrixMultNumber(Sd, (1.0 / (Data[d][0].Length - 1)), n);

                    double[,] SdSd = Counts.InverseMatrix(Sd);
                    double[,] aa = Counts.MatrixMultMatrix(SdSd, Sd, n);
                    Sd = Counts.MatrixMultNumber(Sd, Data[d][0].Length, n); // 1

                    buff = Counts.MatrixMultVector(Sd, xd);                 // 2

                    buff_1 = Counts.VectorPlusVector(buff, buff_1);
                    Mbuff_1 = Counts.MatrixPlus(Mbuff_1, Sd, n);
                }
                Mbuff_1 = Counts.InverseMatrix(Mbuff_1);
                x = Counts.MatrixMultVector(Mbuff_1, buff_1);

                for (int d = 0; d < k; d++)
                {
                    xd = new double[n];
                    Sd = new double[n, n];

                    for (int i = 0; i < n; i++)
                        xd[i] = Data[d][i].Average();
                    for (int i = 0; i < Data[d][0].Length; i++)
                    {
                        Xld = new double[n];
                        for (int j = 0; j < n; j++)
                        {
                            Xld[j] = Data[d][j][i];
                        }
                        buff = Counts.VectorMinusVector(Xld, xd);
                        MBuff = Counts.VV_Matrix(buff, buff);
                        Sd = Counts.MatrixPlus(Sd, MBuff, n);
                    }
                    Sd = Counts.MatrixMultNumber(Sd, (1.0 / (double)(Data[d][0].Length - 1)), n);

                    Sd = Counts.InverseMatrix(Sd);

                    buff = Counts.VectorMinusVector(xd, x);
                    buff = Counts.MatrixMultVector(Sd, buff);   // 2

                    temp = Counts.VectorMultVector(buff, Counts.VectorMinusVector(xd, x));
                    temp *= Data[d][0].Length;
                    V += temp;
                }

                check = false;
                if (V <= Quantils.XiSquare(0.95, n * (k - 1)))
                    check = true;
                return (V);
            }

            public static double CompareKDC(List<List<double[]>> Data, ref bool check)
            {
                double V = 0;

                int k = Data.Count;
                int n = Data[0].Count;

                double[] xd;
                double[] Xld;
                double[,] Sd;


                double[] buff;
                double[,] MBuff;

                double[,] S = new double[n, n];
                double N = 0;

                for (int d = 0; d < k; d++)
                {
                    xd = new double[n];
                    Sd = new double[n, n];

                    for (int i = 0; i < n; i++)
                        xd[i] = Data[d][i].Average();
                    for (int i = 0; i < Data[d][0].Length; i++)
                    {
                        Xld = new double[n];
                        for (int j = 0; j < n; j++)
                        {
                            Xld[j] = Data[d][j][i];
                        }
                        buff = Counts.VectorMinusVector(Xld, xd);
                        MBuff = Counts.VV_Matrix(buff, buff);
                        Sd = Counts.MatrixPlus(Sd, MBuff, n);
                    }

                    S = Counts.MatrixPlus(S, Sd, n);
                    N += Data[d][0].Length;
                }
                S = Counts.MatrixMultNumber(S, (1.0 / (N - k)), n);

                for (int d = 0; d < k; d++)
                {
                    xd = new double[n];
                    Sd = new double[n, n];

                    for (int i = 0; i < n; i++)
                        xd[i] = Data[d][i].Average();
                    for (int i = 0; i < Data[d][0].Length; i++)
                    {
                        Xld = new double[n];
                        for (int j = 0; j < n; j++)
                        {
                            Xld[j] = Data[d][j][i];
                        }
                        buff = Counts.VectorMinusVector(Xld, xd);
                        MBuff = Counts.VV_Matrix(buff, buff);
                        Sd = Counts.MatrixPlus(Sd, MBuff, n);
                    }
                    Sd = Counts.MatrixMultNumber(Sd, (1.0 / (double)(Data[d][0].Length - 1)), n);

                    V += Math.Log(Counts.DET(S, n) / Counts.DET(Sd, n)) * ((Data[d][0].Length - 1) / 2);
                }

                check = false;
                if (V <= Quantils.XiSquare(0.95, n * (k - 1)))
                    check = true;
                return (V);
            }
        }

        private class Correlations
        {
            public static double Part(int i, int j, int[] del)
            {
                if (del.Length == 1)
                    return (Rijd(i, j, del[0]));
                else if (del.Length > 1)
                {
                    int[] a = new int[del.Length - 1];
                    for (int q = 1; q < del.Length; q++)
                        a[q - 1] = del[q];
                    return ((Part(i, j, a) - Part(i, del[0], a) * Part(j, del[0], a)) / 
                        Math.Sqrt((1 - Math.Pow(Part(i, del[0], a), 2)) * (1 - Math.Pow(Part(j, del[0], a), 2))));
                }
                else
                    return R(i, j);
            }
            private static double Rijd(int i, int j, int d)
            {
                double ret = (R(i, j) - R(i, d) * R(j, d)) / Math.Sqrt((1 - Math.Pow(R(i, d), 2)) * (1 - Math.Pow(R(j, d), 2)));
                return (ret);
            }
            private static double R(int i, int j)
            {
                double ret = DataStorage.R[i][j];
                return (ret);
            }

            public static double Rxk(List<double[]> removed, ref bool check)
            {
                double ret = 0;
                int n = DataStorage.R.Count;
                int N = DataStorage.Data[0].Length;

                double[,] r = new double[DataStorage.R.Count, DataStorage.R.Count];
                double[,] rck = new double[removed.Count, removed.Count];

                for (int i = 0; i < DataStorage.R.Count; i++)
                    for (int j = 0; j < DataStorage.R[i].Count; j++)
                        r[i, j] = DataStorage.R[i][j];
                double DCdet = Counts.DET(r, n);


                for (int i = 0; i < removed.Count; i++)
                    for (int j = 0; j < removed.Count; j++)
                        rck[i, j] = Correlation.Pair(removed[i],
                            removed[j], removed[i].Average(), removed[j].Average());
                double DCKdet = Counts.DET(rck, removed.Count);

                ret = Math.Sqrt(Math.Abs(1 - DCdet / DCKdet));
                double f = ((N - n - 1) / n) * (Math.Pow(ret, 2) / (1 - Math.Pow(ret, 2)));
                check = false;
                if (f > Quantils.Fisher(0.95, n, N - n - 1))
                    check = true;
                return (ret);

            }
        }

        private class RegressionK
        {
            public static double MNK(bool mult, ref double[] a, ref bool[] a_check)
            {
                double S2 = 0;
                
                if (mult)
                {

                }
                else
                {
                    double[,] XT = new double[DataStorage.Data.Count, DataStorage.Data[0].Length];
                    for (int i = 0; i < DataStorage.Data.Count; i++)
                        for (int j = 0; j < DataStorage.Data[0].Length; j++)
                            XT[i, j] = DataStorage.Data[i][j];

                    double[,] X = Counts.TranspMatrix(XT);
                    double[] Y = new double[DataStorage.Regression[0].Length];
                    for (int j = 0; j < DataStorage.Regression[0].Length; j++)
                        Y[j] = DataStorage.Regression[0][j];

                    double[] A = MVT(Counts.InverseMatrix(MM(XT, X)), MVT(XT, Y));
                    a = new double[A.Length];
                    for (int i = 0; i < A.Length; i++)
                        a[i] = A[i];

                    double[] Temp = MVT(X, A);
                    for (int i = 0; i < Temp.Length; i++)
                        Temp[i] = Y[i] - Temp[i];
                    S2 = Counts.VectorMultVector(Temp, Temp);
                    S2 = S2 / (DataStorage.Data[0].Length - DataStorage.Data.Count);

                    a_check = new bool[a.Length];
                    double[,] a_temp = Counts.MatrixMultMatrix(XT, X, a.Length);
                    double t = 0;
                    for (int i = 0; i < a.Length; i++)
                    {
                        t = (a[i] - DataStorage.A[i]) / (Math.Sqrt(S2) * Math.Sqrt(a_temp[i, i]));
                        if (t <= Quantils.Student(DataStorage.Data[0].Length - DataStorage.Data.Count))
                            a_check[i] = true;
                    }
                }
                return (S2);
            }

            public static double R2(List<double[]> Data, List<List<double>> Rl, ref bool check)
            {
                double R2 = 0;

                double[,] R = new double[Rl.Count, Rl.Count];

                for (int i = 0; i < Rl.Count; i++)
                    for (int j = 0; j < Rl.Count; j++)
                        R[i, j] = Rl[i][j];
                double Dr = Counts.DET(R, Rl.Count);

                List<double[]> Temp = new List<double[]>();
                for (int i = 0; i < Data.Count; i++)
                    Temp.Add(Data[i]);
                Temp.Add(DataStorage.Regression[0]);

                double[,] Ry = new double[Temp.Count, Temp.Count];
                for (int i = 0; i < Temp.Count; i++)
                    for (int j = 0; j < Temp.Count; j++)
                        Ry[i, j] = (Correlation.Pair(Temp[i], Temp[j], Temp[i].Average(), Temp[j].Average()));
                double Dry = Counts.DET(Ry, Temp.Count);

                R2 = Math.Sqrt(Math.Abs(1 - Dry / Dr));
                double f = ((double)(Data[0].Length - Data.Count - 1) / Data.Count) * (1 / (1 - R2) - 1);
                check = false;
                if (f >= Quantils.Fisher(0.95, Data.Count, Data[0].Length - Data.Count - 1))
                    check = true;
                return (R2);
            }

            private static double[,] MM(double[,] a, double[,] b)
            {
                int n = a.GetLength(0);
                int N = b.GetLength(1);
                double[,] c = new double[n, N];

                for (int p = 0; p < n; p++)
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < a.GetLength(1); j++)
                        {
                            c[p, i] += a[p, j] * b[j, i];
                        }
                    }
                return (c);
            }
            private static double[] MVT(double[,] a, double[] b)
            {
                int n = a.GetLength(0);
                int N = a.GetLength(1);
                double[] c = new double[n];
                
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        c[i] += a[i, j] * b[j];
                    }
                }
                return (c);
            }
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            ShowCompares(4);
            ShowData(4);
            try
            {
                string[] a = textBox2.Text.Split(';');
                if (a.Length > 0)
                {
                    int[] arr = new int[a.Length];
                    for (int q = 0; q < a.Length; q++)
                        arr[q] = Convert.ToInt32(a[q]) - 1;

                    bool check = false;
                    List<double[]> temp = new List<double[]>();
                    for (int i = 0; i < DataStorage.Data.Count; i++)
                    {
                        check = false;
                        for (int j = 0; j < arr.Length; j++)
                            if (i == arr[j])
                                check = true;
                        if (!check)
                            temp.Add(DataStorage.Data[i]);
                    }
                    check = false;
                    double r = Correlations.Rxk(temp, ref check);

                    radioButton1.Checked = true;

                    dataGridView3.Rows.Add();
                    dataGridView3[0, dataGridView3.Rows.Count - 1].Value = "Множинна кореляція " + " {" + textBox2.Text + "}";
                    dataGridView3[1, dataGridView3.Rows.Count - 1].Value = Math.Round(r, 4);
                    dataGridView3[2, dataGridView3.Rows.Count - 1].Value = check == true ? "true" : "false";
                }
            }
            catch(Exception ex)
            {
                ex = null;
            }
            try
            {
                int i = (int)numericUpDown1.Value - 1;
                int j = (int)numericUpDown2.Value - 1;
                string[] a = textBox1.Text.Split(';');
                int[] arr = new int[a.Length];
                for (int q = 0; q < a.Length; q++)
                    arr[q] = Convert.ToInt32(a[q]) - 1;
                double r = Correlations.Part(i, j, arr);
                double t = r * Math.Sqrt(DataStorage.Data[0].Length - a.Length - 2) / Math.Sqrt(1 - r * r);
                
                radioButton1.Checked = true;

                dataGridView3.Rows.Add();
                dataGridView3[0, dataGridView3.Rows.Count - 1].Value = "Часткова кореляція " + i.ToString() + "," + j.ToString() + " {" + textBox1.Text + "}";
                dataGridView3[1, dataGridView3.Rows.Count - 1].Value = Math.Round(r, 4);
                dataGridView3[2, dataGridView3.Rows.Count - 1].Value = Math.Abs(t) >= Quantils.Student(DataStorage.Data[0].Length - a.Length - 2) ? "true" : "false";
            }
            catch(Exception ex)
            {
                ex = null;
            }
        } // correlation count


        private void button5_Click(object sender, EventArgs e)
        {
            int temp = 0;
            if (radioButton6.Checked == true)
                temp = 1;
            if (radioButton5.Checked == true)
                temp = 2;

            this.Hide();
            KRegressionGenerate window = new KRegressionGenerate(this, temp);
            window.Show();
            window.FormClosed += new FormClosedEventHandler(Opened_Form_Closed);
        }
        void Opened_Form_Closed(object sender, FormClosedEventArgs e)
        {
            this.Show();

            button6_Click(sender, e);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (DataStorage.A != null && DataStorage.A.Count > 0)
            {
                double[] A = new double[0];
                bool check = false;
                bool[] a_check = new bool[0];
                double S2 = RegressionK.MNK(DataStorage.multregr, ref A, ref a_check);
                double R2 = RegressionK.R2(DataStorage.Data, DataStorage.R, ref check);

                button4_Click(sender, e);

                dataGridView3.Rows.Add();
                dataGridView3[0, dataGridView3.Rows.Count - 1].Value = "S2 залишкове";
                dataGridView3[1, dataGridView3.Rows.Count - 1].Value = Math.Round(S2, 4);

                dataGridView3.Rows.Add();
                dataGridView3[0, dataGridView3.Rows.Count - 1].Value = "Коефіцієнти (а)";
                string n = "";
                for (int i = 0; i < A.Length; i++)
                {
                    n += Math.Round(A[i], 4).ToString();
                    if (i != A.Length - 1)
                        n += "; ";
                }
                dataGridView3[1, dataGridView3.Rows.Count - 1].Value = "{ " + n + " }";
                n = "";
                for (int i = 0; i < A.Length; i++)
                {
                    n += a_check[i].ToString();
                    if (i != A.Length - 1)
                        n += "; ";
                }
                dataGridView3[2, dataGridView3.Rows.Count - 1].Value = "{ " + n + " }";

                dataGridView3.Rows.Add();
                dataGridView3[0, dataGridView3.Rows.Count - 1].Value = "R2 коеф. детермінації";
                dataGridView3[1, dataGridView3.Rows.Count - 1].Value = Math.Round(R2, 4);
                dataGridView3[2, dataGridView3.Rows.Count - 1].Value = check.ToString();
            } // regression count
        }
    }

    static class DataStorage
    {
        public static bool                  N_size;

        public static int                   index;
        public static int                   step;

        public static List<double[]>        Data;

        public static List<List<double>>    Params;
        public static List<List<double>>    DC;
        public static List<List<double>>    R;
        public static List<string>          ParamNames;

        public static List<List<double[]>>          N_Data;
        public static List<List<double>>            N_Compares;
        public static List<string>                  N_Names;

        
        public static List<double[]>        Regression;
        public static bool                  multregr;
        public static List<double>          A;
    }
}
