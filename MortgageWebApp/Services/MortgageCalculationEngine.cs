using MortgageWebApp.Models;

namespace MortgageWebApp.Services
{
    public class MortgageCalculationEngine : IMortgageCalculationEngine
    {
        public decimal CalculateMonthlyPayment(MortgageDetails mortgageDetails)
        {
            if (mortgageDetails.LoanAmount <= 0)
                throw new ArgumentException("Loan amount must be greater than zero");
            if (mortgageDetails.AnnualInterestRate < 0)
                throw new ArgumentException("Interest rate cannot be negative");
            if (mortgageDetails.LoanTermYears <= 0)
                throw new ArgumentException("Loan term must be greater than zero");

            decimal monthlyInterestRate = mortgageDetails.AnnualInterestRate / 100 / 12;
            int numberOfPayments = mortgageDetails.LoanTermYears * 12;

            if (monthlyInterestRate == 0)
            {
                return mortgageDetails.LoanAmount / numberOfPayments;
            }

            decimal payment = mortgageDetails.LoanAmount * 
                             (monthlyInterestRate * (decimal)Math.Pow((double)(1 + monthlyInterestRate), numberOfPayments)) / 
                             ((decimal)Math.Pow((double)(1 + monthlyInterestRate), numberOfPayments) - 1);

            return Math.Round(payment, 2);
        }

        public decimal CalculateMonthlyPaymentForPeriod(decimal loanAmount, decimal annualInterestRate, int termYears)
        {
            if (loanAmount <= 0)
                throw new ArgumentException("Loan amount must be greater than zero");
            if (annualInterestRate < 0)
                throw new ArgumentException("Interest rate cannot be negative");
            if (termYears <= 0)
                throw new ArgumentException("Loan term must be greater than zero");

            decimal monthlyInterestRate = annualInterestRate / 100 / 12;
            int numberOfPayments = termYears * 12;

            if (monthlyInterestRate == 0)
            {
                return loanAmount / numberOfPayments;
            }

            decimal payment = loanAmount * 
                             (monthlyInterestRate * (decimal)Math.Pow((double)(1 + monthlyInterestRate), numberOfPayments)) / 
                             ((decimal)Math.Pow((double)(1 + monthlyInterestRate), numberOfPayments) - 1);

            return Math.Round(payment, 2);
        }

        public List<PaymentSchedule> GenerateAmortizationSchedule(MortgageDetails mortgageDetails)
        {
            if (mortgageDetails.LoanAmount <= 0)
                throw new ArgumentException("Loan amount must be greater than zero");
            if (mortgageDetails.AnnualInterestRate < 0)
                throw new ArgumentException("Interest rate cannot be negative");
            if (mortgageDetails.LoanTermYears <= 0)
                throw new ArgumentException("Loan term must be greater than zero");

            var schedule = new List<PaymentSchedule>();
            decimal balance = mortgageDetails.LoanAmount;
            decimal monthlyInterestRate = mortgageDetails.AnnualInterestRate / 100 / 12;
            int numberOfPayments = mortgageDetails.LoanTermYears * 12;
            decimal monthlyPayment = CalculateMonthlyPayment(mortgageDetails);
            DateTime paymentDate = mortgageDetails.StartDate.AddMonths(1);

            for (int i = 1; i <= numberOfPayments; i++)
            {
                decimal interestPayment = balance * monthlyInterestRate;
                decimal principalPayment = monthlyPayment - interestPayment;
                balance -= principalPayment;

                if (balance < 0)
                    balance = 0;

                schedule.Add(new PaymentSchedule
                {
                    PaymentNumber = i,
                    PaymentDate = paymentDate,
                    TotalPayment = monthlyPayment,
                    PrincipalPayment = principalPayment,
                    InterestPayment = interestPayment,
                    RemainingBalance = balance
                });

                paymentDate = paymentDate.AddMonths(1);
            }

            return schedule;
        }

        public List<PaymentSchedule> GenerateAmortizationScheduleWithExtraPayments(MortgageDetails mortgageDetails, decimal extraMonthlyPayment)
        {
            if (mortgageDetails.LoanAmount <= 0)
                throw new ArgumentException("Loan amount must be greater than zero");
            if (mortgageDetails.AnnualInterestRate < 0)
                throw new ArgumentException("Interest rate cannot be negative");
            if (mortgageDetails.LoanTermYears <= 0)
                throw new ArgumentException("Loan term must be greater than zero");

            var schedule = new List<PaymentSchedule>();
            decimal balance = mortgageDetails.LoanAmount;
            decimal monthlyInterestRate = mortgageDetails.AnnualInterestRate / 100 / 12;
            int numberOfPayments = mortgageDetails.LoanTermYears * 12;
            decimal monthlyPayment = CalculateMonthlyPayment(mortgageDetails);
            DateTime paymentDate = mortgageDetails.StartDate.AddMonths(1);

            for (int i = 1; i <= numberOfPayments; i++)
            {
                if (balance <= 0) break;
                
                decimal interestPayment = balance * monthlyInterestRate;
                decimal principalPayment = monthlyPayment - interestPayment;
                decimal totalExtraPayment = extraMonthlyPayment;
                decimal totalPayment = monthlyPayment + totalExtraPayment;

                // Apply extra payment to principal
                balance -= principalPayment + totalExtraPayment;

                if (balance < 0)
                {
                    totalPayment = monthlyPayment + principalPayment;
                    balance = 0;
                }

                schedule.Add(new PaymentSchedule
                {
                    PaymentNumber = i,
                    PaymentDate = paymentDate,
                    TotalPayment = totalPayment,
                    PrincipalPayment = principalPayment + totalExtraPayment,
                    InterestPayment = interestPayment,
                    RemainingBalance = balance
                });

                paymentDate = paymentDate.AddMonths(1);
            }

            return schedule;
        }

        public List<PaymentSchedule> GenerateAmortizationScheduleWithOneTimeExtraPayment(MortgageDetails mortgageDetails, decimal oneTimeExtraPayment, int extraPaymentMonth)
        {
            if (mortgageDetails.LoanAmount <= 0)
                throw new ArgumentException("Loan amount must be greater than zero");
            if (mortgageDetails.AnnualInterestRate < 0)
                throw new ArgumentException("Interest rate cannot be negative");
            if (mortgageDetails.LoanTermYears <= 0)
                throw new ArgumentException("Loan term must be greater than zero");

            var schedule = new List<PaymentSchedule>();
            decimal balance = mortgageDetails.LoanAmount;
            decimal monthlyInterestRate = mortgageDetails.AnnualInterestRate / 100 / 12;
            int numberOfPayments = mortgageDetails.LoanTermYears * 12;
            decimal monthlyPayment = CalculateMonthlyPayment(mortgageDetails);
            DateTime paymentDate = mortgageDetails.StartDate.AddMonths(1);

            for (int i = 1; i <= numberOfPayments; i++)
            {
                decimal interestPayment = balance * monthlyInterestRate;
                decimal principalPayment = monthlyPayment - interestPayment;
                decimal totalExtraPayment = 0;

                // Apply one-time extra payment in specified month
                if (i == extraPaymentMonth)
                {
                    totalExtraPayment = oneTimeExtraPayment;
                    balance -= totalExtraPayment;
                }

                balance -= principalPayment;

                if (balance < 0)
                    balance = 0;

                schedule.Add(new PaymentSchedule
                {
                    PaymentNumber = i,
                    PaymentDate = paymentDate,
                    TotalPayment = monthlyPayment + totalExtraPayment,
                    PrincipalPayment = principalPayment + totalExtraPayment,
                    InterestPayment = interestPayment,
                    RemainingBalance = balance
                });

                paymentDate = paymentDate.AddMonths(1);
            }

            return schedule;
        }

        public ExtraPaymentScenario CalculateExtraPaymentScenario(MortgageDetails mortgageDetails)
        {
            if (mortgageDetails.LoanAmount <= 0)
                throw new ArgumentException("Loan amount must be greater than zero");
            if (mortgageDetails.AnnualInterestRate < 0)
                throw new ArgumentException("Interest rate cannot be negative");
            if (mortgageDetails.LoanTermYears <= 0)
                throw new ArgumentException("Loan term must be greater than zero");

            var originalPayment = CalculateMonthlyPayment(mortgageDetails);
            var originalSchedule = GenerateAmortizationSchedule(mortgageDetails);
            var newSchedule = GenerateAmortizationScheduleWithExtraPayments(mortgageDetails, mortgageDetails.ExtraMonthlyPayment);
            
            int originalTermMonths = mortgageDetails.LoanTermYears * 12;
            int newTermMonths = newSchedule.Count;
            decimal monthsSaved = originalTermMonths - newTermMonths;
            decimal totalInterestPaid = newSchedule.Sum(p => p.InterestPayment);
            decimal originalTotalInterest = originalSchedule.Sum(p => p.InterestPayment);
            decimal totalInterestSaved = originalTotalInterest - totalInterestPaid;

            return new ExtraPaymentScenario
            {
                OriginalMonthlyPayment = originalPayment,
                NewMonthlyPayment = originalPayment + mortgageDetails.ExtraMonthlyPayment,
                OriginalTermMonths = originalTermMonths,
                NewTermMonths = newTermMonths,
                MonthsSaved = Math.Round(monthsSaved, 2),
                TotalInterestSaved = Math.Round(totalInterestSaved, 2),
                TotalInterestPaid = Math.Round(totalInterestPaid, 2)
            };
        }

        public PortfolioCalculationResult CalculatePortfolio(List<MortgagePeriod> periods)
        {
            if (periods == null || periods.Count == 0)
                throw new ArgumentException("At least one mortgage period is required");

            var result = new PortfolioCalculationResult();
            var combinedSchedule = new List<PaymentSchedule>();
            decimal balance = 0;
            int globalPaymentNumber = 1;

            for (int periodIndex = 0; periodIndex < periods.Count; periodIndex++)
            {
                var period = periods[periodIndex];
                
                var periodSchedule = GenerateAmortizationScheduleWithExtraPaymentsAndCap(
                    period.LoanAmount,
                    period.AnnualInterestRate,
                    period.LoanTermYears,
                    period.StartDate,
                    period.EarlyRepaymentCapPercent,
                    period.ExtraPayments,
                    balance,
                    globalPaymentNumber);

                balance = 0;
                foreach (var payment in periodSchedule)
                {
                    if (payment.RemainingBalance <= 0)
                    {
                        balance = 0;
                        break;
                    }
                    balance = payment.RemainingBalance;
                }

                var periodResult = new MortgagePeriodResult
                {
                    PeriodIndex = periodIndex,
                    StartDate = period.StartDate,
                    OriginalLoanAmount = period.LoanAmount,
                    MonthlyPayment = CalculateMonthlyPaymentForPeriod(period.LoanAmount, period.AnnualInterestRate, period.LoanTermYears),
                    MonthCount = periodSchedule.Count,
                    TotalInterestPaid = periodSchedule.Sum(p => p.InterestPayment),
                    FinalBalance = balance
                };
                
                if (periodSchedule.Any())
                {
                    periodResult.EndDate = periodSchedule.Last().PaymentDate;
                }

                result.PeriodResults.Add(periodResult);
                combinedSchedule.AddRange(periodSchedule);
                globalPaymentNumber += periodSchedule.Count;
            }

            result.CombinedSchedule = combinedSchedule;
            result.TotalInterestPaid = combinedSchedule.Sum(p => p.InterestPayment);
            result.TotalAmountPaid = combinedSchedule.Sum(p => p.TotalPayment);
            result.TotalMonths = combinedSchedule.Count;

            if (periods.Count > 0)
            {
                var baselineDetails = new MortgageDetails
                {
                    LoanAmount = periods[0].LoanAmount,
                    AnnualInterestRate = periods[0].AnnualInterestRate,
                    LoanTermYears = periods.Sum(p => p.LoanTermYears)
                };
                var baselineSchedule = GenerateAmortizationSchedule(baselineDetails);
                var baselineInterest = baselineSchedule.Sum(p => p.InterestPayment);
                result.TotalInterestSaved = baselineInterest - result.TotalInterestPaid;
                result.MonthsSaved = baselineSchedule.Count - result.TotalMonths;
            }

            return result;
        }

        private List<PaymentSchedule> GenerateAmortizationScheduleWithExtraPaymentsAndCap(
            decimal loanAmount,
            decimal annualInterestRate,
            int termYears,
            DateTime startDate,
            decimal earlyRepaymentCapPercent,
            List<ExtraPaymentEntry> extraPayments,
            decimal carryOverBalance,
            int startPaymentNumber)
        {
            var schedule = new List<PaymentSchedule>();
            decimal balance = carryOverBalance > 0 ? carryOverBalance : loanAmount;
            decimal monthlyInterestRate = annualInterestRate / 100 / 12;
            int numberOfPayments = termYears * 12;
            decimal monthlyPayment = CalculateMonthlyPaymentForPeriod(loanAmount, annualInterestRate, termYears);
            
            decimal maxAnnualRepayment = loanAmount * (earlyRepaymentCapPercent / 100);
            decimal maxMonthlyCap = maxAnnualRepayment / 12;
            
            DateTime paymentDate = startDate.AddMonths(1);
            int currentYear = paymentDate.Year;
            decimal yearTotalPaid = 0;

            var extraPaymentMap = extraPayments.ToDictionary(e => e.MonthNumber, e => e.Amount);

            for (int i = 1; i <= numberOfPayments; i++)
            {
                if (balance <= 0) break;

                int paymentYear = paymentDate.Year;
                if (paymentYear != currentYear)
                {
                    yearTotalPaid = 0;
                    currentYear = paymentYear;
                }

                decimal interestPayment = balance * monthlyInterestRate;
                decimal principalPayment = monthlyPayment - interestPayment;
                
                decimal extraPayment = extraPaymentMap.ContainsKey(i) ? extraPaymentMap[i] : 0;
                decimal totalPayment = monthlyPayment + extraPayment;
                
                yearTotalPaid += extraPayment;
                decimal maxCapPayment = maxMonthlyCap + monthlyPayment;
                bool isOverCap = extraPayment > maxMonthlyCap;

                balance -= principalPayment + extraPayment;

                if (balance < 0)
                {
                    totalPayment = monthlyPayment + principalPayment + extraPayment;
                    balance = 0;
                }

                int yearNumber = paymentDate.Year - startDate.Year + 1;

                schedule.Add(new PaymentSchedule
                {
                    PaymentNumber = startPaymentNumber + i - 1,
                    PaymentDate = paymentDate,
                    TotalPayment = totalPayment,
                    PrincipalPayment = principalPayment + extraPayment,
                    InterestPayment = interestPayment,
                    RemainingBalance = balance,
                    YearNumber = yearNumber,
                    MaxCapPayment = Math.Round(maxCapPayment, 2),
                    IsOverCap = isOverCap,
                    ExtraPayment = extraPayment
                });

                paymentDate = paymentDate.AddMonths(1);
            }

            return schedule;
        }

        public decimal GetProjectedBalanceAfterFixedPeriod(decimal loanAmount, decimal annualInterestRate, int termYears, int fixedPeriodYears)
        {
            if (fixedPeriodYears <= 0 || fixedPeriodYears > termYears)
            {
                return loanAmount;
            }

            int fixedPeriodMonths = fixedPeriodYears * 12;
            decimal monthlyInterestRate = annualInterestRate / 100 / 12;
            int totalMonths = termYears * 12;

            decimal monthlyPayment;
            if (monthlyInterestRate == 0)
            {
                monthlyPayment = loanAmount / totalMonths;
            }
            else
            {
                monthlyPayment = loanAmount *
                    (monthlyInterestRate * (decimal)Math.Pow((double)(1 + monthlyInterestRate), totalMonths)) /
                    ((decimal)Math.Pow((double)(1 + monthlyInterestRate), totalMonths) - 1);
            }

            decimal balance = loanAmount;
            for (int i = 0; i < fixedPeriodMonths; i++)
            {
                decimal interestPayment = balance * monthlyInterestRate;
                decimal principalPayment = monthlyPayment - interestPayment;
                balance -= principalPayment;

                if (balance <= 0)
                {
                    return 0;
                }
            }

            return Math.Round(balance, 2);
        }
    }
}
