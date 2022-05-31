using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laybuy.PageObjects
{
    public class HomePage : BasePage
    {
        private IWebDriver _driver;
        public HomePage(IWebDriver driver): base(driver)
        {
            _driver = driver;
        }

        public By laybuyLogoBy = By.Id("logo-laybuy");
        public IWebElement LaybuyLogo => _driver.FindElement(laybuyLogoBy);

        public By shopLinkBy => By.LinkText("Shop");
        public IWebElement ShopLink=>_driver.FindElement(shopLinkBy);  
    }
}
