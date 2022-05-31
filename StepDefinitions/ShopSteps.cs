using BoDi;
using Laybuy.Helpers;
using Laybuy.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Laybuy.StepDefinitions
{
    [Binding]
    public class ShopSteps: BaseSteps
    {
        HomePage _homePage;
        ShopPage _shopPage;
        WaitHelper _waitHelper;
        public ShopSteps(IWebDriver driver, IObjectContainer objectContainer, ScenarioContext scenarioContext, FeatureContext featureContext) : base(driver, objectContainer, scenarioContext, featureContext)
        {
            _homePage= new HomePage(driver);
            _shopPage = new ShopPage(driver);
            _waitHelper= new WaitHelper(driver);
        }
        
        [Given(@"I am on Laybuy website")]
        public void GivenIAmOnLaybuyWebsite()
        {
            _waitHelper.WaitForElementVisible(_homePage.laybuyLogoBy);
        }

        [When(@"I search in the Shop with the keyword '(.*)'")]
        public void WhenISearchInTheShopWithTheKeyword(string keyWord)
        {
            //navigate to the Shop
            _waitHelper.WaitForElementClickable(_homePage.shopLinkBy);
            _homePage.ShopLink.Click();
            //search in the Shop
            _waitHelper.WaitForElementVisible(_shopPage.searchBoxBy);
            _shopPage.SearchBox.SendKeys(keyWord+Keys.Enter);   
        }

        [Then(@"I should see at least (.*) shop directory tiles")]
        public void ThenIShouldSeeAtLeastShopDirectoryTiles(int expectedNumber)
        {
            _waitHelper.WaitForElementVisible(_shopPage.resultsTielsBy);           
            Assert.GreaterOrEqual(_shopPage.ResultsTiles.Count, expectedNumber, $"There are less than {expectedNumber} tiles");
        }

        [Then(@"Any tile should navigate to the correct merchant wbsite")]
        public void ThenAnyTileShouldNavigateToTheCorrectMerchantWbsite()
        {
            int numberOfTiles=_shopPage.ResultsTiles.Count;
            int rm=new Random().Next(1,numberOfTiles);

            string tileUrl=_shopPage.ResultsTiles[rm].GetAttribute("href");
            var element = _shopPage.ResultsTiles[rm];
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;            
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            element.Click();
            _driver.SwitchTo().Window(_driver.WindowHandles.Last());
            string expectedUrl=tileUrl.Substring(0,tileUrl.IndexOf("/?")).Replace("www.","").Replace("https://","");          
            Assert.IsTrue(_driver.Url.Contains(expectedUrl), "The new opened window is not the merchant website.");   
        }




    }
}
