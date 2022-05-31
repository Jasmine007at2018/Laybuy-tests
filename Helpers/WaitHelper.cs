using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SELENIUM_EXTRAS = SeleniumExtras.WaitHelpers.ExpectedConditions;


namespace Laybuy.Helpers
{
    public class WaitHelper
    {
        private WebDriverWait _waitHelper;
        private IWebDriver _driver;

        public WaitHelper(IWebDriver driver)
        {
            _waitHelper = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }
        public void WaitForElementClickable(By locator)
        {
            _waitHelper.Until(SELENIUM_EXTRAS.ElementToBeClickable(locator));
        }

        public void WaitForElementVisible(By locator)
        {
            _waitHelper.Until(SELENIUM_EXTRAS.ElementIsVisible(locator));
        }

    }
}
