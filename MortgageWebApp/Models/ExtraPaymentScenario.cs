namespace MortgageWebApp.Models
{
    public class ExtraPaymentScenario
    {
        public decimal OriginalMonthlyPayment { get; set; }
        public decimal NewMonthlyPayment { get; set; }
        public int OriginalTermMonths { get; set; }
        public int NewTermMonths { get; set; }
        public decimal MonthsSaved { get; set; }
        public decimal TotalInterestSaved { get; set; }
        public decimal TotalInterestPaid { get; set; }
    }
}
