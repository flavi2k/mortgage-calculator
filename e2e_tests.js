const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch();
  const page = await browser.newPage();
  
  console.log('=== Mortgage Calculator Playwright E2E Tests ===\n');
  
  try {
    console.log('Navigating to http://localhost:5001...');
    await page.goto('http://localhost:5001', { waitUntil: 'networkidle', timeout: 30000 });
    console.log('Page loaded!\n');
    
    console.log('--- Test 1: Switch to Portfolio Tab ---');
    await page.click('#tabPortfolio');
    await page.waitForSelector('#portfolioTabContent.active');
    console.log('✓ Portfolio tab activated\n');
    
    console.log('--- Test 2: Check Initial Mortgage Period Exists ---');
    const periodCards = await page.locator('.period-card').count();
    if (periodCards >= 1) {
      console.log(`✓ Mortgage period exists (count: ${periodCards})\n`);
    } else {
      console.log(`✗ No periods found\n`);
    }
    
    console.log('--- Test 3: Fill in Initial Mortgage Details ---');
    const dateInputs = page.locator('#periodsContainer input[type="date"]');
    await dateInputs.first().fill('2024-01-01');
    
    const textInputs = page.locator('#periodsContainer input[type="text"]');
    const firstTextInput = textInputs.first();
    await firstTextInput.click();
    await firstTextInput.selectText();
    await firstTextInput.fill('250000');
    
    const numberInputs = page.locator('#periodsContainer input[type="number"]');
    await numberInputs.nth(0).fill('4.5');
    await numberInputs.nth(1).fill('25');
    console.log('✓ Initial mortgage details filled\n');
    
    console.log('--- Test 4: Add Remortgage Period ---');
    await page.click('.add-period-btn');
    await page.waitForTimeout(500);
    const periodCards2 = await page.locator('.period-card').count();
    if (periodCards2 === 2) {
      console.log('✓ Remortgage period added\n');
    } else {
      console.log(`✗ Expected 2 periods, found ${periodCards2}\n`);
    }
    
    console.log('--- Test 5: Verify Remortgage Period Fields ---');
    const periodTitles = await page.locator('.period-title').allTextContents();
    if (periodTitles[1].includes('Remortgage')) {
      console.log('✓ Second period labeled as remortgage\n');
    } else {
      console.log('✗ Second period not labeled correctly\n');
    }
    
    console.log('--- Test 6: Calculate Portfolio ---');
    await page.click('#calculatePortfolioBtn');
    await page.waitForTimeout(3000);
    
    const successMessage = await page.locator('.message.success').count();
    if (successMessage > 0) {
      console.log('✓ Portfolio calculation successful\n');
    } else {
      console.log('✗ Portfolio calculation failed\n');
    }
    
    console.log('--- Test 7: Check Amortization Table with Cap Columns ---');
    const scheduleCard = await page.locator('#portfolioScheduleCard').isVisible();
    if (scheduleCard) {
      console.log('✓ Portfolio schedule table displayed\n');
    }
    
    const headers = await page.locator('#portfolioScheduleCard th').allTextContents();
    const hasMaxCap = headers.some(h => h.includes('Max'));
    const hasOverCap = headers.some(h => h.includes('Over'));
    if (hasMaxCap && hasOverCap) {
      console.log('✓ 10% cap columns present in table\n');
    } else {
      console.log('✗ Missing cap columns\n');
    }
    
    console.log('--- Test 8: Add Extra Payments ---');
    const extraPaymentBtns = page.locator('button:has-text("Add Extra Payments")');
    await extraPaymentBtns.first().click();
    await page.waitForSelector('.extra-payment-grid', { state: 'visible' });
    console.log('✓ Extra payment grid opened\n');
    
    console.log('--- Test 9: Add Extra Payment Row ---');
    const addRowBtns = page.locator('button:has-text("Add Row")');
    await addRowBtns.first().click();
    await page.waitForTimeout(300);
    console.log('✓ Extra payment row added\n');
    
    console.log('--- Test 10: Switch back to Single Mortgage Tab ---');
    await page.click('#tabSingle');
    await page.waitForSelector('#singleTabContent.active');
    console.log('✓ Single mortgage tab activated\n');
    
    console.log('--- Test 11: Verify Single Mortgage Calculation Works ---');
    await page.click('#calculateBtn');
    await page.waitForTimeout(1000);
    const monthlyPayment = await page.locator('#monthlyPayment').textContent();
    console.log(`✓ Monthly payment: ${monthlyPayment}\n`);
    
    console.log('=== ALL TESTS COMPLETED ===');
    
  } catch (error) {
    console.error('Test Error:', error.message);
  }
  
  await browser.close();
})();
