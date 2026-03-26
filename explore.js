const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch({ 
    headless: false,
    slowMo: 500
  });
  const page = await browser.newPage();
  
  page.on('console', msg => {
    if (msg.type() === 'error') {
      console.log('ERROR:', msg.text());
    }
  });
  
  page.on('pageerror', err => {
    console.log('PAGE ERROR:', err.message);
  });
  
  console.log('Opening http://localhost:5000...');
  await page.goto('http://localhost:5000', { waitUntil: 'networkidle' });
  console.log('Page loaded!');
  
  // Wait for user to interact
  console.log('\nPress Enter to close...');
  await new Promise(() => {});
})();