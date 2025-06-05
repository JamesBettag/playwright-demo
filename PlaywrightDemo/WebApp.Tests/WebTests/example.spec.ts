import { test, expect } from '@playwright/test';

test('Electricity Energy Physical Forwards Fixed Firm', async ({ page }) => {

    await page.goto('https://portal.business-dev.ptp.energy/login?continue=/ETRM/Trading/TradeBuilder.aspx');

    await page.fill('#username', process.env.TEST_USERNAME || '');

    await page.fill('#password', process.env.TEST_PASSWORD || '');

    await page.locator('#app span').nth(2).click();

    await page.waitForURL('https://portal.business-dev.ptp.energy/ETRM/Trading/TradeBuilder.aspx');

    await page.locator('.w-full').first().selectOption("Today");

    await page.locator('.ta-form-grid__cell > .select > select').first().selectOption("Central");

    await page.locator('div:nth-child(4) > div:nth-child(2) > .select > select').first().selectOption("*Mock_Company_-_Test_Only_CA1");

    await page.locator('div').filter({ hasText: /^CommodityCapacityElectricityEmissionsEnvironmentalEventNatural GasTransmission$/ }).getByRole('combobox').selectOption("Electricity");

    await page.locator('div:nth-child(5) > div:nth-child(3) > div:nth-child(2) > .select > select').selectOption("Energy");

    await page.locator('div').filter({ hasText: /^Phys\.\/Fin\.FinancialPhysical$/ }).getByRole('combobox').selectOption("Physical");

    await page.locator('div:nth-child(5) > div:nth-child(2) > .select > select').selectOption("Forwards");

    await page.locator('div:nth-child(6) > div:nth-child(2) > .select > select').selectOption("Fixed");

    await page.locator('div:nth-child(7) > div:nth-child(2) > .select > select').selectOption("Firm")

    await page.getByRole('button', { name: 'Continue' }).click()
    await page.waitForURL('https://portal.business-dev.ptp.energy/ETRM/Trading/TradeBuilder.aspx?subTab=details');

    await page.getByTitle('Trader\r\nOwner Groups: Business Analysts, Trading, Accounting, Control, Asset Man').getByRole('combobox').selectOption({ index: 1 });// Select the first option in the dropdown

    await page.getByTitle('Transmission Provider\r\nOwner Groups: Business Analysts, Trading, Accounting, Con').getByRole('combobox').selectOption("ERCO");

    await page.getByTitle('Electricity Delivery Location\r\nOwner Groups: Business Analysts, Trading, Account').getByRole('combobox').selectOption({ index: 1 }); // Select the first option in the dropdown

    await page.getByTitle('Balancing Authority reported').getByRole('combobox').selectOption("ERCO");

    await page.getByTitle('P/S from the Business Unit\'s').getByRole('combobox').selectOption("PURCHASE");

    await page.getByTitle('Required field. Transaction').locator('label'). click(); //one click false

    await page.getByTitle('If true, transaction is a Specified Transaction as defined by Washington State').getByLabel('').click() //one click false

    await page.getByTitle('If true, transaction is a Specified Transaction as defined by CARB\r\nOwner Groups: Business Analysts, Trading, Accounting, Control, Asset Management, Operations, Origination, Scheduling\r\nTRSCTN_META_BOOL_NF_G44X', { exact: true }).locator('label').click();

    await page.getByRole('spinbutton').nth(1).fill('1');

    await page.getByRole('spinbutton').nth(2).fill('1');

    await page.getByTitle('Notes for CARB transactions\r\nOwner Groups: Business Analysts, Trading, Accountin').getByRole('textbox').fill("This is a test trade.");

    await page.getByText('Details', { exact: true }).click();

    await page.getByRole('button', { name: 'Save changes' }).click();

    const tradeNameText = await page.locator('#details-container--trade-name').textContent();
    
    await page.getByText('ORIGEN', { exact: true }).fill("ORIGEN");
    

    console.log('Trade Name:', tradeNameText);
    await page.pause();

})

test('pic diff', async ({ page }) => {
    await page.goto('https://portal.business-dev.ptp.energy/login?continue=/ETRM/Trading/TradeBuilder.aspx');

    await page.fill('#username', process.env.TEST_USERNAME || '');

    await page.fill('#password', process.env.TEST_PASSWORD || '');

    await page.locator('#app span').nth(2).click();

    await page.waitForURL('https://portal.business-dev.ptp.energy/ETRM/Trading/TradeBuilder.aspx');

    await page.locator('.w-full').first().selectOption("Next Day");

    await page.locator('.ta-form-grid__cell > .select > select').first().selectOption("Mountain");

    await expect(page).toHaveScreenshot('form-state.png', { maxDiffPixels: 100 });
})

