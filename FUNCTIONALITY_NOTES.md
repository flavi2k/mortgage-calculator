# Mortgage Calculator - Functionality Notes

## Overview
A full-featured mortgage calculator web application built with ASP.NET Core 8.0 backend and vanilla JavaScript frontend. Currently supports multi-period portfolio tracking with remortgages.

## Current Features

### 1. Portfolio Mortgage Input Fields
- **Add Mortgage Period** button to add multiple periods (initial + remortgages)
- For each period:
  - **Start Date** - When the mortgage period begins
  - **Fixed Period (years)** - 2, 5, or 10 years fixed rate
  - **Loan Amount (£)** - Default: 226,800
  - **Interest Rate (%)** - Default: 2.75% for first period, 5.05% for remortgages
  - **Term (years)** - Default: 25
  - **Early Repayment Cap (%)** - Default: 10%
- **Property Value (£)** - Default: 300,000 (for LTV calculations)

### 2. Portfolio Amortization Schedule Table
Full payment breakdown table with pagination showing:
- **#** - Payment number (continuous)
- **Month** - Month name and year (e.g., "Sept 2022")
- **Date** - Payment date
- **Payment** - Base monthly payment
- **Extra Payment** - Editable extra payment field
- **Total Payment** - Base + Extra payment
- **Principal** - Principal portion
- **Interest** - Interest portion
- **Balance** - Remaining balance
- **LTV %** - Loan to Value percentage
- **Year** - Year number
- **Max 10%** - Maximum allowed at 10% cap
- **Over Cap** - "OVER" or "OK" indicator

### 3. Extra Payments in Table
- Edit extra payments directly in the table
- Type amount in the "Extra Payment" column
- Click "Recalculate" to apply changes
- Extra payments are preserved when recalculating

### 4. Period Transitions
- Visual separator row shows when new remortgage period starts
- Payment changes at the fixed period boundary
- Example: 2-year fixed → new payment from month 25

### 5. Portfolio Summary
- Total Interest Paid
- Total Payment
- Total Months
- Interest Saved (vs single 25-year term)

### 6. Save/Load Portfolio
- Save Portfolio - Stores to localStorage
- Load Portfolio - Restores saved portfolio
- Clear Saved - Removes saved data

---

## Technical Stack
- **Backend**: ASP.NET Core 8.0, C#
- **Frontend**: Vanilla HTML/CSS/JavaScript
- **Icons**: Font Awesome
- **Storage**: localStorage for Save/Load

## API Endpoints
- `POST /api/mortgage/calculate-monthly-payment`
- `POST /api/mortgage/amortization-schedule`
- `POST /api/mortgage/amortization-schedule-extra`
- `POST /api/mortgage/extra-payment-scenario`
- `POST /api/mortgage/amortization-schedule-one-time`
- `POST /api/mortgage/calculate-portfolio` - Portfolio calculation with multiple periods

## Backend Models
- **ExtraPaymentEntry**: monthNumber, amount
- **MortgagePeriod**: loanAmount, annualInterestRate, loanTermYears, startDate, fixedPeriodYears, earlyRepaymentCapPercent, extraPayments
- **PortfolioCalculationResult**: combinedSchedule, totalInterestPaid, totalAmountPaid, totalMonths, periodResults

## Running the App
```bash
cd MortgageWebApp
dotnet run --urls "http://localhost:5001"
```

## Portfolio Calculation Logic
- **Single period**: Full 25-year term (300 payments)
- **Two periods**: First period uses fixed months (e.g., 24 months for 2-year), second period uses remaining months to complete total term (276 months)
- **Three+ periods**: Each period uses fixed months, last period uses remaining months to reach total term

## Unit Tests
Run with: `cd MortgageWebApp.Tests && dotnet test`

## E2E Tests
Run with: `node e2e_tests.js`
