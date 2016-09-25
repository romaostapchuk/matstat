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
using System.IO;

namespace Laba_1_Graph
{
    public partial class Form_Choose : Form
    {
        public Form_Choose()
        {
            InitializeComponent();
        }
        List<double[]> Samples = new List<double[]>();

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 1;
            string filename;

            

            List<double> timeList = new List<double>();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                StreamReader streamReader = new StreamReader(fs, Encoding.ASCII);
                String hstr = "";
                filename = openFileDialog1.SafeFileName;
                try
                {
                    String str = streamReader.ReadToEnd();
                    str = str.Replace("\t", " ");
                    str = str.Replace("   ", " ");
                    str = str.Replace("  ", " ");
                    str = str.Replace("\n", " ");
                    str = str.Replace(",", ".");
                    for (int i = 0; i < str.Length; i++)
                    {
                        hstr = "";
                        for (; i < str.Length && str[i] != ' ' && str[i] != '\t'; i++)
                        {
                            hstr += str[i];
                        }
                        if (hstr != "")
                            timeList.Add(Convert.ToDouble(hstr));
                    }
                    double[] temparray = new double[timeList.Count];
                    int k = 0;
                    foreach (double element in timeList)
                    {
                        temparray[k] = element;
                        k++;
                    }

                    Samples.Add(temparray);

                    dataGridView1.Rows.Clear();
                    for (int i =1; i < Samples.Count + 1; i ++) 
                        dataGridView1.Rows.Add(Convert.ToString(i) + "_" + filename.Replace(".txt",""));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + hstr);
                }
                finally
                {
                    streamReader.Close();
                    fs.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            Samples = new List<double[]>();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)
               dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell != null)
                MessageBox.Show(Convert.ToString(cell.Value));
        }
    }
}
