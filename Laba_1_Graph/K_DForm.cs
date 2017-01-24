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
    public partial class K_DForm : Form
    {
        List<double[]> arr_all;
        List<double[]> undo = new List<double[]>();

        public K_DForm()
        {
            InitializeComponent();
        }

        public void GetValues(List<double[]> arr)
        {
            arr_all = new List<double[]>();

            foreach (double[] element in arr)
            {
                arr_all.Add(element);
            }

            undo = new List<double[]>();
            for (int i = 0; i < arr_all.Count; i++)
            {
                undo.Add(arr_all[i]);
            }
            ValuesOnTab();
        }

        private void ValuesOnTab()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Назва";
            dataGridView1.Columns[1].Name = "Ӫ";
            dataGridView1.Columns[2].Name = "Значення";
            dataGridView1.Columns[1].Width = 250;
            dataGridView1.Columns[0].Width = 250;

            double Val1 = 0;
            double Val2 = 0;
            string Bart = "";
            if (Criteria.Bartlett(arr_all, ref Val1))
            {
                string Av = "";
                if (Criteria.AverageK(arr_all, ref Val2))
                    Av = "збіжні";
                else
                    Av = "незбіжні";
                dataGridView1.Rows.Add("Середні: ", Av, Val2);

                Bart = "збіжні";
            }
            else
                Bart = "незбіжні";
            dataGridView1.Rows.Add("Дисперсії: ", Bart, Val1); // dispersion



            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add("Kритерії: ");

            if (Criteria.PreQcheck(arr_all))
            {
                string Q = "";
                if (Criteria.Q_criteria(arr_all, ref Val2))
                    Q = "однорідні";
                else
                    Q = "неоднорідні";
                dataGridView1.Rows.Add("Q-критерій: ", Q, Val2);
            }

            string H = "";
            if (Criteria.H_criteria(arr_all, ref Val2))
                H = "однорідні";
            else
                H = "неоднорідні";
            dataGridView1.Rows.Add("H-критерій: ", H, Val2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void вилученняАномальнихToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(double[] arr in arr_all)
                remove_abnormal(arr, arr.Min(), arr.Max());
            GetValues(arr_all);
        }

        private double[] remove_abnormal(double[] allnum, double MinX, double MaxX)
        {
            int N = allnum.Length; // pre-count used in auto-remove of abnormals
            double[] S_allnum = new double[0]; // cleared array of values
            double AverageX = Functions.Average(N, allnum); // Average X

            try
            {
                double
                    t = 1.55 + 0.8 * (Math.Sqrt(Math.Abs(Functions.Exces(N, AverageX, allnum))) * Math.Log10(N / 10)),
                    A = Math.Round(AverageX - t * Functions.Sigm(N, AverageX, allnum)),
                    B = Math.Round(AverageX + t * Functions.Sigm(N, AverageX, allnum));
                int k = 0;
                double[] Buf_allnum = new double[allnum.Length]; // temporary array

                for (int i = 0; i < allnum.Length; i++)
                {
                    if (allnum[i] < B && allnum[i] > A)
                    {
                        Buf_allnum[k] = allnum[i];
                        k++;
                    }
                }
                S_allnum = new double[k];
                for (int i = 0; i < k; i++)
                {
                    S_allnum[i] = Buf_allnum[i];
                }
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
            return (S_allnum);
        }

        private void стандартзаціяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (double[] arr in arr_all)
            {
                Counts.Sort(arr);

                double av_1 = Functions.Average(arr.Length, arr);

                double delt = Functions.Sigm(arr.Length, av_1, arr);
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = (arr[i] - av_1) / delt;
                }
            }
            GetValues(arr_all);
        }

        private void відновтиДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int i = 0; i <  arr_all.Count; i++)
            {
                arr_all[i] = new double[undo[i].Length];
                arr_all[i] = undo[i];
            }
            GetValues(arr_all);
        }
    }
}
