using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using BB.UI.Web.MVC.Controllers;

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
            AccountController _acountController = new AccountController();
        }

        [TestMethod]
        public void LoginTest()
        {
            chromeDriver.Navigate().GoToUrl("http://teamd.azurewebsites.net/");
            var loginModal = chromeDriver.FindElement(By.Id("loginModal"));

            Assert.IsFalse(loginModal.Displayed);

            chromeDriver.FindElement(By.XPath("//a[@href='#loginModal']")).Click();

            loginModal = chromeDriver.FindElement(By.Id("loginModal"));
            Assert.IsTrue(loginModal.Displayed);

            loginModal.FindElement(By.Id("Email")).SendKeys("admin@admin.com");
            loginModal.FindElement(By.Id("Password")).SendKeys("password");

            var loginButton = chromeDriver.FindElement(By.XPath("//input[@value='Login']"));
            loginButton.Click();
            var helloMessage = chromeDriver.FindElement(By.XPath("//a[@href='/Manage']"));
            Assert.AreEqual("hello admin@admin.com!", helloMessage.Text);
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
