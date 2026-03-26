using System.ComponentModel.DataAnnotations;

namespace MortgageWebApp.Models
{
    public class MortgagePeriod
    {
        [Required]
        public decimal LoanAmount { get; set; }
        
        [Required]
        public decimal AnnualInterestRate { get; set; }
        
        [Required]
        public int LoanTermYears { get; set; }
        
        public DateTime StartDate { get; set; } = DateTime.Now;
        
        public int FixedPeriodYears { get; set; } = 5;
        
        public decimal EarlyRepaymentCapPercent { get; set; } = 10m;
        
        public List<ExtraPaymentEntry> ExtraPayments { get; set; } = new();
    }
}
