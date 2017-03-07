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
    public partial class _2Curs : Form
    {
        bool Binary = true;
        bool[] Regr;
        double[] arr_1;     double Min_1; double Max_1;
        double[] arr_2;     double Min_2; double Max_2;

        double[] undo_1;
        double[] undo_2;

        double step_1 = 0;
        double step_2 = 0;

        double Rxy = 0;

        Point? prevPosition = null;         // position on a chart
        ToolTip tooltip = new ToolTip();    // toolabar on a


        public _2Curs()
        {
            InitializeComponent();
        }
        private void Chart_MouseMove2(object sender, MouseEventArgs e)
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
        private void Chart_MouseMove4(object sender, MouseEventArgs e)
        {
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            tooltip.RemoveAll();
            prevPosition = pos;
            var results = chart4.HitTest(pos.X, pos.Y, false, ChartElementType.PlottingArea);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.PlottingArea)
                {
                    var xVal = result.ChartArea.AxisX.PixelPositionToValue(pos.X);
                    var yVal = result.ChartArea.AxisY.PixelPositionToValue(pos.Y);

                    tooltip.Show("X=" + xVal, this.chart4, pos.X, pos.Y - 15);
                }
            }
        }

        public void GetValues(double[] arr1, double[] arr2, bool N_D, bool[] Rgr)
        {
            Binary = N_D;
            Regr = new bool[Rgr.Length];
            for (int i = 0; i < Rgr.Length; i++)
                Regr[i] = Rgr[i];
            arr_1 = new double[arr1.Length];
            int k = 0;
            foreach (double element in arr1)
            {
                arr_1[k] = element;
                k++;
            }

            arr_2 = new double[arr2.Length];
            k = 0;
            foreach (double element in arr2)
            {
                arr_2[k] = element;
                k++;
            }

            undo_1 = new double[arr_1.Length];
            for (int i = 0; i < arr_1.Length; i++)
            {
                undo_1[i] = arr_1[i];
            }
            undo_2 = new double[arr_2.Length];
            for (int i = 0; i < arr_2.Length; i++)
            {
                undo_2[i] = arr_2[i];
            }

            Compare_Show(arr_1, arr_2);
        }

        public void Compare_Show(double[] arr1, double[] arr2)
        {
            this.chart2.MouseMove += Chart_MouseMove2;
            this.chart4.MouseMove += Chart_MouseMove4;

            arr_1 = new double[arr1.Length];
            int k = 0;
            foreach (double element in arr1)
            {
                arr_1[k] = element;
                k++;
            }

            arr_2 = new double[arr2.Length];
            k = 0;
            foreach (double element in arr2)
            {
                arr_2[k] = element;
                k++;
            }

            Min_1 = arr_1.Min();
            Max_1 = arr_1.Max();

            Min_2 = arr_2.Min();
            Max_2 = arr_2.Max();
            
            double s = 0;
            step_1 = Counts.Step(s, arr_1.Length, Min_1, Max_1);// Counting step
            step_2 = Counts.Step(s, arr_2.Length, Min_2, Max_2);// Counting step


            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();


            List<double> Xi_Vl = new List<double>();
            List<double> Xi_Bd = new List<double>();

            List<double> arr_1_Vl = new List<double>();
            List<double> arr_2_Vl = new List<double>();

            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart4.Series.Clear();

            double[] arr_1B = new double[arr_1.Length];
            double[] arr_2B = new double[arr_2.Length];
            for (int i = 0; i < arr_1.Length; i++)
                arr_1B[i] = arr_1[i];
            for (int i = 0; i < arr_2.Length; i++)
                arr_2B[i] = arr_2[i];
            Counts.Sort(arr_1B);
            Counts.Sort(arr_2B);

            Graphs.EmpericNorm(chart2, arr_1B.Length, Min_1, Max_1, ref arr_1B, step_1);
            Graphs.EmpericClass(chart2, arr_1B.Length, Min_1, Max_1, ref arr_1B, step_1, Xi_Bd);
            Graphs.Histogram(chart1, arr_1B.Length, Min_1, Max_1, ref arr_1B, step_1, arr_1_Vl);

            Graphs.EmpericNorm(chart4, arr_2B.Length, Min_2, Max_2, ref arr_2B, step_2);
            Graphs.EmpericClass(chart4, arr_2B.Length, Min_2, Max_2, ref arr_2B, step_2, Xi_Bd);
            Graphs.Histogram(chart3, arr_2B.Length, Min_2, Max_2, ref arr_2B, step_2, arr_2_Vl);

            ShowValues();
            if (!Binary)
                ValuesOnTab();
            if (Binary)
            {
                CountCorrelation(step_1);
                TablesCount();
                Regress();
            }
        }

        private void ShowValues()
        {
            double av_1 = Functions.Average(arr_1.Length, arr_1);
            this.label1.Text = "Дисперсія = " + 
                Convert.ToString(Functions.Disp(arr_1.Length, av_1, arr_1));
            this.label2.Text = "Максимум = " + Math.Round(Max_1 , 4);
            this.label3.Text = "Мінімум = " + Math.Round(Min_1, 4);
            this.label9.Text = "Кількість = " + arr_1.Length;
            this.label7.Text = "Середнє = " + av_1;

            double av_2 = Functions.Average(arr_2.Length, arr_2);
            this.label6.Text = "Дисперсія = " +
                Convert.ToString(Functions.Disp(arr_2.Length, av_2, arr_2));
            this.label5.Text = "Максимум = " + Math.Round(Max_2, 4);
            this.label4.Text = "Мінімум = " + Math.Round(Min_2, 4);
            this.label10.Text = "Кількість = " + arr_2.Length;
            this.label8.Text = "Середнє = " + av_2;
        }
        private void ValuesOnTab()
        {
            dataGridView1.ColumnCount = 3; 
            dataGridView1.Columns[0].Name = "Назва";
            dataGridView1.Columns[2].Name = "Ӫ";
            dataGridView1.Columns[1].Width = 250;
            dataGridView1.Columns[0].Width = 250;
            
            double Val1 = 0;

            double av_1 = Functions.Average(arr_1.Length, arr_1);
            double av_2 = Functions.Average(arr_2.Length, arr_2);
            double E = (av_1 + av_2) / 2;                                   //Math expectation
            dataGridView1.Rows.Add("Мат. сподівання:", "", Math.Round(E, 4));

            double Disp_1 = Functions.Disp(arr_1.Length, av_1, arr_1);
            double Disp_2 = Functions.Disp(arr_2.Length, av_2, arr_2);      //Dispersion for DC

            if (Ftest.CheckFtest(Disp_1, Disp_2, arr_1.Length, arr_2.Length, ref Val1))
            {
                dataGridView1.Rows.Add("Дисперсії(f-Test): ", "збіжні", Math.Round(Val1, 4));
                /*      перевірка збігу середніх        */
                double T_z;
                string Dis_z;
                if (arr_1.Length == arr_2.Length)                               //Dependent 
                {
                    T_z = _2D.TDep2D(arr_1.Length, arr_1, arr_2);
                    Dis_z = _2D.Check_T_Dep(T_z, arr_1.Length) ? "збігаються(залежні)" : "не збігaються(залежні)";
                }
                else
                {
                    T_z = _2D.TIndep2D(av_1, av_2, _2D.IndepDisp(arr_1.Length, arr_2.Length, Disp_1, Disp_2));
                    Dis_z = _2D.Check_T_Indep(T_z, arr_1.Length + arr_2.Length - 2) ? "збігаються(незалежні)" : "не збігaються(незалежні)";
                }
                dataGridView1.Rows.Add("Середні(T-test):", Dis_z, Math.Round(T_z, 4));
            }
            else
                dataGridView1.Rows.Add("Дисперсії(f-Test): ", "незбіжні", Math.Round(Val1, 4));

            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add("Критерії");

            if (Criteria.Smirnof_Kolmahorof(arr_1, arr_2, ref Val1))
                dataGridView1.Rows.Add("Cмирнова-Колмогорова [F(x) = G(y)]:", "Однорідні", Val1);
            else
                dataGridView1.Rows.Add("Cмирнова-Колмогорова [F(x) = G(y)]:", "Не однорідні", Val1);
            if (Criteria.Vilkokson(arr_1, arr_2, ref Val1))
            {
                //dataGridView1.Rows.Add("Cмирнова-Колмогорова [F(x) = G(y)]:", "Однорідні", Val1 + 0.134);
                dataGridView1.Rows.Add("Вілкоксона [F(x) = G(y)]:", "Однорідні", Val1);
            }
            else
            {
                //dataGridView1.Rows.Add("Cмирнова-Колмогорова [F(x) = G(y)]:", "Не однорідні", Val1 + 0.134);
                dataGridView1.Rows.Add("Вілкоксона [F(x) = G(y)]:", "Не однорідні", Val1);
            }

            if (Criteria.U_criteria(arr_1, arr_2, ref Val1))
                dataGridView1.Rows.Add("U-Критерій [F(x) = G(y)]:", "Однорідні", Val1);
            else
                dataGridView1.Rows.Add("U-Критерій [F(x) = G(y)]:", "Не однорідні", Val1);

            if (Criteria.Diff_Average(arr_1, arr_2, ref Val1))
                dataGridView1.Rows.Add("Різнця середніх рангів:", "Однорідні", Val1);
            else
                dataGridView1.Rows.Add("Різнця середніх рангів:", "Не однорідні", Val1);

            if (arr_1.Length == arr_2.Length)
            {
                double move = 0;
                if (Criteria.Sign(arr_1, arr_2, ref move, ref Val1))
                    dataGridView1.Rows.Add("Критерій знаків:", "нуль", Val1); // mediana riznici
                else
                    dataGridView1.Rows.Add("Критерій знаків:", "не нуль, зсув(" + move + ")", Val1);
            }
        }

        private void CountCorrelation(double step)
        {
            dataGridView2.ColumnCount = 4;
            dataGridView2.Columns[0].Name = "Назва";
            dataGridView2.Columns[2].Name = "Ӫ";
            dataGridView2.Columns[3].Name = "Інервал";

            dataGridView2.Columns[1].Width = 150;
            dataGridView2.Columns[0].Width = 250;
            dataGridView2.Columns[3].Width = 150;
            if (arr_1.Length == arr_2.Length)
            {
                int N = arr_1.Length;
                double av_1 = Functions.Average(N, arr_1);
                double av_2 = Functions.Average(N, arr_2);

                Rxy = Correlation.Pair(arr_1, arr_2, av_1, av_2);
                if (Laba_1_Graph.Correlation.Check_Correlation(Rxy, N))
                    dataGridView2.Rows.Add("Парна кореляція:", "значуща", Math.Round(Rxy, 4), 
                        "[ " + Correlation.IntervalBot(Rxy, N) + " ; " 
                        + Correlation.IntervalTop(Rxy, N) + " ]");
                else
                    dataGridView2.Rows.Add("Парна кореляція:", "не значуща", Math.Round(Rxy, 4),
                        "[ " + Correlation.IntervalBot(Rxy, N) + " ; "
                        + Correlation.IntervalTop(Rxy, N) + " ]");
                double P = 0;
                if (Correlation.Ratio(arr_1, arr_2, step, av_2, ref P))
                    dataGridView2.Rows.Add("Кореляційне відношення:", "зв'язок присутній", Math.Round(P, 4));
                else
                    dataGridView2.Rows.Add("Кореляційне відношення:", "зв'язок відсутній", Math.Round(P, 4));

                double Tc = 0;
                if (Correlation.Spirmen(arr_1, arr_2, ref Tc))
                    dataGridView2.Rows.Add("Коефіцієн Спірмена:", "значущий", Math.Round(Tc, 4));
                else
                    dataGridView2.Rows.Add("Коефіцієн Спірмена:", "не значущий", Math.Round(Tc, 4));

                double T = 0;
                if (Correlation.Kendal(arr_1, arr_2, ref T))
                    dataGridView2.Rows.Add("Коефіцієн Кендалла:", "значущий", Math.Round(T, 4));
                else
                    dataGridView2.Rows.Add("Коефіцієн Кендалла:", "не значущий", Math.Round(T, 4));

                double Cc = 0;
                Correlation.Concord(arr_1, arr_2, ref Cc);
                dataGridView2.Rows.Add("Коефіцієн конкордації:", "", Math.Round(Cc, 4));
            }
        }
        private void TablesCount()
        {
            dataGridView3.ColumnCount = 3;
            dataGridView3.Columns[0].Name = "Назва";
            dataGridView3.Columns[2].Name = "Ӫ";
                        
            dataGridView3.Columns[1].Width = 150;
            dataGridView3.Columns[0].Width = 250;

            if (arr_1.Length == arr_2.Length)
            {
                int N = arr_1.Length;
                double av_1 = Functions.Average(N, arr_1);
                double av_2 = Functions.Average(N, arr_2);

                double I = Tables.Fehner(arr_1, arr_2, av_1, av_2);
                dataGridView3.Rows.Add("Індекс Фехнера:", "", Math.Round(I, 4));

                double F = 0;
                if (Tables.Fi(arr_1, arr_2, av_1, av_2, ref F))
                    dataGridView3.Rows.Add("Оцінка коефіцієнта Ф:", "значуща", Math.Round(F, 4));
                else
                    dataGridView3.Rows.Add("Оцінка коефіцієнта Ф:", "не значуща", Math.Round(F, 4));

                double Q = 0;
                double Y = 0;
                if (Tables.Yula(arr_1, arr_2, av_1, av_2, ref Q, ref Y))
                    dataGridView3.Rows.Add("Оцінка коефіцієнта Юла:", "значуща", Math.Round(Q, 4) + ";" + Math.Round(Y, 4));
                else
                    dataGridView3.Rows.Add("Оцінка коефіцієнта Юла:", "не значуща", Math.Round(Q, 4) + ";" + Math.Round(Y, 4));
                
                double C = 0;
                double X_2 = 0;
                if (Tables.MxN_Pirson(arr_1, arr_2, step_1, step_2, ref C, ref X_2))
                {
                    dataGridView3.Rows.Add("X2 Пірсона:", "", Math.Round(X_2, 4));
                    dataGridView3.Rows.Add("Коефіцієнт сполучень Пірсона:", "значущий", Math.Round(C, 4));
                }
                else
                    dataGridView3.Rows.Add("Коефіцієнт сполучень Пірсона:", "не значущий", Math.Round(C, 4));
                
                double Tb = 0;
                if (Tables.Kendall(arr_1, arr_2, step_1, step_2, ref Tb))
                    dataGridView3.Rows.Add("Міра звязку Кендалла:", "значуща", Math.Round(Tb, 4));
                else if (Tables.Styuart(arr_1, arr_2, step_1, step_2, ref Tb))
                    dataGridView3.Rows.Add("Статистика Стюарда:", "значуща", Math.Round(Tb, 4));
                else
                    dataGridView3.Rows.Add("Статистика Стюарда:", "не значуща", Math.Round(Tb, 4));
                }
        }
        private void Regress()
        {
            dataGridView4.ColumnCount = 3;

            dataGridView4.Columns[1].Width = 150;
            dataGridView4.Columns[2].Width = 200;
            dataGridView4.Columns[0].Width = 250;
            if (arr_1.Length == arr_2.Length)
            {
                double
                    S2_L,
                    S2_P;
                if (Regr[1])    //      Linear regression
                { 
                    if (Regression.Linear.Bartlet(arr_1, arr_2, step_1))
                    {
                        double a = 0;
                        double b = 0;
                        S2_L = Regression.Linear.MNK(arr_1, arr_2, ref a, ref b);
                        dataGridView4.Rows.Add("Лінійна регресія:", "МНК S2", Math.Round(S2_L, 4));
                        dataGridView4.Rows.Add("", "", "a = " + Math.Round(a, 4) + ";" + "b = " + Math.Round(b, 4));


                        double R_2 = Regression.Linear.Determination(arr_1, arr_2);
                        dataGridView4.Rows.Add("Коефіцієнт детермінації:", "", Math.Round(R_2, 4));

                        if (Regression.Linear.Model(S2_L, arr_2))
                            dataGridView4.Rows.Add("Модель регресії:", "адекватна");
                        else
                            dataGridView4.Rows.Add("Модель регресії:", "не адекватна");
                    }
                    else
                        dataGridView4.Rows.Add("Лінійна регресія:", "умови не виконуються");
                }
                else if (Regr[0])   //  Parabolic regression
                {
                    double[] abc = { 0, 0, 0 };
                    double[] a1b1c1 = { 0, 0, 0 };

                    S2_P = Regression.Parabol.MNK1(arr_1, arr_2, Rxy, ref abc[0], ref abc[1], ref abc[2]);
                    double S2_P2 = Regression.Parabol.MNK2(arr_1, arr_2, Rxy, ref a1b1c1[0], ref a1b1c1[1], ref a1b1c1[2]);
                    dataGridView4.Rows.Add("Параболічна регресія:", "МНК S2", Math.Round(S2_P, 4));
                    dataGridView4.Rows.Add("", "",
                        "a = " + Math.Round(abc[0], 4) +
                        ";" + "b = " + Math.Round(abc[1], 4) +
                        ";" + "c = " + Math.Round(abc[2], 4));
                    bool check = Regression.Parabol.Check2(arr_1, arr_2, S2_P2, a1b1c1);
                    if (check)
                        dataGridView4.Rows.Add("Модель регресії:", "адеквана");
                    else
                        dataGridView4.Rows.Add("Модель регресії:", "не адеквана");
                }
                else if (Regr[11])   //  Kaziliear regression   11
                {
                    double[] ab = new double[2];
                    Regression.Kvazilinear.Transform(arr_1, arr_2, ref ab[0], ref ab[1],
                        Regression.Kvazilinear.Model_11.Fx, Regression.Kvazilinear.Model_11.Qy, Regression.Kvazilinear.Model_11.W);
                    double S2 = 0;
                    S2 = Regression.Kvazilinear.KvazilinearS2(arr_1, arr_2, ab, Regression.Kvazilinear.Model_11.Fx, 11);
                    dataGridView4.Rows.Add("Квазілінійна регресія:", "МНК S2", Math.Round(S2, 4));
                    double EMistake = 0;
                    if (Regression.Kvazilinear.RelativeMistake(arr_1, arr_2, ab, Regression.Kvazilinear.Model_11.Fx, 11, ref EMistake))
                        dataGridView4.Rows.Add("Відносна похибка:", "адекватна", Math.Round(EMistake, 4) + "%");
                    else
                        dataGridView4.Rows.Add("Відносна похибка:", "не адекватна", Math.Round(EMistake, 4) + "%");

                }
                else if (Regr[2])   //  Kaziliear regression    2
                {
                    double[] ab = new double[2];
                    Regression.Kvazilinear.Transform(arr_1, arr_2, ref ab[0], ref ab[1],
                        Regression.Kvazilinear.Model_2.Fx, Regression.Kvazilinear.Model_2.Qy, Regression.Kvazilinear.Model_2.W);
                    double S2 = 0;
                    S2 = Regression.Kvazilinear.KvazilinearS2(arr_1, arr_2, ab, Regression.Kvazilinear.Model_2.Fx, 2);
                    dataGridView4.Rows.Add("Квазілінійна регресія:", "МНК S2", Math.Round(S2, 4));
                    double EMistake = 0;
                    if (Regression.Kvazilinear.RelativeMistake(arr_1, arr_2, ab, Regression.Kvazilinear.Model_2.Fx, 2, ref EMistake))
                        dataGridView4.Rows.Add("Відносна похибка:", "адекватна", Math.Round(EMistake, 4) + "%");
                    else
                        dataGridView4.Rows.Add("Відносна похибка:", "не адекватна", Math.Round(EMistake, 4) + "%");
                }
                else if (Regr[3])   //  Kaziliear regression    3
                {
                    double[] ab = new double[2];
                    Regression.Kvazilinear.Transform(arr_1, arr_2, ref ab[0], ref ab[1],
                        Regression.Kvazilinear.Model_3.Fx, Regression.Kvazilinear.Model_3.Qy, Regression.Kvazilinear.Model_3.W);
                    double S2 = 0;
                    S2 = Regression.Kvazilinear.KvazilinearS2(arr_1, arr_2, ab, Regression.Kvazilinear.Model_3.Fx, 3);
                    dataGridView4.Rows.Add("Квазілінійна регресія:", "МНК S2", Math.Round(S2, 4));
                    double EMistake = 0;
                    if (Regression.Kvazilinear.RelativeMistake(arr_1, arr_2, ab, Regression.Kvazilinear.Model_3.Fx, 3, ref EMistake))
                        dataGridView4.Rows.Add("Відносна похибка:", "адекватна", Math.Round(EMistake, 4) + "%");
                    else
                        dataGridView4.Rows.Add("Відносна похибка:", "не адекватна", Math.Round(EMistake, 4) + "%");
                }
                else if (Regr[4])   //  Kaziliear regression    4
                {
                    double[] ab = new double[2];
                    Regression.Kvazilinear.Transform(arr_1, arr_2, ref ab[0], ref ab[1],
                        Regression.Kvazilinear.Model_4.Fx, Regression.Kvazilinear.Model_4.Qy, Regression.Kvazilinear.Model_4.W);
                    double S2 = 0;
                    S2 = Regression.Kvazilinear.KvazilinearS2(arr_1, arr_2, ab, Regression.Kvazilinear.Model_4.Fx, 4);
                    dataGridView4.Rows.Add("Квазілінійна регресія:", "МНК S2", Math.Round(S2, 4));
                    double EMistake = 0;
                    if (Regression.Kvazilinear.RelativeMistake(arr_1, arr_2, ab, Regression.Kvazilinear.Model_4.Fx, 4, ref EMistake))
                        dataGridView4.Rows.Add("Відносна похибка:", "адекватна", Math.Round(EMistake, 4) + "%");
                    else
                        dataGridView4.Rows.Add("Відносна похибка:", "не адекватна", Math.Round(EMistake, 4) + "%");
                }
                else if (Regr[6])   //  Kaziliear regression    6
                {
                    double[] ab = new double[2];
                    Regression.Kvazilinear.Transform(arr_1, arr_2, ref ab[0], ref ab[1],
                        Regression.Kvazilinear.Model_6.Fx, Regression.Kvazilinear.Model_6.Qy, Regression.Kvazilinear.Model_6.W);
                    double S2 = 0;
                    S2 = Regression.Kvazilinear.KvazilinearS2(arr_1, arr_2, ab, Regression.Kvazilinear.Model_6.Fx, 6);
                    dataGridView4.Rows.Add("Квазілінійна регресія:", "МНК S2", Math.Round(S2, 4));
                    double EMistake = 0;
                    if (Regression.Kvazilinear.RelativeMistake(arr_1, arr_2, ab, Regression.Kvazilinear.Model_6.Fx, 6, ref EMistake))
                        dataGridView4.Rows.Add("Відносна похибка:", "адекватна", Math.Round(EMistake, 4) + "%");
                    else
                        dataGridView4.Rows.Add("Відносна похибка:", "не адекватна", Math.Round(EMistake, 4) + "%");
                }
                else if (Regr[7])   //  Kaziliear regression    7
                {
                    double[] ab = new double[2];
                    Regression.Kvazilinear.Transform(arr_1, arr_2, ref ab[0], ref ab[1],
                        Regression.Kvazilinear.Model_7.Fx, Regression.Kvazilinear.Model_7.Qy, Regression.Kvazilinear.Model_7.W);
                    double S2 = 0;
                    S2 = Regression.Kvazilinear.KvazilinearS2(arr_1, arr_2, ab, Regression.Kvazilinear.Model_7.Fx, 7);
                    dataGridView4.Rows.Add("Квазілінійна регресія:", "МНК S2", Math.Round(S2, 4));
                    dataGridView4.Rows.Add("", "",
                        "a = " + Math.Round(ab[0], 4) +
                        ";" + "b = " + Math.Round(ab[1], 4));
                    double EMistake = 0;
                    if (Regression.Kvazilinear.RelativeMistake(arr_1, arr_2, ab, Regression.Kvazilinear.Model_7.Fx, 7, ref EMistake))
                        dataGridView4.Rows.Add("Відносна похибка:", "адекватна", Math.Round(EMistake, 4) + "%");
                    else
                        dataGridView4.Rows.Add("Відносна похибка:", "не адекватна", Math.Round(EMistake, 4) + "%");
                }
            }
        }


        private void вилученняАномальнихToolStripMenuItem_Click(object sender, EventArgs e)
        {
            arr_1 = remove_abnormal(arr_1, arr_1.Min(), arr_1.Max());
            arr_2 = remove_abnormal(arr_2, arr_2.Min(), arr_2.Max());
            Compare_Show(arr_1, arr_2);
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
        private void відновтиДаніToolStripMenuItem_Click(object sender, EventArgs e)
        {
            arr_1 = new double[undo_1.Length];
            for (int i = 0; i < undo_1.Length; i++)
            {
                arr_1[i] = undo_1[i];
            }

            arr_2 = new double[undo_2.Length];
            for (int i = 0; i < undo_2.Length; i++)
            {
                arr_2[i] = undo_2[i];
            }

            Compare_Show(arr_1, arr_2);
        }
        private void стандартзаціяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Counts.Sort(arr_1);
            Counts.Sort(arr_2);

            double av_1 = Functions.Average(arr_1.Length, arr_1);
            double av_2 = Functions.Average(arr_2.Length, arr_2);

            double delt = Functions.Sigm(arr_1.Length, av_1, arr_1);
            for (int i = 0; i < arr_1.Length; i++)
            {
                arr_1[i] = (arr_1[i] - av_1) / delt;
            }

            delt = Functions.Sigm(arr_2.Length, av_2, arr_2);
            for (int i = 0; i < arr_2.Length; i++)
            {
                arr_2[i] = (arr_2[i] - av_2) / delt;
            }

            Compare_Show(arr_1, arr_2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 window = new Form1();
            window.Compare_Show(arr_1);
            window.Show();
            window.FormClosed += new FormClosedEventHandler(Opened_Form_Closed);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 window = new Form1();
            window.Compare_Show(arr_2);
            window.Show();
            window.FormClosed += new FormClosedEventHandler(Opened_Form_Closed);
        }
        void Opened_Form_Closed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}