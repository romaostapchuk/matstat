using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Laba_1_Graph
{
    public partial class Form_Choose : Form
    {
        public static List<double[]> Samples = new List<double[]>();
        List<int> arr = new List<int>();
        bool _2_Graphs;
        bool _K_Graphs;

        public Form_Choose()
        {
            InitializeComponent();
            chart1.Visible = false;
            chart2.Visible = false;
            this.dataGridView1.ContextMenuStrip = contextMenuStrip1;
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)
               dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell != null)
            {
                chart1.Visible = false;
                chart2.Visible = false;
                arr.Add(cell.RowIndex);
                cell.Style.BackColor = Color.Red;
                _2_Graphs = false;
                _K_Graphs = false;
                if (arr.Count == 2)
                {
                    double s = 0;
                    chart2.Series.Clear();
                    chart1.Series.Clear();
                    if (Samples[arr[0]].Length == Samples[arr[1]].Length && checkBox1.Checked == true)
                    {
                        chart1.Visible = true;
                        chart2.Visible = chart1.Visible = true;
                        Graphs._2D_EmpericNorm(chart2, Samples[arr[0]], Samples[arr[1]]);
                        Graphs._2D_Histogram(chart1, Samples[arr[0]], Samples[arr[1]],
                        Counts.Step(s, Samples[arr[0]].Length, Samples[arr[0]].Min(), Samples[arr[0]].Max()),
                        Counts.Step(s, Samples[arr[1]].Length, Samples[arr[1]].Min(), Samples[arr[1]].Max()));

                        double Rxy = Laba_1_Graph.Correlation.Pair(Samples[arr[0]], Samples[arr[1]], Samples[arr[0]].Average(), Samples[arr[1]].Average());
                        //      Linear regression build
                        //{
                        //    if (Laba_1_Graph.Correlation.Check_Correlation(Rxy, Samples[arr[0]].Length))
                        //        if (Regression.Linear.Bartlet(Samples[arr[0]], Samples[arr[1]],
                        //        Counts.Step(s, Samples[arr[0]].Length, Samples[arr[0]].Min(), Samples[arr[0]].Max())))
                        //        {
                        //            double a = 0;
                        //            double b = 0;
                        //            double S2 = Regression.Linear.MNK(Samples[arr[0]], Samples[arr[1]], ref a, ref b);
                        //            Graphs._2D_RegressionLinear(chart2, Samples[arr[0]], Samples[arr[1]], a, b, Math.Sqrt(S2));
                        //        }
                        //}

                        //      Parabolic regression build
                        {
                            double[] abc = { 0, 0, 0 };
                            double[] a1b1c1 = { 0, 0, 0 };
                            double S2_P = Regression.Parabol.MNK1(Samples[arr[0]], Samples[arr[1]], Rxy, ref abc[0], ref abc[1], ref abc[2]);
                            double S2_P2 = Regression.Parabol.MNK2(Samples[arr[0]], Samples[arr[1]], Rxy, ref a1b1c1[0], ref a1b1c1[1], ref a1b1c1[2]);
                            Graphs._2D_RegressionParabol(chart2, Samples[arr[0]], Samples[arr[1]], abc, S2_P, a1b1c1, S2_P2);
                        }
                    }
                    _2_Graphs = true;
                    _K_Graphs = false;
                }
                else
                {
                    chart2.Series.Clear();
                    chart1.Series.Clear();
                    _2_Graphs = false;
                    if (arr.Count > 2)
                        _K_Graphs = true;
                }
            }
        }
        
        void Opened_Form_Closed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void згенеруватиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            ExpNorm window = new ExpNorm(this);
            window.Show();
            window.FormClosed += new FormClosedEventHandler(Opened_Form_Closed);
        }

        private void вибратиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 1;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            string filename;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                StreamReader streamReader = new StreamReader(fs, Encoding.ASCII);
                filename = openFileDialog1.SafeFileName;
                int i = -1;
                string[] s = null;
                try
                {
                    String str = streamReader.ReadToEnd();
                    str = Regex.Replace(str.Replace("\t", " ").Trim(' '), " +", " ");
                    str = str.Replace("\r", " ");
                    str = str.Replace(" \n", "\n");
                    str = str.Replace("\n ", "\n");
                    str = str.Replace(",", ".");
                    s = Regex.Split(str, "\n");
                    string[] strN = Regex.Split(s[0], " ");
                    List<double>[] A = new List<double>[strN.Length];
                    for (int k = 0; k < strN.Length; k++)
                        A[k] = new List<double>();
                    for (; i < s.Length - 1;)
                    {
                        i++;
                        strN = Regex.Split(s[i], " ");
                        for (int j = 0; j < strN.Length; j++)
                            A[j].Add(Math.Round(Convert.ToDouble(strN[j]), 2));
                    }
                    for (int j = 0; j < A.Length; j++)
                    {

                        double[] temparray = new double[A[j].Count];
                        int k = 0;
                        foreach (double element in A[j])
                        {
                            temparray[k] = element;
                            k++;
                        }

                        Samples.Add(temparray);
                        for (i = Convert.ToInt32(dataGridView1.RowCount.ToString()); i < Samples.Count + 1; i++)
                            dataGridView1.Rows.Add(filename.Replace(".txt", "_" + Samples[i - 1].Length + "_" + (j + 1)));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла!:\n" + ex.Message + "\n" + s[i]);
                }
                finally
                {
                    streamReader.Close();
                    fs.Close();
                }
            }
        }

        private void порівнятиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < arr.Count; i++)
                dataGridView1.Rows[arr[i]].Cells[0].Style.BackColor = Color.White;
            if (_2_Graphs)
            {
                this.Hide();
                _2Curs window = new _2Curs();
                window.GetValues(Samples[arr[0]], Samples[arr[1]], checkBox1.Checked);
                window.Show();
                window.FormClosed += new FormClosedEventHandler(Opened_Form_Closed);
                arr = new List<int>();
            }
            else if (_K_Graphs)
            {
                this.Hide();
                K_DForm window = new K_DForm();
                List<double[]> Samples_1 = new List<double[]>();
                foreach (int index in arr)
                {
                    Samples_1.Add(Samples[index]);
                }
                window.GetValues(Samples_1);
                window.Show();
                window.FormClosed += new FormClosedEventHandler(Opened_Form_Closed);
                arr = new List<int>();
            }
            _2_Graphs = false;
            chart2.Series.Clear();
            chart1.Series.Clear();
        }

        private void розглянутиОкремоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (arr.Count == 1)
            {
                for (int i = 0; i < arr.Count; i++)
                    dataGridView1.Rows[arr[i]].Cells[0].Style.BackColor = Color.White;
                this.Hide();
                Form1 window = new Form1();
                window.Compare_Show(Samples[arr[0]]);
                window.Show();
                window.FormClosed += new FormClosedEventHandler(Opened_Form_Closed);
                arr = new List<int>();
            }
        }

        private void очиститиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Visible = false;
            chart2.Visible = false;
            for (int i = 0; i < arr.Count; i++)
                dataGridView1.Rows[arr[i]].Cells[0].Style.BackColor = Color.White;
            arr = new List<int>();
            _2_Graphs = false;
            chart2.Series.Clear();
            chart1.Series.Clear();
        }

        private void видалитиДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Visible = false;
            chart2.Visible = false;
            for (int i = 0; i < arr.Count; i++)
                dataGridView1.Rows[arr[i]].Cells[0].Style.BackColor = Color.White;
            dataGridView1.Rows.Clear();
            Samples.Clear();
            Samples = new List<double[]>();
            arr = new List<int>();
            _2_Graphs = false;
            chart2.Series.Clear();
            chart1.Series.Clear();
            GC.Collect();
        }

        private void закритиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void неЛінійнаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegressionGenerate window = new RegressionGenerate(this);
            window.Show();
            window.FormClosed += new FormClosedEventHandler(Opened_Form_Closed);
        }
    }
}