using System;
using TechTalk.SpecFlow;
using System.Configuration;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Bdd.Project.Test.ApiClients;
using Bdd.Project.Test.Models;
using System.Threading;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bdd.Porject.Test.Steps
{
    [Binding]
    public class WeatherSteps
    {
        private static string HomeUrl { get; set; }
        private static string SearchString { get; set; }
        private static string GoogleTemp { get; set; }
        private static string WeatherApiTemp { get; set; }
        private IWebDriver webDriver { get; set; }
        private IWebElement searchBox { get; set; }
        private IWebElement searchButton { get; set; }
        private ClientInterface client{ get;set;}
        private WeatherResponseModel weatherResponse { get; set; }

        [BeforeFeature]
        public static void Setup()
        {
            HomeUrl = ConfigurationManager.AppSettings["GoogleURL"];
            SearchString = ConfigurationManager.AppSettings["SearchValue"];
        }

        [Given(@"Call Google home URL")]
        public void GivenCallGoogleHomeURL()
        {
            // Starting the Firefox driver
            webDriver = new FirefoxDriver();
            webDriver.Navigate().GoToUrl(HomeUrl);
            webDriver.Manage().Window.Maximize();
        }

        [Then(@"Find the search box")]
        public void ThenFindTheSearchBox()
        {
            searchBox = webDriver.FindElement(By.ClassName("gLFyf"));
        }

        [Then(@"Enter search box text")]
        public void ThenEnterSearchBoxText()
        {
            searchBox.SendKeys(SearchString);
        }

        [Then(@"Find and click the search button")]
        public void ThenFindAndClickTheSearchButton()
        {
            searchButton = webDriver.FindElement(By.ClassName("gNO89b"));
            Thread.Sleep(1000);
            searchButton.Click();
        }

        [Then(@"Read the result Temperature")]
        public void ThenReadTheResultTemperature()
        {
            GoogleTemp = webDriver.FindElement(By.Id("wob_tm")).Text;
        }

        [Then(@"Call the Open weather Api")]
        public void ThenCallTheOpenWeatherApi()
        {
            client = new ClientInterface();
            var response = client.GetCurrentWeather("8.5241", "76.9366");

            WeatherApiTemp = response.current.temp.ToString();
        }

        [Then(@"Compare the temperatures")]
        public void ThenCompareTheTemperatures()
        {
            Assert.AreEqual(int.Parse(GoogleTemp), int.Parse(WeatherApiTemp));
        }
    }
}
