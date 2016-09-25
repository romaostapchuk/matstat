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
    
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.button1.DialogResult = DialogResult.OK;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public static double pwr()
        {
            Form2 window = new Form2();
            window.ShowDialog();
            if (window.DialogResult == DialogResult.OK && window.textBox1.TextLength > 0)
                return double.Parse(window.textBox1.Text);
            else
                return 1;
        }

    }
}
