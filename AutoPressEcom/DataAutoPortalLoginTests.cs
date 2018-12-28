using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
namespace AutoPressEcom
{
    [TestClass]
    public class DataAutoPortalLoginTests
    {
        string SITE_URL = @"http://localhost:81/autopress/wp-login.php";
        string BROWSER = "CHROME";

        IWebDriver _driver;

        [TestInitialize]
        public void OnStart()
        {
            //Opening the browser 
            if (BROWSER == "CHROME")
                _driver = new ChromeDriver();
            else if (BROWSER == "IE")
                _driver = new InternetExplorerDriver();
            _driver.Manage().Window.Maximize();
        }

        [TestCleanup]
        public void OnEnd()
        {
            //Closing the browser
            _driver.Close();

        }

        [DataTestMethod]
        [DataRow("siteadmin","siteadmin", "http://localhost:81/autopress/wp-admin/")] //<-- Admin
        [DataRow("jojo", "jojo", "http://localhost:81/autopress/wp-admin/")] //<-- Store Manager
        [DataRow("smitha", "smitha", "http://localhost:81/autopress/my-account/")] //<-- Customer
        public void Portal_Login(string username , string password , string expectedurl)
        {
            //Navigating to login page
            _driver.Navigate().GoToUrl(SITE_URL);
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(25));
            wait.Until<IWebElement>(d => d.FindElement(By.Id("user_login")));
            //Finding the Username textbox 
            IWebElement _webElement = _driver.FindElement(By.Id("user_login"));
            //inputing values
            _webElement.SendKeys(username);
            //Finding the password textbox
            _webElement = _driver.FindElement(By.Id("user_pass"));
            //Waiting for element to find before inputing
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until<IWebElement>(d => d.FindElement(By.Id("user_pass")));
            //inputing values
            _webElement.SendKeys(password);
            //submit the textbox keypress (enter key)
            _webElement.Submit();
            //Navigating to next page to confirm it is authenticated
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(100));

            Assert.AreEqual(expectedurl, _driver.Url);

            
        }


    }
}
