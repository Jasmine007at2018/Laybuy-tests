using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laybuy.PageObjects
{
    public class ShopPage : HomePage
    {
        private IWebDriver _driver;
        public ShopPage(IWebDriver driver): base(driver)
        {
            _driver = driver;
        }

        public By searchBoxBy = By.XPath("//input[@type='search']");
        public IWebElement SearchBox => _driver.FindElement(searchBoxBy);
        public By resultsTilesHeaderBy = By.CssSelector(".shop-directory-module--match-text--2dLTb");
        public By resultsTielsBy = By.CssSelector(".tile-module--tile--1ZeJx");
        public IList<IWebElement> ResultsTiles=>_driver.FindElements(resultsTielsBy);
    }
}
