using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_1_Graph
{
    public partial class KRegressionGenerate : Form
    {
        public KRegressionGenerate()
        {
            InitializeComponent();
        }

        private K_DForm frm;
        int rg;

        public KRegressionGenerate(K_DForm frm, int am)
        {
            InitializeComponent();
            this.frm = frm;
            rg = am;
            DataStorage.A = new List<double>();
            dataGridView1.ColumnCount = 1;
            for (int i = 0; i < DataStorage.Data.Count; i++)
                dataGridView1.Rows.Add();
            if (am == 2)
            {
                dataGridView1.Rows.Add();
                dataGridView1[0, 0].Style.BackColor = Color.LightGreen;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataStorage.Regression = new List<double[]>();

            double eps = 0;
            try
            {
                eps = Convert.ToDouble(textBox1.Text);
            }
            catch (Exception ex){}

            double[] a = new double[DataStorage.Data.Count];
            for (int i = 0; i < DataStorage.Data.Count; i++)
            {
                DataStorage.A.Add(a[i] = Convert.ToDouble(dataGridView1[0, i].Value));
            }

            double[] temp = new double[DataStorage.Data[0].Length];
            if (rg == 1)
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    for (int j = 0; j < DataStorage.Data.Count; j++)
                        temp[i] += a[j] * DataStorage.Data[j][i] + TestSimpleRNG.SimpleRNG.GetNormal(0, Math.Max(eps, 0.001));
                }
                DataStorage.multregr = false;
            }
            else if (rg == 2)
            {
                a = new double[DataStorage.Data.Count + 1];
                for (int i = 0; i < DataStorage.Data.Count + 1; i++)
                    a[i] = Convert.ToDouble(dataGridView1[0, i].Value);

                for (int i = 0; i < temp.Length; i++)
                {
                    for (int j = 0; j < DataStorage.Data.Count; j++)
                        temp[i] += a[j + 1] * DataStorage.Data[j][i] + TestSimpleRNG.SimpleRNG.GetNormal(0, Math.Max(eps, 0.001));
                    temp[i] += a[0];
                }
                DataStorage.multregr = true;
            }
            DataStorage.Regression.Add(temp);

            this.Close();
            }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
