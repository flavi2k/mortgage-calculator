const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch();
  const page = await browser.newPage();
  
  console.log('=== Extra Payment Balance Test ===\n');
  
  try {
    await page.goto('http://localhost:5001', { waitUntil: 'networkidle', timeout: 30000 });
    
    console.log('1. Calculate default portfolio');
    const calcBtn = page.locator('#calculatePortfolioBtn');
    if (await calcBtn.isVisible()) {
      await calcBtn.click();
      await page.waitForTimeout(2000);
    }
    
    console.log('2. Get original balance on first row');
    const originalBalanceCell = page.locator('#portfolioScheduleBody tr').first().locator('td').nth(8);
    const originalBalance = await originalBalanceCell.textContent();
    console.log(`   Original balance: ${originalBalance}`);
    
    console.log('3. Add extra payment of 500');
    const extraInput = page.locator('#portfolioScheduleBody input[type="text"]').first();
    await extraInput.fill('500');
    await page.click('.header');
    await page.waitForTimeout(2000);
    
    console.log('4. Get new balance after recalculation');
    const newBalanceCell = page.locator('#portfolioScheduleBody tr').first().locator('td').nth(8);
    const newBalance = await newBalanceCell.textContent();
    console.log(`   New balance: ${newBalance}`);
    
    // Parse the values
    const parseGBP = (str) => parseFloat(str.replace(/[^0-9.-]/g, ''));
    const orig = parseGBP(originalBalance);
    const newBal = parseGBP(newBalance);
    
    console.log('\n=== Results ===');
    console.log(`Original: £${orig.toLocaleString()}`);
    console.log(`New: £${newBal.toLocaleString()}`);
    console.log(`Difference: £${(orig - newBal).toLocaleString()}`);
    
    if (newBal < orig) {
      console.log('\n✓ PASSED: Balance decreased with extra payment!');
    } else {
      console.log('\n✗ FAILED: Balance did not decrease!');
    }
    
  } catch (error) {
    console.error('Test Error:', error.message);
  }
  
  await browser.close();
})();
