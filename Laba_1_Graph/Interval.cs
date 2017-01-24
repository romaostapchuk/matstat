using System;

namespace Laba_1_Graph
{
    public static class Interval
    {
        /// <summary>
        /// Interval Average X
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="Disp"></param>
        /// <param name="AverageX"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public static double AverageB(int amount, double Mean, double AverageX)
        {
            double T = Quantils.Student(amount);
            return Math.Round(AverageX - (T * Mean) / (Math.Sqrt(amount)),4);
        }
        public static double AverageU(int amount, double Mean, double AverageX)
        {
            double T = Quantils.Student(amount);
            return Math.Round(AverageX + (T * Mean) / (Math.Sqrt(amount)),4);
        }

        /// <summary>
        /// Interval Dispersion
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="Disp"></param>
        /// <param name="AverageX"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public static double SigmB(int amount, double Mean)
        {
            double T = Quantils.Student(amount);
            return Math.Round(Mean - (T * Mean) / (2 * Math.Sqrt(amount)),4);
        }
        public static double SigmU(int amount, double Mean)
        {
            double T = Quantils.Student(amount);
            return Math.Round(Mean + (T * Mean) / (2 * Math.Sqrt(amount)),4);
        }

        /// <summary>
        /// Interval Asimetry
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="AverageX"></param>
        /// <param name="T"></param>
        /// <param name="M2"></param>
        /// <param name="M3"></param>
        /// <param name="M4"></param>
        /// <param name="M5"></param>
        /// <param name="M6"></param>
        /// <returns></returns>
        public static double AsimB(int amount, double Asim, double[] numb)
        {
            return Math.Round(Asim - SigmaAsim(amount,Asim,numb),4);
        }
        public static double AsimU(int amount, double Asim, double[] numb)
        {
            return Math.Round(Asim + SigmaAsim(amount, Asim, numb), 4);
        }

        /// <summary>
        /// Interval Exces
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="AverageX"></param>
        /// <param name="T"></param>
        /// <param name="M2"></param>
        /// <param name="M3"></param>
        /// <param name="M4"></param>
        /// <param name="M5"></param>
        /// <param name="M6"></param>
        /// <param name="M8"></param>
        /// <returns></returns>
        public static double ExcesB(int amount, double Excess, double[] numb)
        {
            double T = Quantils.Student(amount);
            double
               M2 = Moments.Central(amount, numb, 2),
               M3 = Moments.Central(amount, numb, 3),
               M4 = Moments.Central(amount, numb, 4),
               M5 = Moments.Central(amount, numb, 5),
               M6 = Moments.Central(amount, numb, 6),
               M8 = Moments.Central(amount, numb, 8);
            double
                B1 = (M3 * M3) / Math.Pow(M2, 3),
                B2 = M4 / Math.Pow(M2, 2),
                B3 = (M3 * M5) / Math.Pow(M2, 4),
                B4 = M6 / Math.Pow(M2, 3),
                B6 = M8 / Math.Pow(M2, 4);
            return Math.Round(Excess - (T * Math.Sqrt((B6 - 4 * B4 * B2 - 8 * B3 + 4 * Math.Pow(B2, 3) 
                - Math.Pow(B2, 2) + 16 * B2 * B1 + 16 * B1) / (amount))),4);
        }
        public static double ExcesU(int amount, double Excess, double[] numb)
        {
            double T = Quantils.Student(amount);
            double
               M2 = Moments.Central(amount, numb, 2),
               M3 = Moments.Central(amount, numb, 3),
               M4 = Moments.Central(amount, numb, 4),
               M5 = Moments.Central(amount, numb, 5),
               M6 = Moments.Central(amount, numb, 6),
               M8 = Moments.Central(amount, numb, 8);
            double
                B1 = (M3 * M3) / Math.Pow(M2, 3),
                B2 = M4 / Math.Pow(M2, 2),
                B3 = (M3 * M5) / Math.Pow(M2, 4),
                B4 = M6 / Math.Pow(M2, 3),
                B6 = M8 / Math.Pow(M2, 4);
            return Math.Round(Excess + (T * Math.Sqrt((B6 - 4 * B4 * B2 - 8 * B3 + 4 * Math.Pow(B2, 3) 
                - Math.Pow(B2, 2) + 16 * B2 * B1 + 16 * B1) / (amount))),4);
        }

        /// <summary>
        /// Interval ratio of variety
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="AverageX"></param>
        /// <param name="T"></param>
        /// <param name="Wn"></param>
        /// <returns></returns>
        public static double NRV_B(int amount,double Wn)
        {
            double T = Quantils.Student(amount);
            return Math.Round(Wn - (T * Wn * Math.Sqrt((1 + 2 * Math.Pow(Wn, 2)) / (2 * amount))),4);
        }
        public static double NRV_U(int amount,double Wn)
        {
            double T = Quantils.Student(amount);
            return Math.Round(Wn + (T * Wn * Math.Sqrt((1 + 2 * Math.Pow(Wn, 2)) / (2 * amount))),4);
        }
        
        public static double SigmaNRV(int amount, double Wn)
        {
            return Math.Round((Wn * Math.Sqrt((1 + 2 * Math.Pow(Wn, 2)) / (2 * amount))), 4);
        }

        public static double SigmaAverage(int amount, double Mean)
        {
            return Math.Round(Mean / (Math.Sqrt(amount)),4);
        }
        
        public static double SigmaDisp(int amount, double Mean)
        {
            return Math.Round(Mean / (2 * Math.Sqrt(amount)), 4);
        }
        
        public static double SigmaAsim(int amount, double Asim, double[] numb)
        {
            double
                M2 = Moments.Central(amount, numb, 2),
                M3 = Moments.Central(amount, numb, 3),
                M4 = Moments.Central(amount, numb, 4),
                M5 = Moments.Central(amount, numb, 5),
                M6 = Moments.Central(amount, numb, 6);
            double
                B1 = (M3 * M3) / Math.Pow(M2, 3),
                B2 = M4 / Math.Pow(M2, 2),
                B3 = (M3 * M5) / Math.Pow(M2, 4),
                B4 = M6 / Math.Pow(M2, 3);
            return Math.Round(Math.Sqrt(Math.Abs((4.0 * B4 - 12.0 * B3 - 24.0 * B2
                + 9.0 * B2 * B1 + 35.0 * B1 - 36.0) * (1.0 / (4.0 * amount)))), 4);
        }
        
        public static double SigmaExces(int amount, double Excess, double[] numb)
        {
            double
               M2 = Moments.Central(amount, numb, 2),
               M3 = Moments.Central(amount, numb, 3),
               M4 = Moments.Central(amount, numb, 4),
               M5 = Moments.Central(amount, numb, 5),
               M6 = Moments.Central(amount, numb, 6),
               M8 = Moments.Central(amount, numb, 8);
            double
                B1 = (M3 * M3) / Math.Pow(M2, 3),
                B2 = M4 / Math.Pow(M2, 2),
                B3 = (M3 * M5) / Math.Pow(M2, 4),
                B4 = M6 / Math.Pow(M2, 3),
                B6 = M8 / Math.Pow(M2, 4);
            return Math.Round(Math.Sqrt((B6 - 4 * B4 * B2 - 8 * B3 + 4 * Math.Pow(B2, 3)
                - Math.Pow(B2, 2) + 16 * B2 * B1 + 16 * B1) / (amount)), 4);
        }
    }
}