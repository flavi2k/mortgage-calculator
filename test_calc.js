const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch();
  const page = await browser.newPage();
  
  console.log('Navigating to http://localhost:5001...');
  await page.goto('http://localhost:5001', { waitUntil: 'networkidle', timeout: 30000 });
  console.log('Page loaded!');
  
  const loanAmount = await page.locator('#loanAmount');
  await loanAmount.click();
  await loanAmount.selectText();
  await loanAmount.type('215000');
  
  const oneTime = await page.locator('#oneTimePayment');
  await oneTime.click();
  await oneTime.selectText();
  await oneTime.type('10000');
  
  await page.click('#calculateBtn');
  
  await page.waitForTimeout(3000);
  
  const loanVal = await page.locator('#loanAmount').inputValue();
  const oneTimeVal = await page.locator('#oneTimePayment').inputValue();
  console.log('Loan amount:', loanVal);
  console.log('One-time:', oneTimeVal);
  
  const monthlyPayment = await page.locator('#monthlyPayment').textContent();
  console.log('Monthly payment:', monthlyPayment);
  
  const savingsTable = await page.locator('#savingsTableBody').innerHTML();
  console.log('Savings table:\n', savingsTable);
  
  await browser.close();
})();