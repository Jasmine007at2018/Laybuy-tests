using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laybuy.Drivers
{
    public class ChromeDriverFactory : AbstractDriverFactory
    {
        private bool headless = false;

        public override DriverOptions GetOptions()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-gpu",
                      "--ignore-certificate-errors",
                      "no-sandbox",
                      "--window-size=1920,1080");
            options.AddUserProfilePreference("download.default_directory", AppDomain.CurrentDomain.BaseDirectory);
            if (headless) options.AddArguments("--headless");
            return options;
        }

        protected override IWebDriver BuildDriver()
        {
            return new ChromeDriver(ChromeDriverService.CreateDefaultService(), (ChromeOptions)GetOptions(), TimeSpan.FromMinutes(2));
        }

        public override IDriverFactory SetHeadless(bool isHeadless)
        {
            headless = isHeadless;
            return this;
        }
    }
}
