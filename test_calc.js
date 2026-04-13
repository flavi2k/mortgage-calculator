const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch();
  const page = await browser.newPage();
  
  console.log('Navigating to http://localhost:5001...');
  await page.goto('http://localhost:5001', { waitUntil: 'networkidle', timeout: 30000 });
  console.log('Page loaded!');
  
  // Fill in some values (clear first)
  const loanInput = await page.locator('#loanAmount');
  await loanInput.click();
  await loanInput.selectText();
  await loanInput.type('250000');
  await page.fill('#interestRate', '4.5');
  
  // Test save
  await page.click('button:has-text("Save")');
  await page.waitForTimeout(500);
  
  // Check localStorage
  const saved = await page.evaluate(() => localStorage.getItem('mortgageCalculation'));
  console.log('Saved data:', saved ? 'Yes' : 'No');
  
  // Clear and fill with different value
  const loanInput2 = await page.locator('#loanAmount');
  await loanInput2.click();
  await loanInput2.selectText();
  await loanInput2.type('100000');
  
  // Test load
  await page.click('button:has-text("Load")');
  await page.waitForTimeout(1000);
  
  const loanValue = await page.locator('#loanAmount').inputValue();
  console.log('Loaded loan amount:', loanValue);
  
  if (loanValue === '250,000') {
    console.log('✓ Save/Load working!');
  }
  
  await browser.close();
})();