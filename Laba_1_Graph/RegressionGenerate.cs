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
    public partial class RegressionGenerate : Form
    {
        bool regr = false;
        bool norm = false;
        public static double[] Arr;
        private Form_Choose form1;

        public RegressionGenerate(Form_Choose form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "";
            int len = 0;
            if (textBox2.TextLength > 0 && textBox3.TextLength > 0 && textBox5.TextLength > 0)
            {
                norm = true;

                len = Convert.ToInt32(textBox2.Text);
                double sig = Convert.ToInt32(textBox3.Text);
                double aver = Convert.ToInt32(textBox5.Text);

                Arr = new double[len];

                for (int i = 0; i < len; i++)
                {
                    Arr[i] = TestSimpleRNG.SimpleRNG.GetNormal(aver, sig);
                }

                if (form1.dataGridView1.ColumnCount == 0)
                {
                    form1.dataGridView1.ColumnCount = 1;
                    form1.dataGridView1.RowHeadersVisible = false;
                    form1.dataGridView1.ColumnHeadersVisible = false;
                }

                str = "GR_norm";
                form1.dataGridView1.Rows.Add(str + "_" + Arr.Length);
                Form_Choose.Samples.Add(Arr);
            }

            if (textBox6.TextLength > 0 && textBox4.TextLength > 0 && textBox1.TextLength > 0 && textBox7.TextLength > 0 && norm)
            {
                regr = true;

                double a = Convert.ToDouble(textBox6.Text);
                double b = Convert.ToDouble(textBox4.Text);
                double c = Convert.ToDouble(textBox1.Text);
                int eps = Convert.ToInt32(textBox7.Text);

                double[] ArrR = new double[len];
                Random r = new Random();
                for (int i = 0; i < len; i++)
                {
                    ArrR[i] = a + b * Arr[i] + c * Arr[i] * Arr[i] + r.Next(-eps , eps);
                }


                if (form1.dataGridView1.ColumnCount == 0)
                {
                    form1.dataGridView1.ColumnCount = 1;
                    form1.dataGridView1.RowHeadersVisible = false;
                    form1.dataGridView1.ColumnHeadersVisible = false;
                }

                str = "GR_regr";
                form1.dataGridView1.Rows.Add(str + "_" + ArrR.Length);
                Form_Choose.Samples.Add(ArrR);
            }

            if (norm && regr)
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
