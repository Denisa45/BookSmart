using System;
using BookSmart.Data.Models;
using System.Globalization;   // (needed for parsing dates)

namespace BookSmart.Utils
{
    public static class FeeCalculator
    {
        // Calculate late fee given due date, current date, and daily rate
        public static decimal CalculateLateFee(DateTime dueAt, DateTime now, decimal ratePerDay)
        {
            if (now <= dueAt)
                return 0m; // Not late → no fee

            int daysLate = (now - dueAt).Days;
            return daysLate * ratePerDay;
        }
    }
}
