using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;

namespace AutoPressEcom
{
    [TestClass]
    public class AutoPortalAdminTest
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
            else
                _driver = new InternetExplorerDriver();
        }

        [TestCleanup]
        public void OnEnd()
        {
            //Closing the browser
            _driver.Close();           

        }

        [TestMethod]
        public void Portal_AdminLogin()
        {
            //Navigating to login page
            _driver.Navigate().GoToUrl(SITE_URL);

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until<IWebElement>(d => d.FindElement(By.Id("user_login")));
            //Finding the Username textbox 
            IWebElement _webElement = _driver.FindElement(By.Id("user_login"));
            //inputing values
            _webElement.SendKeys("siteadmin");
            //Finding the password textbox
            _webElement = _driver.FindElement(By.Id("user_pass"));
            //Waiting for element to find before inputing
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until<IWebElement>(d => d.FindElement(By.Id("user_pass")));
            //inputing values
            _webElement.SendKeys("siteadmin");
            //submit the textbox keypress (enter key)
            _webElement.Submit();
            //Navigating to next page to confirm it is authenticated
            Assert.AreEqual("http://localhost:81/autopress/wp-admin/", _driver.Url);
        }

        


    }
}
