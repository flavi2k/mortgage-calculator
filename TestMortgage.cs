using MortgageWebApp.Models;
using MortgageWebApp.Services;

// Test the mortgage calculation engine directly
var engine = new MortgageCalculationEngine();

// Test basic mortgage calculation
var mortgageDetails = new MortgageDetails
{
    LoanAmount = 300000,
    AnnualInterestRate = 4.5m,
    LoanTermYears = 30
};

try
{
    var monthlyPayment = engine.CalculateMonthlyPayment(mortgageDetails);
    Console.WriteLine($"Monthly Payment: ${monthlyPayment:F2}");
    
    var schedule = engine.GenerateAmortizationSchedule(mortgageDetails);
    Console.WriteLine($"Total Payments: {schedule.Count}");
    Console.WriteLine($"First Payment: ${schedule[0].TotalPayment:F2}");
    Console.WriteLine($"Last Payment: ${schedule[schedule.Count-1].TotalPayment:F2}");
    
    Console.WriteLine("Test completed successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
