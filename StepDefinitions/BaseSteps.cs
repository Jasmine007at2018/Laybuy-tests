using BoDi;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Laybuy.StepDefinitions
{
    public abstract class BaseSteps
    {
        protected readonly IWebDriver _driver;
        protected IObjectContainer _objectContainer;
        protected ScenarioContext _scenarioContext;
        protected FeatureContext _featureContext;
        public BaseSteps(IWebDriver driver, IObjectContainer objectContainer, ScenarioContext scenatioContext, FeatureContext featureContext)
        {
            _driver = driver;
            _objectContainer = objectContainer;
            _scenarioContext = scenatioContext;
            _featureContext = featureContext;
        }

    }
}
