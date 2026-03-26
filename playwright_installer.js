#!/usr/bin/env node

const { execSync } = require('child_process');
const { existsSync } = require('fs');

function isPlaywrightInstalled() {
  try {
    execSync('playwright --version', { stdio: 'ignore' });
    return true;
  } catch (error) {
    return false;
  }
}

function installPlaywright() {
  try {
    console.log('Installing Playwright CLI...');
    execSync('npm install -g @playwright/test', { stdio: 'inherit' });
    console.log('Playwright CLI installed successfully!');
  } catch (error) {
    console.error('Failed to install Playwright CLI:', error.message);
    process.exit(1);
  }
}

function main() {
  if (isPlaywrightInstalled()) {
    console.log('Playwright CLI is already installed.');
  } else {
    installPlaywright();
  }
}

main();
