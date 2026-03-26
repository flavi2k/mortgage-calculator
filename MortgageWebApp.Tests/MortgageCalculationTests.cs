using MortgageWebApp.Models;
using MortgageWebApp.Services;

namespace MortgageWebApp.Tests;

public class MortgageCalculationTests
{
    private readonly MortgageCalculationEngine _engine = new();

    [Fact]
    public void CalculateMonthlyPayment_BasicMortgage_ReturnsCorrectValue()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 215000,
            AnnualInterestRate = 5.05m,
            LoanTermYears = 23
        };

        var result = _engine.CalculateMonthlyPayment(details);

        Assert.InRange(result, 1318m, 1320m);
    }

    [Fact]
    public void CalculateMonthlyPayment_300000At4_5PercentFor30Years_Returns1520()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 300000,
            AnnualInterestRate = 4.5m,
            LoanTermYears = 30
        };

        var result = _engine.CalculateMonthlyPayment(details);

        Assert.InRange(result, 1519m, 1522m);
    }

    [Fact]
    public void GenerateAmortizationSchedule_23YearTerm_Returns276Payments()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 215000,
            AnnualInterestRate = 5.05m,
            LoanTermYears = 23
        };

        var schedule = _engine.GenerateAmortizationSchedule(details);

        Assert.Equal(276, schedule.Count);
    }

    [Fact]
    public void GenerateAmortizationSchedule_30YearTerm_Returns360Payments()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 300000,
            AnnualInterestRate = 4.5m,
            LoanTermYears = 30
        };

        var schedule = _engine.GenerateAmortizationSchedule(details);

        Assert.Equal(360, schedule.Count);
    }

    [Fact]
    public void GenerateAmortizationSchedule_FirstPayment_HasPrincipalAndInterest()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 215000,
            AnnualInterestRate = 5.05m,
            LoanTermYears = 23
        };

        var schedule = _engine.GenerateAmortizationSchedule(details);
        var first = schedule[0];

        var total = first.PrincipalPayment + first.InterestPayment;
        Assert.InRange(total, 1318m, 1320m);
        Assert.True(first.PrincipalPayment > 0);
        Assert.True(first.InterestPayment > 0);
    }

    [Fact]
    public void GenerateAmortizationSchedule_LastPayment_HasZeroBalance()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 215000,
            AnnualInterestRate = 5.05m,
            LoanTermYears = 23
        };

        var schedule = _engine.GenerateAmortizationSchedule(details);
        var last = schedule[schedule.Count - 1];

        Assert.Equal(0, last.RemainingBalance);
    }

    [Fact]
    public void CalculateExtraPaymentScenario_WithExtraPayment_SavesInterest()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 215000,
            AnnualInterestRate = 5.05m,
            LoanTermYears = 23,
            ExtraMonthlyPayment = 200
        };

        var result = _engine.CalculateExtraPaymentScenario(details);

        Assert.True(result.TotalInterestSaved > 0);
        Assert.True(result.MonthsSaved > 0);
    }

    [Fact]
    public void CalculateExtraPaymentScenario_ZeroExtraPayment_ZeroSavings()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 215000,
            AnnualInterestRate = 5.05m,
            LoanTermYears = 23,
            ExtraMonthlyPayment = 0
        };

        var result = _engine.CalculateExtraPaymentScenario(details);

        Assert.Equal(0, result.MonthsSaved);
    }

    [Fact]
    public void GenerateAmortizationScheduleWithExtraPayments_FasterPayoff()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 215000,
            AnnualInterestRate = 5.05m,
            LoanTermYears = 23
        };

        var standardSchedule = _engine.GenerateAmortizationSchedule(details);
        var extraSchedule = _engine.GenerateAmortizationScheduleWithExtraPayments(details, 200);

        Assert.True(extraSchedule.Count < standardSchedule.Count);
    }

    [Fact]
    public void GenerateAmortizationScheduleWithOneTimePayment_AtMonth12_ReducesBalance()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 215000,
            AnnualInterestRate = 5.05m,
            LoanTermYears = 23
        };

        var standardSchedule = _engine.GenerateAmortizationSchedule(details);
        var oneTimeSchedule = _engine.GenerateAmortizationScheduleWithOneTimeExtraPayment(details, 10000, 12);

        Assert.True(oneTimeSchedule[11].RemainingBalance < standardSchedule[11].RemainingBalance);
    }

    [Fact]
    public void CalculateMonthlyPayment_ZeroInterestRate_DividesEvenly()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 120000,
            AnnualInterestRate = 0,
            LoanTermYears = 10
        };

        var result = _engine.CalculateMonthlyPayment(details);

        Assert.Equal(1000m, result);
    }

    [Fact]
    public void CalculateMonthlyPayment_InvalidLoanAmount_ThrowsException()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 0,
            AnnualInterestRate = 5,
            LoanTermYears = 23
        };

        Assert.Throws<ArgumentException>(() => _engine.CalculateMonthlyPayment(details));
    }

    [Fact]
    public void CalculateMonthlyPayment_NegativeInterest_ThrowsException()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 215000,
            AnnualInterestRate = -5,
            LoanTermYears = 23
        };

        Assert.Throws<ArgumentException>(() => _engine.CalculateMonthlyPayment(details));
    }

    [Fact]
    public void CalculateMonthlyPayment_ZeroTerm_ThrowsException()
    {
        var details = new MortgageDetails
        {
            LoanAmount = 215000,
            AnnualInterestRate = 5.05m,
            LoanTermYears = 0
        };

        Assert.Throws<ArgumentException>(() => _engine.CalculateMonthlyPayment(details));
    }

    [Fact]
    public void CalculatePortfolio_SinglePeriod_ReturnsCorrectSchedule()
    {
        var periods = new List<MortgagePeriod>
        {
            new MortgagePeriod
            {
                LoanAmount = 200000,
                AnnualInterestRate = 5.0m,
                LoanTermYears = 25,
                StartDate = new DateTime(2024, 1, 1),
                EarlyRepaymentCapPercent = 10m,
                ExtraPayments = new List<ExtraPaymentEntry>()
            }
        };

        var result = _engine.CalculatePortfolio(periods);

        Assert.NotNull(result);
        Assert.Equal(300, result.CombinedSchedule.Count);
        Assert.True(result.TotalInterestPaid > 0);
    }

    [Fact]
    public void CalculatePortfolio_TwoPeriods_SequentialBalanceTransfer()
    {
        var periods = new List<MortgagePeriod>
        {
            new MortgagePeriod
            {
                LoanAmount = 200000,
                AnnualInterestRate = 5.0m,
                LoanTermYears = 2,
                StartDate = new DateTime(2024, 1, 1),
                EarlyRepaymentCapPercent = 10m,
                ExtraPayments = new List<ExtraPaymentEntry>()
            },
            new MortgagePeriod
            {
                LoanAmount = 180000,
                AnnualInterestRate = 4.5m,
                LoanTermYears = 23,
                StartDate = new DateTime(2026, 1, 1),
                EarlyRepaymentCapPercent = 10m,
                ExtraPayments = new List<ExtraPaymentEntry>()
            }
        };

        var result = _engine.CalculatePortfolio(periods);

        Assert.NotNull(result);
        Assert.Equal(2, result.PeriodResults.Count);
        Assert.True(result.CombinedSchedule.Count > 0);
    }

    [Fact]
    public void CalculatePortfolio_WithExtraPayments_VariablePerMonth()
    {
        var periods = new List<MortgagePeriod>
        {
            new MortgagePeriod
            {
                LoanAmount = 200000,
                AnnualInterestRate = 5.0m,
                LoanTermYears = 25,
                StartDate = new DateTime(2024, 1, 1),
                EarlyRepaymentCapPercent = 10m,
                ExtraPayments = new List<ExtraPaymentEntry>
                {
                    new ExtraPaymentEntry { MonthNumber = 1, Amount = 300 },
                    new ExtraPaymentEntry { MonthNumber = 5, Amount = 1400 }
                }
            }
        };

        var result = _engine.CalculatePortfolio(periods);

        Assert.NotNull(result);
        var month1 = result.CombinedSchedule.FirstOrDefault(p => p.PaymentNumber == 1);
        Assert.NotNull(month1);
        Assert.Equal(300, month1.ExtraPayment);
    }

    [Fact]
    public void CalculatePortfolio_Calculates10PercentCap()
    {
        var periods = new List<MortgagePeriod>
        {
            new MortgagePeriod
            {
                LoanAmount = 200000,
                AnnualInterestRate = 5.0m,
                LoanTermYears = 25,
                StartDate = new DateTime(2024, 1, 1),
                EarlyRepaymentCapPercent = 10m,
                ExtraPayments = new List<ExtraPaymentEntry>()
            }
        };

        var result = _engine.CalculatePortfolio(periods);

        Assert.NotNull(result);
        var firstPayment = result.CombinedSchedule.FirstOrDefault();
        Assert.NotNull(firstPayment);
        decimal expectedMaxCap = (200000m * 0.10m / 12m) + firstPayment.TotalPayment;
        Assert.Equal(expectedMaxCap, firstPayment.MaxCapPayment, 2);
    }

    [Fact]
    public void CalculatePortfolio_ExtraPaymentExceedsCap_IsOverCap()
    {
        var periods = new List<MortgagePeriod>
        {
            new MortgagePeriod
            {
                LoanAmount = 100000,
                AnnualInterestRate = 5.0m,
                LoanTermYears = 25,
                StartDate = new DateTime(2024, 1, 1),
                EarlyRepaymentCapPercent = 10m,
                ExtraPayments = new List<ExtraPaymentEntry>
                {
                    new ExtraPaymentEntry { MonthNumber = 1, Amount = 2000 }
                }
            }
        };

        var result = _engine.CalculatePortfolio(periods);

        Assert.NotNull(result);
        var month1 = result.CombinedSchedule.FirstOrDefault(p => p.PaymentNumber == 1);
        Assert.NotNull(month1);
        Assert.True(month1.IsOverCap);
    }

    [Fact]
    public void CalculatePortfolio_ExtraPaymentUnderCap_NotOverCap()
    {
        var periods = new List<MortgagePeriod>
        {
            new MortgagePeriod
            {
                LoanAmount = 100000,
                AnnualInterestRate = 5.0m,
                LoanTermYears = 25,
                StartDate = new DateTime(2024, 1, 1),
                EarlyRepaymentCapPercent = 10m,
                ExtraPayments = new List<ExtraPaymentEntry>
                {
                    new ExtraPaymentEntry { MonthNumber = 1, Amount = 100 }
                }
            }
        };

        var result = _engine.CalculatePortfolio(periods);

        Assert.NotNull(result);
        var month1 = result.CombinedSchedule.FirstOrDefault(p => p.PaymentNumber == 1);
        Assert.NotNull(month1);
        Assert.False(month1.IsOverCap);
    }

    [Fact]
    public void CalculatePortfolio_EmptyPeriods_ThrowsException()
    {
        var periods = new List<MortgagePeriod>();
        Assert.Throws<ArgumentException>(() => _engine.CalculatePortfolio(periods));
    }
}
