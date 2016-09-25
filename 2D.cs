using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_1_Graph
{
    class _2D
    {
        public static double    DepDisp    (int amount, double[] Z, double averageZ)
        {
            double sum = 0;
            for (int i = 0; i < amount; i ++)
                sum = Z[i] - averageZ;

            double Disp = (1 / (amount - 1)) * Math.Pow(sum, 2);
            return Disp;
        }
        public static double    TDep2D     (int amount, double[] numb1, double[] numb2)
        {

            double[] Z = new double[amount];

            for (int i = 0; i < amount; i++)
            {
                Z[i] = numb1[i] - numb2[i];
            }

            double sumZ = 0;
            for (int i = 0; i < amount; i++)
                sumZ += Z[i];

            double averageZ = (1 / amount) * sumZ;

            double D = DepDisp(amount, Z, averageZ);

            double T = (averageZ * Math.Sqrt(amount)) / Math.Sqrt(D);
            return T;
        }

        public static double    IndepDisp  (int amount1, int amount2, double Disp1, double Disp2)
        {
            double DispZ = Disp1 / amount1 + Disp2 / amount2;
            return DispZ;
        }
        public static double    TIndep2D   (double average1, double average2, double DispZ)
        {
            //******************************//
            //                              //
            //Nyu = N1 + N2 - 2, for quantil//
            //                              //
            //******************************//
            double T = (average1 - average2) / DispZ;
            return T;
        }

        class Ftest
        {
            public static double    F       (double Disp1, double Disp2)
            {
                double f = 0;
                if (Disp1 >= Disp2)
                    f = Disp1 / Disp2;
                else
                    f = Disp2 / Disp1;
                return f;
            }

            public static double    Flnyu   (int amount1, int amount2)
            {
                double F = 0;
                double
                    Nyu1 = amount1 - 1,
                    Nyu2 = amount2 - 1;

                double z = 0;

                F = Math.Exp(2 * z);
                return F;
            }
        }
    }
}
