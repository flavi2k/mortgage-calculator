const { chromium } = require('playwright');

(async () => {
  const browser = await chromium.launch();
  const page = await browser.newPage();
  
  const errors = [];
  const warnings = [];
  
  page.on('console', msg => {
    if (msg.type() === 'error') {
      errors.push(msg.text());
    } else if (msg.type() === 'warning') {
      warnings.push(msg.text());
    }
  });
  
  page.on('pageerror', err => {
    errors.push(err.message);
  });
  
  try {
    console.log('Navigating to http://localhost:5000...');
    await page.goto('http://localhost:5000', { waitUntil: 'networkidle', timeout: 30000 });
    console.log('Page loaded successfully!');
    
    const title = await page.title();
    console.log('Page title:', title);
    
    const url = page.url();
    console.log('Current URL:', url);
    
    console.log('\n--- Page Content ---');
    const body = await page.content();
    console.log(body.substring(0, 2000));
    
    if (errors.length > 0) {
      console.log('\n--- Console Errors ---');
      errors.forEach(e => console.log('ERROR:', e));
    } else {
      console.log('\n--- No console errors detected ---');
    }
    
    if (warnings.length > 0) {
      console.log('\n--- Console Warnings ---');
      warnings.forEach(w => console.log('WARNING:', w));
    }
    
  } catch (error) {
    console.error('Error:', error.message);
  }
  
  await browser.close();
})();