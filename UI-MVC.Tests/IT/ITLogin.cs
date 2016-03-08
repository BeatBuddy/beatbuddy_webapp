using BB.BL;
using BB.BL.Domain;
using BB.DAL;
using BB.DAL.EFOrganisation;
using BB.DAL.EFUser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using BB.UI.Web.MVC.Controllers;
using System;
using BB.UI.Web.MVC.Tests.Helpers;
using OpenQA.Selenium.Support.UI;

namespace BB.UI.Web.MVC.Tests.IT
{
    [TestClass]
    public class ITLogin 
    {
        IWebDriver chromeDriver;

        [TestInitialize]
        public void TestInitialize()
        {
            chromeDriver = new ChromeDriver();

            IUserManager userManager = DbInitializer.CreateUserManager();
            AccountController _acountController = new AccountController(userManager);
        }

        [TestMethod]
        public void LoginTest()
        {
            WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(10));

            chromeDriver.Navigate().GoToUrl("http://teamd.azurewebsites.net/");
            var loginModal = chromeDriver.FindElement(By.Id("loginModal"));

            Assert.IsFalse(loginModal.Displayed);

            wait.Until(driver => driver.FindElement(By.XPath("//a[@href='#loginModal']")).Displayed);
            chromeDriver.FindElement(By.XPath("//a[@href='#loginModal']")).Click();

            loginModal = chromeDriver.FindElement(By.Id("loginModal"));
            wait.Until(driver => driver.FindElement(By.Id("loginModal")).Displayed);
            //Assert.IsTrue(loginModal.Displayed);

            loginModal.FindElement(By.Id("Email")).SendKeys("admin@admin.com");
            loginModal.FindElement(By.Id("Password")).SendKeys("password");

            var loginButton = chromeDriver.FindElement(By.XPath("//input[@value='Login']"));
            loginButton.Click();

            wait.Until(driver => driver.FindElement(By.XPath("//a[@href='/Manage']")).Displayed);
            var helloMessage = chromeDriver.FindElement(By.XPath("//a[@href='/Manage']"));
            Assert.AreEqual("hello admin@admin.com!", helloMessage.Text);

            var myPortal = chromeDriver.FindElement(By.XPath("//a[@href='/Home/Portal']"));
            Assert.AreEqual("my portal", myPortal.Text);

           
            var logOff = chromeDriver.FindElement(By.LinkText("log off"));
            logOff.Click();

            chromeDriver.FindElement(By.XPath("//a[@href='#loginModal']")).Click();

            var login = chromeDriver.FindElement(By.XPath("//a[@href='#loginModal']"));
            Assert.IsTrue(login.Displayed);
            Assert.AreEqual("login", login.Text);
            
        }

        [TestMethod]
        public void LoginFailedTest() {
            chromeDriver.Navigate().GoToUrl("http://teamd.azurewebsites.net/");
            var loginModal = chromeDriver.FindElement(By.Id("loginModal"));

            Assert.IsFalse(loginModal.Displayed);

            chromeDriver.FindElement(By.XPath("//a[@href='#loginModal']")).Click();
            WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(5));
            wait.Until(driver => driver.FindElement(By.Id("loginModal")).Displayed);
            //Assert.IsTrue(loginModal.Displayed);

            loginModal = chromeDriver.FindElement(By.Id("loginModal"));
            Assert.IsTrue(loginModal.Displayed);

            loginModal.FindElement(By.Id("Email")).SendKeys("admin@admin.com");
            loginModal.FindElement(By.Id("Password")).SendKeys("wrongpassword");

            var loginButton = chromeDriver.FindElement(By.XPath("//input[@value='Login']"));
            loginButton.Click();

            var login = chromeDriver.FindElement(By.XPath("//form[@action='/Account/Login']"));

            var error = login.FindElements(By.XPath("//div[@class='validation-summary-errors text-danger']"));
            foreach(var errorr in error) {
                if (errorr.Displayed) {
                    var errorMessage = errorr.FindElement(By.TagName("ul")).FindElement(By.TagName("li"));
                    Assert.AreEqual("Invalid login attempt.", errorMessage.Text);
                }
            }
        }



        [TestCleanup]
        public void TestCleanup() {
            if (chromeDriver != null) chromeDriver.Quit();
        }
        /*
        [TestMethod]
        public void RegisterTest() {
            //var user = await _acountController.UserManager.FindByEmailAsync("heylenmatthias@gmail.com");
            //await _acountController.UserManager.DeleteAsync(user);
            //UserManager _userManager = new UserManager(ContextEnum.BeatBuddy);
            //var userr = _userManager.ReadUser("heylenmatthias@gmail.com");
            //_userManager.DeleteUser(userr.Id);

            /*
            chromeDriver.FindElement(By.Id("registerLink")).Click();

            chromeDriver.FindElement(By.Id("Email")).SendKeys("heylenmatthias@gmail.com");
            chromeDriver.FindElement(By.Id("FirstName")).SendKeys("Matthias");
            chromeDriver.FindElement(By.Id("LastName")).SendKeys("Heylen");
            chromeDriver.FindElement(By.Id("NickName")).SendKeys("acidshards");
            chromeDriver.FindElement(By.Id("Password")).SendKeys("Password1");
            chromeDriver.FindElement(By.Id("ConfirmPassword")).SendKeys("Password1");

            var helloMessage = chromeDriver.FindElement(By.XPath("//a[@href='/Manage']"));
            
        }*/
    }

}
