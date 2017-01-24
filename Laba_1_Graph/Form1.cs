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
    public partial class Form1 : Form
    {
        double step = 0;                       // step is being count in "Run" button
        double MinX = 0;                       // Minimum value of X
        double MaxX = 0;                       // Maximum value of X
        double[] allnum = new double[0];          // array of values
        double[] undoAllnum = new double[0];      // array 4 keeping values to return to them if asked
        int N = 0;                          //Length of array

        Point? prevPosition = null;         // position on a chart1
        ToolTip tooltip = new ToolTip();    // toolabar on a

        bool RunClicked = false;            // checks if "Run" button was clicked
        bool ReadFile = false;              // checks if File was chosen

        double Alpha = 0;                       // Extreme function
        double Bt = 0;

        bool normal = false, exponential = false, extreme = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        //fixed borders
        private void Chart2_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            var results = chart2.HitTest(pos.X, pos.Y, false, ChartElementType.PlottingArea);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.PlottingArea)
                {
                    var xVal = result.ChartArea.AxisX.PixelPositionToValue(pos.X);
                    var yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);

                    tooltip.Show("X=" + xVal, this.chart2, pos.X, pos.Y - 15);
                }
            }
        }
        // Shows in a toolbar value of X on a grahic

        public void Compare_Show(double[] arr)
        {
            allnum = new double[arr.Length];
            int k = 0;
            foreach (double element in arr)
            {
                allnum[k] = element;
                k++;
            }

            this.chart2.MouseMove += Chart2_MouseMove;
            RunClicked = true;
            N = 0;
            Counts.Sort(allnum);
            undoAllnum = new double[allnum.Length];
            for (int i = 0; i < allnum.Length; i++)
            {
                undoAllnum[i] = allnum[i];
            }

            N = allnum.Length;
            MinX = allnum[0];
            MaxX = allnum[N - 1];
            double AverageX = Functions.Average(N, allnum); // Average X
            double s = 0;
            if (textBox4.TextLength > 0)
                s = Convert.ToInt32(textBox4.Text);
            step = Counts.Step(s, N, MinX, MaxX);// Counting step


            dataGridView1.Rows.Clear();
            Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, false, false);
            ReadFile = false;
        }

        private void Run_Click(object sender, EventArgs e)
        {
            this.chart2.MouseMove += Chart2_MouseMove;// adding after start

            RunClicked = true;
            N = 0;
            if (ReadFile == false)
            {
                if (radioButton1.Checked)
                {
                    normal = true;
                    exponential = false;
                    extreme = false;
                }
                else if (radioButton2.Checked)
                {
                    normal = false;
                    exponential = true;
                    extreme = false;
                }
                else if (radioButton3.Checked)
                {
                    normal = false;
                    exponential = false;
                    extreme = true;
                }


                if (textBox3.TextLength > 0)
                {
                    allnum = new double[Convert.ToInt32(textBox3.Text)];
                }
                else
                {
                    allnum = new double[300];
                }
                N = allnum.Length;
                Counts.Init(textBox1, textBox2, textBox5, textBox6, textBox7, N, allnum, normal, exponential, extreme, ref Alpha, ref Bt);//Initialising allnum with random 
            }

            Counts.Sort(allnum);
            undoAllnum = new double[allnum.Length];
            for(int i = 0; i < allnum.Length; i++)
            {
                undoAllnum[i] = allnum[i];
            }

            N = allnum.Length;
            MinX = allnum[0];
            MaxX = allnum[N - 1];
            double AverageX = Functions.Average(N, allnum); // Average X
            double s = 0;
            if (textBox4.TextLength > 0)
                s = Convert.ToInt32(textBox4.Text);
            step = Counts.Step(s, N, MinX, MaxX);// Counting step


            dataGridView1.Rows.Clear();
            Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, false, false);
            ReadFile = false;
        }
        // Main function "Run" button, builds all function(USE FIRST)
        

        public static void Programm(Chart chart1, Chart chart2, int N, double MinX, double MaxX,
            double[] allnum, double step, DataGridView dataGridView1, double AverageX,
            bool exponential, bool normal)
        {
            List<double> Xi_Vl = new List<double>();
            List<double> Xi_Bd = new List<double>();

            List<double> K_Vl = new List<double>();
            List<double> K_Psb = new List<double>();

            chart1.Series.Clear();
            chart2.Series.Clear();



            Graphs.EmpericNorm(chart2, N, MinX, MaxX, ref allnum, step);
            Graphs.EmpericClass(chart2, N, MinX, MaxX, ref allnum, step, Xi_Bd);
            Graphs.Histogram(chart1, N, MinX, MaxX, ref allnum, step, Xi_Vl);


            dataGridView1.ColumnCount = 7;
            dataGridView1.Columns[0].Width = 200;
            dataGridView1.Columns[0].Name = "Назва";
            dataGridView1.Columns[1].Name = "Θн";
            dataGridView1.Columns[2].Name = "Ӫ";
            dataGridView1.Columns[3].Name = "Θв";
            dataGridView1.Columns[4].Name = "σ{Θ}";
            dataGridView1.Columns[5].Name = "Значення T-тесту";
            dataGridView1.Columns[6].Name = "Результат тесту";

            /*
           Point values on ValuesTab
           */
            double med = Functions.Med(N, allnum);
            double MED_U = Functions.MED_Uol(N, allnum);
            double mod = Functions.Mod(N, MinX, MaxX, allnum);
            double mad = Functions.Mad(N, med, allnum);
            double S = Functions.Disp(N, AverageX, allnum);
            double delt = Functions.Sigm(N, AverageX, allnum);
            double A = Functions.Asim(N, AverageX, allnum);
            double E = Functions.Exces(N, AverageX, allnum);
            double Xi = Math.Round(1/(Math.Sqrt(Math.Abs(Functions.Exces(N, AverageX, allnum)))), 4);
            double Wn = Functions.NRV(mad, med);
            double Hi = Math.Round(delt / AverageX, 4);
            double CutAvrg = Functions.CutAverage(N, allnum);
            double
                q_05 = Functions.Qantil(allnum, 0.05),
                q_25 = Functions.Qantil(allnum, 0.25),
                q_50 = Functions.Qantil(allnum, 0.5),
                q_5 = Functions.Qantil(allnum, 0.75),
                q_95 = Functions.Qantil(allnum, 0.95);


            /*
            Interval values on ValuesTab
            */
            double Avrg_Bottom = Math.Round(Interval.AverageB(N, delt, AverageX), 4);
            double Avrg_Upper = Math.Round(Interval.AverageU(N, delt, AverageX), 4);
            double S_Bottom = Math.Round(Interval.SigmB(N, delt), 4);
            double S_Upper = Math.Round(Interval.SigmU(N, delt), 4);
            double A_Bottom = Math.Round(Interval.AsimB(N, A, allnum), 4);
            double A_Upper = Math.Round(Interval.AsimU(N, A, allnum), 4);
            double E_Bottom = Math.Round(Interval.ExcesB(N, E, allnum), 4);
            double E_Upper = Math.Round(Interval.ExcesU(N, E, allnum), 4);
            double Wn_Bottom = Math.Round(Interval.NRV_B(N, Wn), 4);
            double Wn_Upper = Math.Round(Interval.NRV_U(N, Wn), 4);

            /*
            Sigm values of interval values
            */
            double StdAvrg = Interval.SigmaAverage(N, delt);
            double StdSigm = Interval.SigmaDisp(N, delt);
            double StdAsim = Interval.SigmaAsim(N, A, allnum);
            double StdExcess = Interval.SigmaExces(N, E, allnum);
            double StdWn = Interval.SigmaNRV(N, Wn);


            dataGridView1.Rows.Add("Кількість ", "", N);
            dataGridView1.Rows.Add("Мінімальне зн.", "", Math.Round(MinX,4));
            dataGridView1.Rows.Add("Максимальне зн.", "", Math.Round(MaxX,4));
            dataGridView1.Rows.Add("Середнє зн.",Avrg_Bottom, Math.Round(AverageX , 4), Avrg_Upper, StdAvrg);
            dataGridView1.Rows.Add("Усічене середнє ", "", CutAvrg);
            dataGridView1.Rows.Add("Медіана Уолша", "", MED_U);
            dataGridView1.Rows.Add("Медіана", "", med);
            dataGridView1.Rows.Add("Медіана абсолютних значень", "", mad);
            dataGridView1.Rows.Add("Мода", "", mod);
            dataGridView1.Rows.Add("0,05 Квантіль", "",q_05);
            dataGridView1.Rows.Add("0,25 Квантіль", "", q_25);
            dataGridView1.Rows.Add("0,5 Квантіль", "", q_50);
            dataGridView1.Rows.Add("0,75 Квантіль", "", q_5);
            dataGridView1.Rows.Add("0,95 Квантіль", "", q_95);
            dataGridView1.Rows.Add("Коефіцієнт варіації пірсона", "", Hi);
            dataGridView1.Rows.Add("Дисперсія", "", S);
            dataGridView1.Rows.Add("Коеф. контрексцесу", "", Xi);
            dataGridView1.Rows.Add("Непараметричний коефіцієнт варіації", Wn_Bottom, Wn, Wn_Upper, StdWn);
            dataGridView1.Rows.Add("Середньоквадратичне відхилення", S_Bottom, delt , S_Upper, StdSigm);
            dataGridView1.Rows.Add("Коеф. асиметрії",A_Bottom, A, A_Upper, StdAsim);
            dataGridView1.Rows.Add("Коеф. ексцесу", E_Bottom, E, E_Upper, StdExcess);
            
            if (exponential || normal)
            {
                double Xi2 = Criteria.Pirson(N, Xi_Vl, Xi_Bd, AverageX, allnum, exponential, normal);
                MessageBox.Show("Хі квадрат Пірсона = " + Convert.ToString(Xi2));
            }
        }
        // Main part of the programm used in "Run" & "Remove" buttons
        
        private void логарифмуванняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RunClicked)
            {
                Counts.Sort(allnum);
                N = allnum.Length;
                if (MinX <= 0)
                {
                    for (int i = 0; i < N; i++)
                    {
                        allnum[i] = allnum[i] + Math.Abs(MinX) + 1;
                    }
                }
                for (int i = 0; i < N; i++)
                {
                    allnum[i] = (Math.Log(allnum[i]));
                }
                Counts.Sort(allnum);
                MinX = allnum[0];
                MaxX = allnum[N - 1];
                double AverageX = Functions.Average(N, allnum); // Average X
                double s = 0;
                if (textBox4.TextLength > 0)
                    s = Convert.ToInt32(textBox4.Text);
                step = Counts.Step(s, N, MinX, MaxX);// Counting step

                dataGridView1.Rows.Clear();
                Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, false, false);
            }
        }
        //logarifmizes the function
        
        private void піднесенняДоСтепеніToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Введіть значення степені");
            double pwr = Form2.pwr();

            if (RunClicked)
            {
                for (int i = 0; i < N; i++)
                {
                    if(allnum[i]>=0)
                        allnum[i] = Math.Pow(allnum[i], pwr);
                    else
                        allnum[i] = -Math.Pow(-allnum[i], pwr);

                }

                Counts.Sort(allnum);
                N = allnum.Length;
                MinX = allnum[0];
                MaxX = allnum[N - 1];
                double AverageX = Functions.Average(N, allnum); // Average X
                double s = 0;
                if (textBox4.TextLength > 0)
                    s = Convert.ToInt32(textBox4.Text);
                step = Counts.Step(s, N, MinX, MaxX);// Counting step

                dataGridView1.Rows.Clear();
                Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, false, false);
            }
        }
        //powerizes all the vakues

        private void автоматичнеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RunClicked)
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
                    allnum = S_allnum;
                    N = allnum.Length;

                    Counts.Sort(allnum);
                    MinX = allnum[0];
                    MaxX = allnum[N - 1];
                    AverageX = Functions.Average(N, allnum); // Average X
                    double s = 0;
                    if (textBox4.TextLength > 0)
                        s = Convert.ToInt32(textBox4.Text);
                    step = Counts.Step(s, N, MinX, MaxX);// Counting step

                    dataGridView1.Rows.Clear();
                    Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, false, false);
                }
                catch (Exception exeption)
                {
                    MessageBox.Show(exeption.Message);
                }
            }
        }
        //auto remove of abnormal values

        private void ручнеToolStripMenuItem_Click(object sender, EventArgs e)//manual remove of abnormal values
        {
            if (RunClicked)
            {
                int N = allnum.Length; // pre-count used in auto-remove of abnormals

                double AverageX = Functions.Average(N, allnum); // Average X

                double[] S_allnum = new double[0];

                double[] Buf_allnum = new double[allnum.Length]; // temporary array
                double a = MinX;
                double b = MaxX;
                int k = 0;
                MessageBox.Show("Введіть значення для нижньої та верхньої границі");
                a = Form2.pwr();
                b = Form2.pwr();

                for (int i = 0; i < allnum.Length; i++)
                {
                    if (allnum[i] <= b && allnum[i] >= a)
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
                allnum = new double[S_allnum.Length];
                for (int i = 0; i < k; i++)
                {
                    allnum[i] = S_allnum[i];
                }
                N = allnum.Length;

                Counts.Sort(allnum);
                MinX = allnum[0];
                MaxX = allnum[N - 1];
                AverageX = Functions.Average(N, allnum); // Average X

                double s = 0;
                if (textBox4.TextLength > 0)
                    s = Convert.ToInt32(textBox4.Text);
                if (Counts.Step(s, N, MinX, MaxX) > 0)// Counting step
                    step = Counts.Step(s, N, MinX, MaxX);// Counting step
                else step = 1;


                dataGridView1.Rows.Clear();
                Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, false, false);
            }
        }

        //Xi sqaure of Pirson
        private void нормальнийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Counts.Sort(allnum);
            undoAllnum = new double[allnum.Length];
            for (int i = 0; i < allnum.Length; i++)
            {
                undoAllnum[i] = allnum[i];
            }

            N = allnum.Length;
            MinX = allnum[0];
            MaxX = allnum[N - 1];
            double AverageX = Functions.Average(N, allnum); // Average X

            double s = 0;
            if (textBox4.TextLength > 0)
                s = Convert.ToInt32(textBox4.Text);
            step = Counts.Step(s, N, MinX, MaxX);// Counting step

            dataGridView1.Rows.Clear();
            Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, false, true);
        }
        private void експоненціальнийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Counts.Sort(allnum);
            undoAllnum = new double[allnum.Length];
            for (int i = 0; i < allnum.Length; i++)
            {
                undoAllnum[i] = allnum[i];
            }

            N = allnum.Length;
            MinX = allnum[0];
            MaxX = allnum[N - 1];
            double AverageX = Functions.Average(N, allnum); // Average X
            double s = 0;
            if (textBox4.TextLength > 0)
                s = Convert.ToInt32(textBox4.Text);
            step = Counts.Step(s, N, MinX, MaxX);// Counting step


            dataGridView1.Rows.Clear();
            Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, true, false);
        }


        //Kolmahorov
        private void нормальнийToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            N = allnum.Length;
            double AverageX = Functions.Average(N, allnum);

            double Kolmah = Criteria.Kolmahorov(N, allnum, AverageX, true, false , false, 0, 0);
            MessageBox.Show("Критерій колмагорова = " + Convert.ToString(Kolmah));
        }
        private void експоненціальнийToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            N = allnum.Length;
            double AverageX = Functions.Average(N, allnum); // Average X
           
            double Kolmah = Criteria.Kolmahorov(N, allnum, AverageX, false, true, false, 0, 0);
            MessageBox.Show("Критерій колмагорова = " + Convert.ToString(Kolmah));
        }
        private void колмагороваToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            N = allnum.Length;
            double AverageX = Functions.Average(N, allnum); // Average X
           
            double KolmahExp = Criteria.Kolmahorov(N, allnum, AverageX, false, true, false, 0,0);
            double KolmahNorm = Criteria.Kolmahorov(N, allnum, AverageX, true, false, false, 0, 0);
            double KolmahExtr = Criteria.Kolmahorov(N, allnum, AverageX, false, false, true, Alpha, Bt);
            if (KolmahExp > 0.1 || KolmahNorm > 0.1 || KolmahExtr > 0.1)
            {
                if (KolmahExp > KolmahNorm && KolmahExp > KolmahExtr)
                {
                    MessageBox.Show("Розподіл найбільш вірогідно є ексопоненцальним \nІмовірність узгодження = " + Convert.ToString(KolmahExp));
                }
                else if (KolmahNorm > KolmahExp && KolmahNorm > KolmahExtr)
                {
                    MessageBox.Show("Розподіл найбільш вірогідно є нормальним \nІмовірність узгодження = " + Convert.ToString(KolmahNorm));
                }
                else if (KolmahExtr > KolmahExp && KolmahExtr > KolmahNorm)
                {
                    MessageBox.Show("Розподіл найбільш вірогідно є екстремальним \nІмовірність узгодження = " + Convert.ToString(KolmahExtr));
                }

            }
            else
                MessageBox.Show("Не можливо визначити вид розподілу!");
        }
        private void екстремальнийToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            N = allnum.Length;
            double AverageX = Functions.Average(N, allnum); // Average X

            double Kolmah = Criteria.Kolmahorov(N, allnum, AverageX, false, false, true, Alpha, Bt);
            MessageBox.Show("Критерій колмагорова = " + Convert.ToString(Kolmah));
        }

        private void відкритиФайлToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReadFile = true;
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
                    allnum = new double[timeList.Count];
                    int k = 0;
                    foreach (double element in timeList)
                    {
                        allnum[k] = element;
                        k++;
                    }
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

                this.chart2.MouseMove += Chart2_MouseMove;
                RunClicked = true;
                N = 0;
                Counts.Sort(allnum);
                undoAllnum = new double[allnum.Length];
                for (int i = 0; i < allnum.Length; i++)
                {
                    undoAllnum[i] = allnum[i];
                }

                N = allnum.Length;
                MinX = allnum[0];
                MaxX = allnum[N - 1];
                double AverageX = Functions.Average(N, allnum); // Average X
                double s = 0;
                if (textBox4.TextLength > 0)
                    s = Convert.ToInt32(textBox4.Text);
                step = Counts.Step(s, N, MinX, MaxX);// Counting step


                dataGridView1.Rows.Clear();
                Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, false, false);
                ReadFile = false;
            }
        }
        //opens files

        private void відновтиДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RunClicked)
            {
                allnum = new double[undoAllnum.Length];
                for (int i = 0; i < undoAllnum.Length; i++)
                {
                    allnum[i] = undoAllnum[i];
                }

                N = allnum.Length;
                Counts.Sort(allnum);

                MinX = allnum[0];
                MaxX = allnum[N - 1];
                double AverageX = Functions.Average(N, allnum); // Average X

                double s = 0;
                if (textBox4.TextLength > 0)
                    s = Convert.ToInt32(textBox4.Text);
                step = Counts.Step(s, N, MinX, MaxX);// Counting step

                dataGridView1.Rows.Clear();
                Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, false, false);
                ReadFile = false;
            }
        }

        //Т-тест
        private void tтестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double Q0 = 0, Q = 0, Sigm = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[2].Value != null &&
                    dataGridView1.Rows[i].Cells[4].Value != null &&
                    dataGridView1.Rows[i].Cells[5].Value != null)
                {
                    Q0 = Convert.ToDouble(dataGridView1.Rows[i].Cells[5].Value);
                    Q = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                    Sigm = Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value);
                    if (T_test(Q0, Q, Sigm, N))
                        dataGridView1.Rows[i].Cells[6].Value = "True";
                    else
                        dataGridView1.Rows[i].Cells[6].Value = "False";
                }
            }
        }
        private static bool T_test(double Q0, double Q, double sigm, int amount)
        {
            double T = ((Q0 - Q) / sigm);
            if (Math.Abs(T) < Quantils.Student(amount))
                return true;
            else
                return false;
        }
        
        //Відновлення функції розподілу
        private double NormDisp(double S, double m, double x)
        {
            double
               Dm = Math.Pow(S, 2) / N,
               Ds = Math.Pow(S, 2) / (2 * N);
            double
                Fm = 0,
                Fs = 0;
            double D = 0;

            Fm = ((-1) / (S * Math.Sqrt(2 * Math.PI))) * Math.Exp(-1 * (Math.Pow(x - m, 2) / (2 * Math.Pow(S, 2))));
            Fs = (( m-x) / (Math.Pow(S, 2) * Math.Sqrt(2 * Math.PI))) * Math.Exp(-1 * (Math.Pow(x - m, 2) / (2 * Math.Pow(S, 2))));

            D = (Math.Pow(Fm, 2) * Dm + Math.Pow(Fs, 2) * Ds );
            return D;
        }
        private double ExtremeDisp(double Alpha, double Bt, double x,int index, int amount, double[] numb)
        {
            double B = 1/Alpha;

            double Szal = 0;
            for (int i = 0; i < amount-1; i++)
            {
                double Zl = Math.Log(Math.Log(1 + Math.Log(1.0 / (1 - ((double)(i) / amount)))));
                if (i == 0 || i == amount )
                    Zl= 0;
                Szal += Math.Pow(Zl -Math.Log(B)-Bt*Math.Log(numb[i]), 2);
            }
            Szal /= (amount - 3);
                double a11 = amount - 1;
                double
                    a12 = 0,
                    a21 = 0,
                    a22 = 0;

                for(int i = 0; i < amount - 1; i++)
                {
                    a12 += Math.Log(numb[i]);
                    a22 += Math.Pow(Math.Log(numb[i]), 2);
                }
                a21 = a12;
            double DA = (a22 * Szal) / (a11 * a22 - a12 * a21);
            double DB = Math.Pow(B, 2) * DA;
            double DBt = (a11 * Szal) / (a11 * a22 - a12 * a21);

            double cov_A_B = -(a12 * Szal) / (a11 * a22 - a12 * a21);
            double cov_B_Bt = B * cov_A_B;

            double FB = Math.Pow(x, Bt) * Math.Exp(-(Math.Exp(B * Math.Pow(x, Bt)) - 1)) * Math.Exp(B * Math.Pow(x, Bt));
            double FBt = FB * B * Math.Log(x);

            double disp = Math.Pow(FB, 2) * DB + Math.Pow(FBt, 2) * DBt + 2 * FB * FBt * cov_B_Bt;
            return disp;
        }
        private void нормальнийToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            N = allnum.Length;
            MinX = allnum[0];
            MaxX = allnum[N - 1];
            double AverageX = Functions.Average(N, allnum); // Average X

            double m = AverageX;
            double S = 0;

            double Sum_of_X2 = 0;
            for (int i = 0; i < N; i++)
            {
                Sum_of_X2 += Math.Pow(allnum[i], 2);
            }
            Sum_of_X2 /= N;
            S = (N / (N - 1)) * Math.Sqrt(Sum_of_X2 - Math.Pow(m, 2));

            double s = 0;
            if (textBox4.TextLength > 0)
                s = Convert.ToInt32(textBox4.Text);
            step = Counts.Step(s, N, MinX, MaxX);// Counting step


            dataGridView1.Rows.Clear();
            Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, m, false, false);
            ReadFile = false;

            chart2.Series.Add("Norm"); //   F(x,Q)
            chart2.Series["Norm"].Color = Color.Green;
            chart2.Series["Norm"].ChartType = SeriesChartType.Line;

            chart1.Series.Add("Normf");//   f(x,Q)
            chart1.Series["Normf"].Color = Color.Red;
            chart1.Series["Normf"].ChartType = SeriesChartType.Line;
            double f = 0;
            for (double i = MinX - step; i < MaxX + 2 * step; i += 0.01 * (MaxX - MinX))
            {
                f = (1 / (Math.Sqrt(2 * Math.PI))) * (Math.Exp(-1 * Math.Pow(i - m , 2) / (2 * Math.Pow(S, 2))));
                chart1.Series["Normf"].Points.AddXY(i, f);
            }

            chart2.Series.Add("+DNorm"); //   +D{Q}
            chart2.Series["+DNorm"].Color = Color.LightGreen;
            chart2.Series["+DNorm"].ChartType = SeriesChartType.Line;

            chart2.Series.Add("-DNorm"); //   -D{Q}
            chart2.Series["-DNorm"].Color = Color.LightGreen;
            chart2.Series["-DNorm"].ChartType = SeriesChartType.Line;

           
            double D = 0;
            for (double i = MinX - step; i < MaxX + 2 * step; i += 0.01 * (MaxX - MinX))
            {
                D = NormDisp(S, m, i);
                chart2.Series["+DNorm"].Points.AddXY(i, Counts.FuncNorm(i, m, S) + Quantils.Student(N) * Math.Sqrt(D));
                chart2.Series["-DNorm"].Points.AddXY(i, Counts.FuncNorm(i, m, S) - Quantils.Student(N) * Math.Sqrt(D));
                chart2.Series["Norm"].Points.AddXY(i, Counts.FuncNorm(i, m, S));
            }
            double
               Dm = Math.Pow(S, 2) / N,
               Ds = Math.Pow(S, 2) / (2 * N);

            dataGridView1.Rows.Add("Параметр δ", Math.Round(S - Math.Sqrt(Ds), 4), Math.Round(S, 4),
                Math.Round(S + Math.Sqrt(Ds), 4), Math.Round(Math.Sqrt(Ds), 4));
            dataGridView1.Rows.Add("Параметр m", Math.Round(m -  Math.Sqrt(Dm), 4), Math.Round(m, 4),
                Math.Round(m + Math.Sqrt(Dm), 4), Math.Round(Math.Sqrt(Dm), 4));
        }
        private void експоненціальнийToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            N = allnum.Length;
            MinX = allnum[0];
            MaxX = allnum[N - 1];
            double AverageX = Functions.Average(N, allnum); // Average X

            double s = 0;
            if (textBox4.TextLength > 0)
                s = Convert.ToInt32(textBox4.Text);
            step = Counts.Step(s, N, MinX, MaxX);// Counting step


            dataGridView1.Rows.Clear();
            Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, false, false);
            ReadFile = false;

            //
            //Побудова
            //

            chart1.Series.Add("Expf");//    f(x,Q)
            chart1.Series["Expf"].Color = Color.Red;
            chart1.Series["Expf"].ChartType = SeriesChartType.Line;

            double lyambda = 1 / AverageX;

            for (double i = 0; i < MaxX + 2 * step; i += 0.01 * (MaxX - MinX))
                chart1.Series["Expf"].Points.AddXY(i , lyambda * Math.Exp(-lyambda*i)*step);

            chart2.Series.Add("ExpF"); //   F(x,Q)
            chart2.Series["ExpF"].Color = Color.Green;
            chart2.Series["ExpF"].ChartType = SeriesChartType.Line;

            chart2.Series.Add("D1ExpF"); //   -D{Q}
            chart2.Series["D1ExpF"].Color = Color.LightGreen;
            chart2.Series["D1ExpF"].ChartType = SeriesChartType.Line;

            chart2.Series.Add("D2ExpF"); //   +D{Q}
            chart2.Series["D2ExpF"].Color = Color.LightGreen;
            chart2.Series["D2ExpF"].ChartType = SeriesChartType.Line;

            double D = 0;
            for (double i = 0; i < MaxX + 2 * step; i += 0.01*(MaxX-MinX))
            {
                D =  Math.Pow(i, 2) * Math.Exp(-2 * lyambda * i) * (Math.Pow(lyambda, 2) / N);
                chart2.Series["D1ExpF"].Points.AddXY(i, Counts.FuncEmperic(i, 1 / AverageX) - Quantils.Student(N) * Math.Sqrt(D));
                if (Counts.FuncEmperic(i, 1 / AverageX) + Quantils.Student(N) * Math.Sqrt(D) <= 1)
                    chart2.Series["D2ExpF"].Points.AddXY(i, Counts.FuncEmperic(i, 1 / AverageX) + Quantils.Student(N) * Math.Sqrt(D));
                else
                    chart2.Series["D2ExpF"].Points.AddXY(i, 1);
                chart2.Series["ExpF"].Points.AddXY(i, Counts.FuncEmperic(i, 1 / AverageX));
            }
            double Dl = Math.Pow(lyambda, 2) / N;
            dataGridView1.Rows.Add("Параметр λ", Math.Round(lyambda - Math.Sqrt(Dl), 4), Math.Round(lyambda, 4),
                Math.Round(lyambda + Math.Sqrt(Dl), 4), Math.Round(Math.Sqrt(Dl), 4));
        }
        private void екстремальнийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            N = allnum.Length;
            MinX = allnum[0];
            MaxX = allnum[N - 1];
            double AverageX = Functions.Average(N, allnum); // Average X

            double s = 0;
            if (textBox4.TextLength > 0)
                s = Convert.ToInt32(textBox4.Text);
            step = Counts.Step(s, N, MinX, MaxX);// Counting step

            dataGridView1.Rows.Clear();
            Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, false, false);
            ReadFile = false;

            //
            //Побудова
            //

            chart1.Series.Add("Expf");//    f(x,Q)
            chart1.Series["Expf"].Color = Color.Red;
            chart1.Series["Expf"].ChartType = SeriesChartType.Line;

            chart2.Series.Add("ExtF"); //   F(x,Q)
            chart2.Series["ExtF"].Color = Color.Green;
            chart2.Series["ExtF"].ChartType = SeriesChartType.Line;

            chart2.Series.Add("+DExtreme"); //   +D{Q}
            chart2.Series["+DExtreme"].Color = Color.LightGreen;
            chart2.Series["+DExtreme"].ChartType = SeriesChartType.Line;

            chart2.Series.Add("-DExtreme"); //   -D{Q}
            chart2.Series["-DExtreme"].Color = Color.LightGreen;
            chart2.Series["-DExtreme"].ChartType = SeriesChartType.Line;

            
            double a11 = N - 1;
            double
                a12 = 0,
                a21 = 0,
                a22 = 0;

            for (int i = 0; i < N - 1; i++)
            {
                a12 += Math.Log(allnum[i]);
                a22 += Math.Pow(Math.Log(allnum[i]), 2);
            }
            a21 = a12;

            double b1 = 0, b2 = 0;
            for(int i =0; i < N-1 ; i++)
            {
                b1 += Math.Log(Math.Log(1 + Math.Log(1.0 / (1 - ((double)(i+1) / N)))));
                b2 += Math.Log(allnum[i]) * Math.Log(Math.Log(1.0 + Math.Log(1.0 / (1 - ((double)(i+1) / N)))));
            }

            double Bt = (b1 - (a11 / a21) * b2) / (a12 - (a22 / a21) * a11);
            double A = (b2 - Bt * a22) / a21;
            double B = Math.Exp(A);


            double Szal = 0;
            for (int i = 0; i < N - 1; i++)
            {
                double Zl = Math.Log(Math.Log(1 + Math.Log(1.0 / (1 - ((double)(i) / N)))));
                if (i == 0 || i == N)
                    Zl = 0;
                Szal += Math.Pow(Zl - Math.Log(B) - Bt * Math.Log(allnum[i]), 2);
            }
            Szal /= (N - 3);
            double DA = (a22 * Szal) / (a11 * a22 - a12 * a21);
            double DB = Math.Pow(B, 2) * DA;
            double DBt = (a11 * Szal) / (a11 * a22 - a12 * a21);
            
            double f = 0;

            for (double i = MinX - step; i < MaxX + 2*step; i += 0.01 * (MaxX - MinX))
            {
                f = step * B * Bt * Math.Pow(i, Bt - 1) * Math.Exp(-(Math.Exp(B * Math.Pow(i, Bt)) - 1)) * Math.Exp(B * Math.Pow(i, Bt));
                chart1.Series["Expf"].Points.AddXY(i, f);//step * (Math.Exp(-Math.Exp((A - i) / B) + (A - i) / B)) / B);
            }


            double D = 0;
            
            for(int i =0;i< N;i++)
            {
                D = ExtremeDisp(Alpha, Bt, allnum[i],i, N, allnum);
                chart2.Series["+DExtreme"].Points.AddXY(allnum[i], Counts.FuncExtreme(allnum[i], B, Bt) + Quantils.Student(N) * Math.Sqrt(D));
                chart2.Series["-DExtreme"].Points.AddXY(allnum[i], Counts.FuncExtreme(allnum[i], B, Bt) - Quantils.Student(N) * Math.Sqrt(D));
                chart2.Series["ExtF"].Points.AddXY(allnum[i], Counts.FuncExtreme(allnum[i], B, Bt));
            }
            
            dataGridView1.Rows.Add("Параметр B", Math.Round(B - Math.Sqrt(DB),4), Math.Round(B,4),
                Math.Round(B + Math.Sqrt(DB),4), Math.Round(Math.Sqrt(DB),4));
            dataGridView1.Rows.Add("Параметр β", Math.Round(Bt - Math.Sqrt(DBt),4), Math.Round(Bt,4),
                Math.Round(Bt + Math.Sqrt(DBt),4), Math.Round(Math.Sqrt(DBt),4));
        }
        
        //Стандартизація даних
        private void стандартзаціяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RunClicked)
            {
                Counts.Sort(allnum);
                N = allnum.Length;
                double AverageX = Functions.Average(N, allnum); // Average X
                double delt = Functions.Sigm(N, AverageX, allnum);
                for (int i = 0; i < N; i++)
                {
                    allnum[i] = (allnum[i] - AverageX) / delt;
                }
                Counts.Sort(allnum);
                N = allnum.Length;
                MinX = allnum[0];
                MaxX = allnum[N - 1];
                AverageX = Functions.Average(N, allnum); // Average X

                double s = 0;
                if (textBox4.TextLength > 0)
                    s = Convert.ToInt32(textBox4.Text);
                step = Counts.Step(s, N, MinX, MaxX);// Counting step

                dataGridView1.Rows.Clear();
                Programm(chart1, chart2, N, MinX, MaxX, allnum, step, dataGridView1, AverageX, false, false);
                ReadFile = false;
            }

        }

        //Вихід
        private void Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}