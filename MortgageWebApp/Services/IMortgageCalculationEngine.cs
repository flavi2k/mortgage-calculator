using MortgageWebApp.Models;

namespace MortgageWebApp.Services
{
    public interface IMortgageCalculationEngine
    {
        decimal CalculateMonthlyPayment(MortgageDetails mortgageDetails);
        decimal CalculateMonthlyPaymentForPeriod(decimal loanAmount, decimal annualInterestRate, int termYears);
        List<PaymentSchedule> GenerateAmortizationSchedule(MortgageDetails mortgageDetails);
        List<PaymentSchedule> GenerateAmortizationScheduleWithExtraPayments(MortgageDetails mortgageDetails, decimal extraMonthlyPayment);
        List<PaymentSchedule> GenerateAmortizationScheduleWithOneTimeExtraPayment(MortgageDetails mortgageDetails, decimal oneTimeExtraPayment, int extraPaymentMonth);
        ExtraPaymentScenario CalculateExtraPaymentScenario(MortgageDetails mortgageDetails);
        PortfolioCalculationResult CalculatePortfolio(List<MortgagePeriod> periods);
        decimal GetProjectedBalanceAfterFixedPeriod(decimal loanAmount, decimal annualInterestRate, int termYears, int fixedPeriodYears);
    }
}
