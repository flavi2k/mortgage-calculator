namespace MortgageWebApp.Models
{
    public class MortgageCalculationResult
    {
        public decimal MonthlyPayment { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalPayment { get; set; }
        public int TotalMonths { get; set; }
    }
}
