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
    public partial class ExpNorm : Form
    {
        bool exp = false;
        bool norm = false;

        public static double[] Arr;
        private Form_Choose form1;

        public ExpNorm(Form_Choose form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "";
            if (radioButton1.Checked)
            {
                if (textBox1.TextLength > 0 && textBox4.TextLength > 0)
                {
                    exp = true;

                    int len = Convert.ToInt32(textBox1.Text);
                    double l = Convert.ToInt32(textBox4.Text);

                    Arr = new double[len];

                    for (int i = 0; i < len; i++)
                    {
                        Arr[i] = TestSimpleRNG.SimpleRNG.GetExponential(1 / l);
                    }
                }
                str = "G_exp";
            }
            else if (radioButton2.Checked)
            {
                if (textBox2.TextLength > 0 && textBox3.TextLength > 0 && textBox5.TextLength > 0)
                {
                    norm = true;

                    int len = Convert.ToInt32(textBox2.Text);
                    double sig = Convert.ToInt32(textBox3.Text);
                    double aver = Convert.ToInt32(textBox5.Text);

                    Arr = new double[len];

                    for (int i = 0; i < len; i++)
                    {
                        Arr[i] = TestSimpleRNG.SimpleRNG.GetNormal(aver, sig);
                    }
                }
                str = "G_norm";
            }
            if (norm || exp)
            {
                if (form1.dataGridView1.ColumnCount == 0)
                {
                    form1.dataGridView1.ColumnCount = 1;
                    form1.dataGridView1.RowHeadersVisible = false;
                    form1.dataGridView1.ColumnHeadersVisible = false;
                }
                form1.dataGridView1.Rows.Add(str + "_" + Arr.Length);
                Form_Choose.Samples.Add(Arr);

                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
