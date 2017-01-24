using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_1_Graph
{
    class Quantils
    {
        public static double Student(int amount)//0,05  t(l/2, nu)
        {
            double T = 0;
            if (amount - 1 == 1)
                T = 6.3138;
            else if (amount - 1 == 2)
                T = 2.9200;
            else if (amount - 1 == 3)
                T = 2.3534;
            else if (amount - 1 == 4)
                T = 2.1318;
            else if (amount - 1 == 5)
                T = 2.0150;
            else if (amount - 1 == 6)
                T = 1.9432;
            else if (amount - 1 == 7)
                T = 1.8946;
            else if (amount - 1 == 8)
                T = 1.8595;
            else if (amount - 1 == 9)
                T = 1.8331;
            else if (amount - 1 == 10)
                T = 1.8125;
            else if (amount - 1 == 11)
                T = 1.7959;
            else if (amount - 1 == 12)
                T = 1.7823;
            else if (amount - 1 == 14 || amount - 1 == 13)
                T = 1.7613;
            else if (amount - 1 == 16 || amount - 1 == 15)
                T = 1.7459;
            else if (amount - 1 == 18 || amount - 1 == 17)
                T = 1.7341;
            else if (amount - 1 == 20 || amount - 1 == 19)
                T = 1.7247;
            else if (amount - 1 > 20 && amount - 1 <= 30)
                T = 1.7081;
            else if (amount - 1 > 30 && amount - 1 <= 40)
                T = 1.6896;
            else if (amount - 1 > 40 && amount - 1 <= 60)
                T = 1.6759;
            else if (amount - 1 > 60 && amount - 1 <= 120)
                T = 1.6564;
            else if (amount - 1 > 120)
                T = 1.6449;
            return T;
        }

        public static double Normal(double L)
        {
            if (L == 0)
            {
                L = 0.01;
            }
            double p = L;
            double alf = L * 2;
            double t = Math.Sqrt(Math.Log(1 / (L * L)));
            double e = 4.5 * Math.Pow(10, -4);
            double[] c = { 2.515517, 0.802853, 0.010328 };
            double[] d = { 1.432788, 0.1892659, 0.001308 };
            double hlpdou1 = (c[0] + c[1] * t + c[2] * t * t);
            double hlpdou2 = (1 + d[0] * t + d[1] * t * t + d[2] * t * t * t);
            double rez = t - ((hlpdou1) / (hlpdou2)) + e;
            return -rez;
        }

        public static double Fisher(double L, double v1, double v2)
        {
            double s = 1.0 / v1 + 1.0 / v2;
            double b = 1.0 / v1 - 1.0 / v2;
            double norm = Normal(L);
            double Z;
            Z = norm * Math.Sqrt(s / 2) - (1 / 6) * b * (norm * norm + 2) +
                Math.Sqrt(s / 2) * ((s / 24) * (norm * norm + 3 * norm) + (1 / 72) * (b * b / s) * (Math.Pow(norm, 3) + 11 * norm)) -
                (b * s / 120) * (Math.Pow(norm, 4) + 9 * Math.Pow(norm, 2) + 8) +
                (Math.Pow(b, 3) / 3240 * s) * (3 * Math.Pow(norm, 4) + 7 * Math.Pow(norm, 2) - 16) +
                Math.Sqrt(s / 2) * ((s * s / 1920) * (Math.Pow(norm, 5) + 20 * Math.Pow(norm, 3) + 15 * norm)) +
                (Math.Pow(b, 4) / 2880) * (Math.Pow(norm, 5) + 44 * Math.Pow(norm, 3) + 183 * norm) +
                (Math.Pow(b, 4) / (155520 * s * s)) * (9 * Math.Pow(norm, 5) - 284 * Math.Pow(norm, 3) - 1513 * norm);
            return (Z);
        }

        public static double XiSquare(double L, double v)
        {
            double X_2 = 0;
            X_2 = v * Math.Pow(1 - (2 / (9 * v)) * +Normal(L) * Math.Sqrt((2 / (9 * v))), 3);
            return (X_2);
        }
    }
}
