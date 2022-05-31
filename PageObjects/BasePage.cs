using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laybuy.PageObjects
{
    public abstract class BasePage
    {
        protected IWebDriver _driver;
        public BasePage(IWebDriver driver)
        {
            _driver = driver; 
        }

    }
}
