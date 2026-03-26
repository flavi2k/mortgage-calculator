using System.ComponentModel.DataAnnotations;

namespace MortgageWebApp.Models
{
    public class PaymentSchedule
    {
        public int PaymentNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal PrincipalPayment { get; set; }
        public decimal InterestPayment { get; set; }
        public decimal RemainingBalance { get; set; }
        public int YearNumber { get; set; }
        public decimal MaxCapPayment { get; set; }
        public bool IsOverCap { get; set; }
        public decimal ExtraPayment { get; set; }
    }
}
