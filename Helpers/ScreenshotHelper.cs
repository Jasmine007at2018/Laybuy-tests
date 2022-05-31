using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laybuy.Helpers
{
    public class ScreenshotHelper
    {
        private readonly IWebDriver _driver;

        public ScreenshotHelper(IWebDriver driver)
        {
            _driver = driver;
        }

        public void SnapFullScreenshot(string featureTitle, string scenarioTitle)
        {
            try
            {
                //VerticalCombineDecorator vcd = new VerticalCombineDecorator(new ScreenshotMaker());
                string ssPath = Path.Combine(Directory.GetCurrentDirectory(), featureTitle.Replace(" ", "_") + "_" + scenarioTitle.Replace(" ", "_") + "_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + "_fullPage.png");
                _driver.TakeScreenshot().SaveAsFile(ssPath);//.ToMagickImage().Write(ssPath);
                Console.WriteLine($"SCREENSHOTXX file:///{ssPath} XXSCREENSHOT");
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
