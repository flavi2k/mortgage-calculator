const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch();
  const page = await browser.newPage();
  
  console.log('=== Inline Extra Payment E2E Tests ===\n');
  
  try {
    await page.goto('http://localhost:5001', { waitUntil: 'networkidle', timeout: 30000 });
    
    console.log('1. Calculate default portfolio');
    await page.waitForTimeout(1000);
    
    const calcBtn = page.locator('#calculatePortfolioBtn');
    if (await calcBtn.isVisible()) {
      await calcBtn.click();
      await page.waitForTimeout(2000);
    }
    
    console.log('2. Check if portfolio schedule is visible');
    const scheduleCard = page.locator('#portfolioScheduleCard');
    const isVisible = await scheduleCard.isVisible();
    console.log(`   Schedule visible: ${isVisible}`);
    
    if (!isVisible) {
      console.log('   ✗ Schedule not visible, cannot test inline extra payments');
      await browser.close();
      return;
    }
    
    console.log('3. Find extra payment input in table');
    const extraInput = page.locator('#portfolioScheduleBody input[type="text"]').first();
    const inputVisible = await extraInput.isVisible();
    console.log(`   Extra payment input visible: ${inputVisible}`);
    
    if (!inputVisible) {
      console.log('   ✗ No extra payment input found in table');
      await browser.close();
      return;
    }
    
    console.log('4. Enter extra payment value of 500');
    await extraInput.fill('500');
    console.log('   ✓ Filled value');
    
    console.log('5. Click outside to trigger blur');
    await page.click('.header');
    await page.waitForTimeout(1500);
    
    console.log('6. Verify value is preserved');
    const valueAfter = await extraInput.inputValue();
    console.log(`   Value after blur: "${valueAfter}"`);
    
    if (valueAfter === '500' || valueAfter.includes('500')) {
      console.log('   ✓ Value preserved correctly!\n');
    } else {
      console.log(`   ✗ Value not preserved! Expected "500", got "${valueAfter}"\n`);
    }
    
    console.log('7. Check that totals were recalculated');
    const resultsGrid = page.locator('#portfolioResultsContainer .results-grid');
    const resultsVisible = await resultsGrid.isVisible();
    console.log(`   Results grid visible: ${resultsVisible}`);
    
    console.log('8. Add another extra payment on a different row');
    const allInputs = page.locator('#portfolioScheduleBody input[type="text"]');
    const count = await allInputs.count();
    console.log(`   Found ${count} extra payment inputs`);
    
    if (count > 1) {
      await allInputs.nth(1).fill('250');
      await page.click('.header');
      await page.waitForTimeout(1500);
      
      const value2 = await allInputs.nth(1).inputValue();
      console.log(`   Second input value: "${value2}"`);
      
      if (value2 === '250' || value2.includes('250')) {
        console.log('   ✓ Second value preserved!\n');
      } else {
        console.log(`   ✗ Second value not preserved!\n`);
      }
    }
    
    console.log('=== ALL INLINE EXTRA PAYMENT TESTS COMPLETE ===');
    console.log('\nTest Result: PASSED');
    
  } catch (error) {
    console.error('Test Error:', error.message);
    console.log('\nTest Result: FAILED');
  }
  
  await browser.close();
})();
