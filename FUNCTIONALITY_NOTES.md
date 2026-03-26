# Mortgage Calculator - Existing Functionality

## Overview
A full-featured mortgage calculator web application built with ASP.NET Core 8.0 backend and vanilla JavaScript frontend. Supports both single mortgages and multi-period portfolio tracking with remortgages.

## Current Features

### 1. Loan Input Fields (Single Mortgage Tab)
- **Loan Amount (£)** - Default: 215,000 (formatted with £ digit separators)
- **Annual Interest Rate (%)** - Default: 5.05
- **Loan Term (years)** - Default: 23
- **Monthly Extra Payment (£)** - Optional recurring extra payment
- **One-Time Extra Payment (£)** - Single lump sum payment
- **Apply One-Time Payment In Month** - Which month to apply lump sum (default: 12)
- **Property Value (£)** - Default: 300,000 (for LTV calculations)

### 2. Results Display
- Monthly Payment
- Total Interest Paid
- Total Payment (principal + interest)
- Term Reduction (months saved with extra payments)

### 3. Amortization Schedule
- Full payment breakdown table with pagination (12 payments per page)
- Shows: Payment #, Date, Payment, Principal, Interest, Balance

### 4. Payment Comparison & Savings
- Side-by-side comparison of:
  - Standard scenario (no extras)
  - Monthly extra payment scenario (interest saved, time saved)
  - One-time payment scenario (interest saved, time saved)

### 5. Payment Visualization Charts (Chart.js)
- **Pie/Doughnut Chart**: Principal vs Interest breakdown
- **Stacked Bar Chart**: Annual principal vs interest over time

### 6. LTV & Remortgage Advisor
- **Current LTV** (Loan-to-Value percentage)
- **Property Value** display
- **Remaining Balance**
- **Equity** amount
- **Advisor Recommendations**:
  - High LTV (>75%): Warning to reach 75% for better rates
  - Moderate LTV (60-75%): Encourage reaching 60%
  - Excellent LTV (<60%): Qualify for best rates

### 7. Financial Advisor
- Payment strategy analysis
- Interest rate comparison (vs ~4.5% market rate)
- Risk assessment (stress test for rate increases)
- Optimal extra payment recommendations

### 8. Save/Load
- **Save** button: Stores calculation to localStorage
- **Load** button: Restores saved calculation
- **Clear Saved** button: Removes saved data

### 9. Print/PDF Export
- Print button with print-friendly CSS
- All cards print properly
- Pagination hidden in print view

---

## Portfolio / Multi-Period Mortgage Features

### 10. Portfolio Tab
New tab for tracking multiple mortgage periods (initial + remortgages) with full amortization across all periods.

#### 10.1 Fixed Period Selection
- Selectable fixed period: **2 years**, **5 years**, or **10 years**
- Standard UK mortgage fixed-rate periods
- Used to calculate when next remortgage period begins

#### 10.2 Auto-Calculated Start Dates
- When adding a new remortgage period:
  - Start date = Previous period start date + Previous period term (years)
- Example: Initial mortgage starts Jan 2024, term 25 years → Remortgage 1 starts Jan 2049

#### 10.3 Auto-Calculated Loan Amount
- New remortgage loan amount defaults to **90% of previous loan amount**
- Assumes 10% principal paid down
- Updates automatically when previous period loan amount changes

#### 10.4 Variable Extra Payments Per Month
- Add extra payments for specific months (not just a fixed monthly amount)
- Example use cases:
  - January: £300 extra
  - May: £1400 extra (bonus month)
- Each extra payment is applied in the specified month
- Remove individual rows as needed

#### 10.5 10% Early Repayment Cap Tracking
- **Configurable per period**: Default 10%, can be changed (e.g., 5%, 15%, 25%)
- Shows in amortization table:
  - **Max 10% Payment**: What the payment would be at the cap
  - **Over Cap indicator**: Shows "OVER" or "OK" based on actual extra payment
- Resets annually per calendar year (UK standard practice)

#### 10.6 Portfolio Save/Load
- **Save Portfolio**: Saves all periods and extra payments
- **Load Portfolio**: Restores saved portfolio and recalculates
- Separate from single mortgage save/load

### 11. Portfolio Amortization Table
Combined schedule across all periods showing:
- Payment # (continuous from start)
- Date
- Payment
- Principal
- Interest
- Balance
- Year number
- Max 10% Cap Payment
- Over/Under Cap indicator

---

## Technical Stack
- **Backend**: ASP.NET Core 8.0, C#
- **Frontend**: Vanilla HTML/CSS/JavaScript
- **Charts**: Chart.js
- **Icons**: Font Awesome
- **Storage**: localStorage for Save/Load

## API Endpoints
- `POST /api/mortgage/calculate-monthly-payment`
- `POST /api/mortgage/amortization-schedule`
- `POST /api/mortgage/amortization-schedule-extra`
- `POST /api/mortgage/extra-payment-scenario`
- `POST /api/mortgage/amortization-schedule-one-time`
- `POST /api/mortgage/calculate-portfolio` - Portfolio calculation with multiple periods

## New Backend Models
- **ExtraPaymentEntry**: monthNumber, amount
- **MortgagePeriod**: loanAmount, annualInterestRate, loanTermYears, startDate, fixedPeriodYears, earlyRepaymentCapPercent, extraPayments
- **PortfolioCalculationResult**: combinedSchedule, totalInterestPaid, totalAmountPaid, totalMonths, periodResults

## Running the App
```bash
cd MortgageWebApp
dotnet run --urls "http://localhost:5001"
```

## Unit Tests
26 unit tests in MortgageWebApp.Tests covering:

### Original Tests (14)
- Monthly payment calculations
- Amortization schedules
- Extra payment scenarios
- One-time extra payments
- Edge cases (zero interest, invalid inputs)

### New Portfolio Tests (12)
- Single period portfolio calculation
- Two periods with sequential balance transfer
- Multiple extra payments at different months
- Extra payment at month 10 calculates correctly
- 10% cap calculation
- Extra payment exceeds cap (over cap indicator)
- Extra payment under cap
- Fixed period years stored correctly
- Multiple extra payments different months
- Balance drops significantly with extra payment
- Empty periods throws exception
- Portfolio with various fixed period combinations

Run with: `cd MortgageWebApp.Tests && dotnet test`

## E2E Tests
Playwright E2E tests in `e2e_tests.js` covering:
- Tab switching (Single Mortgage ↔ Portfolio)
- Add mortgage period
- Add remortgage period
- Extra payment per month entry
- 10% cap display in table
- Calculate portfolio
- Single mortgage calculation

Run with: `node e2e_tests.js`
