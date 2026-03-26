# Mortgage Calculator - Existing Functionality

## Overview
A full-featured mortgage calculator web application built with ASP.NET Core 8.0 backend and vanilla JavaScript frontend.

## Current Features

### 1. Loan Input Fields
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

## Running the App
```bash
cd MortgageWebApp
dotnet run --urls "http://localhost:5001"
```

## Unit Tests
14 unit tests in MortgageWebApp.Tests covering:
- Monthly payment calculations
- Amortization schedules
- Extra payment scenarios
- One-time extra payments
- Edge cases (zero interest, invalid inputs)

Run with: `cd MortgageWebApp.Tests && dotnet test`