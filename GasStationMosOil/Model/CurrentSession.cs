using System.Collections.Generic;

namespace GasolineMosOil.Model
{
    public static class CurrentSession
    {
        public static Receipt CurrentReceipt = new();

        public static double Revenue;

        public static List<Receipt> HistoryOfReceipts = new();

        public static void CreateNewReceipt(double receiptTotal)
        {
            if (receiptTotal == 0)
            {
                CurrentReceipt = new Receipt();
            }
            else
            {
                HistoryOfReceipts.Add(CurrentReceipt);
                CurrentReceipt = new Receipt();
                Revenue += receiptTotal;
            }
        }
    }
}