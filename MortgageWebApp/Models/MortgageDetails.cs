using System.ComponentModel.DataAnnotations;

namespace MortgageWebApp.Models
{
    public class MortgageDetails
    {
        [Required]
        public decimal LoanAmount { get; set; }
        
        [Required]
        public decimal AnnualInterestRate { get; set; }
        
        [Required]
        public int LoanTermYears { get; set; }
        
        public decimal ExtraMonthlyPayment { get; set; }
        public decimal OneTimeExtraPayment { get; set; }
        public int ExtraPaymentMonth { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
    }
}
