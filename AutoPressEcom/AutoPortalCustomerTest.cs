using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace AutoPortal
{
    [TestClass]
    public class AutoPortalCustomerTest
    {
        string SITE_URL = @"http://localhost:81/autopress/wp-login.php";
        string BROWSER = "IE";
        int TIMESPAN = 100;
        IWebDriver _driver;

        [TestInitialize]
        public void OnStart()
        {
            //Opening the browser 
            if (BROWSER == "CHROME")
                _driver = new ChromeDriver();
            else
                _driver = new InternetExplorerDriver();
            _driver.Manage().Window.Maximize();
        }

        [TestCleanup]
        public void OnEnd()
        {            
            //Closing the browser
            _driver.Close();
        }

        [TestMethod]
        public void Execute_WorkFlow()
        {
            Portal_Login();
            NavigateTo_Shop();
            NavigateTo_ProductCategory("Hoodies");
            Addto_cart();
        }

        public void Portal_Login()
        {
            //Navigating to login page
            _driver.Navigate().GoToUrl(SITE_URL);
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(TIMESPAN));
            wait.Until<IWebElement>(d => d.FindElement(By.Id("user_login")));
            //Finding the Username textbox 
            IWebElement _webElement = _driver.FindElement(By.Id("user_login"));
            //inputing values
            _webElement.SendKeys("smitha");
            //Finding the password textbox
            _webElement = _driver.FindElement(By.Id("user_pass"));
            //Waiting for element to find before inputing
            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(TIMESPAN));
            wait.Until<IWebElement>(d => d.FindElement(By.Id("user_pass")));
            //inputing values
            _webElement.SendKeys("smitha");
            //submit the textbox keypress (enter key)
            _webElement.Submit();


            if (!_driver.Url.Equals("http://localhost:81/autopress/my-account/"))
            wait.Until<bool>(d=>d.Url.Equals("http://localhost:81/autopress/my-account/"));
        
            //Navigating to next page to confirm it is authenticated
            Assert.AreEqual("http://localhost:81/autopress/my-account/", _driver.Url);            
        }

        
        public void NavigateTo_Shop()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(TIMESPAN));
            wait.Until<IWebElement>(d => d.FindElement(By.ClassName("nav-menu")));

            IWebElement _webElement = _driver.FindElement(By.ClassName("nav-menu"));
            ReadOnlyCollection<IWebElement> _webElementsli = _driver.FindElements(By.TagName("li"));
            bool IsShopLinkFound = false;
            foreach (IWebElement liElement in _webElementsli)
            {
                IWebElement _webElementAnchor = liElement.FindElement(By.TagName("a"));
                if(_webElementAnchor.Text=="Shop")
                {
                    IsShopLinkFound = true;
                    _webElementAnchor.Click();
                    break;
                }
            }
            
            Assert.IsTrue(IsShopLinkFound);
        }

        public void NavigateTo_ProductCategory(string productcategory)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until<ReadOnlyCollection<IWebElement>>(d => d.FindElements(By.ClassName("product-category")));
            //Finding the Username textbox 
            ReadOnlyCollection<IWebElement> _webElementsli = _driver.FindElements(By.ClassName("product-category"));

            foreach (IWebElement liElement in _webElementsli)
            {
                IWebElement _webElementAnchor = liElement.FindElement(By.TagName("a"));
                if (_webElementAnchor.Text.Contains(productcategory))
                {                  
                    _webElementAnchor.Click();
                    break;
                }
            }

            wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until<IWebElement>(d => d.FindElement(By.ClassName("woocommerce-products-header")));

            IWebElement _webElementheader = _driver.FindElement(By.ClassName("woocommerce-products-header"));
            string header = _webElementheader.FindElement(By.TagName("h1")).Text;

            Assert.AreEqual(header, productcategory);

        }

        public void Addto_cart()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(TIMESPAN));
            wait.Until<ReadOnlyCollection<IWebElement>>(d => d.FindElements(By.ClassName("products")));
            //Finding the Username textbox 
            ReadOnlyCollection<IWebElement> _webElementsA11products = _driver.FindElements(By.ClassName("products"));
            IWebElement add2CartButton = _webElementsA11products[0].FindElement(By.ClassName("add_to_cart_button"));
            add2CartButton.Click();
            _driver.Navigate().GoToUrl("http://localhost:81/autopress/cart/");

            Thread.Sleep(5000);
        }

    }
}
