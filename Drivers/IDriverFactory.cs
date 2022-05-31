using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laybuy.Drivers
{
    public interface IDriverFactory
    {
        IWebDriver GetDriver();

        DriverOptions GetOptions();

        IDriverFactory SetHeadless(bool isHeadless);
    }
}
