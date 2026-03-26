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
  
  await page.click('#calculateBtn');
  await page.waitForTimeout(3000);
  
  const ltvCard = await page.locator('#ltvCard').isVisible();
  const financialAdvisor = await page.locator('#financialAdvisor').count();
  
  console.log('LTV Card visible:', ltvCard);
  console.log('Financial Advisor exists:', financialAdvisor > 0);
  
  if (ltvCard && financialAdvisor > 0) {
    console.log('✓ Financial Advisor working!');
  }
  
  await browser.close();
})();