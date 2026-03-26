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

            // Handle case where interest rate is 0
            if (monthlyInterestRate == 0)
            {
                return mortgageDetails.LoanAmount / numberOfPayments;
            }

            // Standard mortgage formula: M = P * [r(1+r)^n] / [(1+r)^n - 1]
            decimal payment = mortgageDetails.LoanAmount * 
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
            DateTime paymentDate = DateTime.Now;

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
            DateTime paymentDate = DateTime.Now;

            for (int i = 1; i <= numberOfPayments; i++)
            {
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
            DateTime paymentDate = DateTime.Now;

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
    }
}
