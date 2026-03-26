namespace MortgageWebApp.Models
{
    public class PortfolioCalculationResult
    {
        public List<PaymentSchedule> CombinedSchedule { get; set; } = new();
        public decimal TotalInterestPaid { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public int TotalMonths { get; set; }
        public decimal TotalInterestSaved { get; set; }
        public int MonthsSaved { get; set; }
        public List<MortgagePeriodResult> PeriodResults { get; set; } = new();
    }

    public class MortgagePeriodResult
    {
        public int PeriodIndex { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal OriginalLoanAmount { get; set; }
        public decimal FinalBalance { get; set; }
        public decimal TotalInterestPaid { get; set; }
        public decimal MonthlyPayment { get; set; }
        public int MonthCount { get; set; }
    }
}
