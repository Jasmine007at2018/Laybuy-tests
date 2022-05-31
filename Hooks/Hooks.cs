using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using BoDi;
using Laybuy.Drivers;
using Laybuy.Helpers;
using Laybuy.Models.Common;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Laybuy.Features.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        private readonly FeatureContext _featureContext;
        private readonly ScenarioContext _scenarioContext;
        private APIResponse _apiResponse = new APIResponse();

        public Hooks(IObjectContainer objectContainer, FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
        }

        //Extent report config
        private ExtentTest featureName;
        private ExtentTest scenario;
        private static AventStack.ExtentReports.ExtentReports extent;

        [BeforeTestRun]
        public static void InitializeReport()
        {
            //Initialize Extent report before test starts
            var htmlReporter = new ExtentV3HtmlReporter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ExtentReports.html"));
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            htmlReporter.Config.ReportName = "Laybuy Tests Report";
            htmlReporter.Config.DocumentTitle = "Laybuy Tests Report";

            extent = new AventStack.ExtentReports.ExtentReports();
            extent.AttachReporter(htmlReporter);

        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            GlobalVariables.APIUrl = Environment.GetEnvironmentVariable("API_URL");
            GlobalVariables.WebUrl = Environment.GetEnvironmentVariable("Web_URL");
            if (GlobalVariables.APIUrl == null || GlobalVariables.WebUrl == null)
            {
                foreach (var e in Environment.GetEnvironmentVariables())
                {
                    var entry = (DictionaryEntry)e;
                    Console.WriteLine($"{entry.Key}:{entry.Value}");
                }
                throw new Exception("env varaibles being null");
            }
            
            if (_scenarioContext.ScenarioInfo.Tags.Contains("Web"))
            {
                string browser = "chrome";
                var headless = bool.Parse(Environment.GetEnvironmentVariable("SELENIUM_HEADLESS", EnvironmentVariableTarget.Process));
                var implicitWait = int.Parse(Environment.GetEnvironmentVariable("SELENIUM_WAIT", EnvironmentVariableTarget.Process));
                var Url = Environment.GetEnvironmentVariable("Web_URL", EnvironmentVariableTarget.Process);              

                IWebDriver driver = FactoryBuilder.GetFactory(browser)
                                                  .SetHeadless(headless)
                                                  .GetDriver();
                driver.Manage()
                      .Timeouts()
                      .ImplicitWait = TimeSpan.FromSeconds(implicitWait);
                driver.Manage()
                      .Window.Maximize();
                driver.Navigate()
                      .GoToUrl(Url);
                _objectContainer.RegisterInstanceAs(driver);
            }
            else if(_scenarioContext.ScenarioInfo.Tags.Contains("API"))
            {
                _objectContainer.RegisterInstanceAs(_apiResponse);
            }
            else
            {
                throw new Exception("It's either a web or API test");
            }

            //Report settings
            featureName = extent.CreateTest<Feature>(_featureContext.FeatureInfo.Title);
            scenario = featureName.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void TearDown()
        {
            if (_scenarioContext.ScenarioInfo.Tags.Contains("Web"))
            {
                IWebDriver driver = _objectContainer.Resolve<IWebDriver>();
                driver.Quit();
            }               

            //Flush and generate the report
            extent.Flush();
        }

        [AfterStep]
        public void InsertReportingSteps()
        {
            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            

            //Reflection
            PropertyInfo pInfo = typeof(ScenarioContext).GetProperty("ScenarioExecutionStatus", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo getter = pInfo.GetGetMethod(nonPublic: true);
            object TestResult = getter.Invoke(_scenarioContext, null);

            //Write steps info into report
            if (TestResult.ToString() == "StepDefinitionPending")
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "When")
                    scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Skip("Step Definition Pending");
            }
            else
            {
                if (_scenarioContext.TestError == null)
                {
                    if (stepType == "Given")
                        scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                    else if (stepType == "When")
                        scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                    else if (stepType == "Then")
                        scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                }
                else if (_scenarioContext.TestError != null && _scenarioContext.ScenarioInfo.Tags.Contains("Web"))
                {
                    IWrapsDriver wrapperAccess = (IWrapsDriver)_objectContainer.Resolve<IWebDriver>();
                    IWebDriver driver = wrapperAccess.WrappedDriver;
                    ScreenshotHelper screenshotTools = new ScreenshotHelper(driver);
                    screenshotTools.SnapFullScreenshot(_featureContext.FeatureInfo.Title, _scenarioContext.ScenarioInfo.Title);
                    if (stepType == "Given")
                        scenario.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                    else if (stepType == "When")
                        scenario.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                    else if (stepType == "Then")
                        scenario.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
                }
            }
        }


        }
}
