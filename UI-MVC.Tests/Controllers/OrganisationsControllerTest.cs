using System;
using System.Web.Mvc;
using BB.UI.Web.MVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BB.UI.Web.MVC.Tests.Controllers
{
    [TestClass]
    public class OrganisationsControllerTest
    {
        [TestMethod]
        public void OrganisationsIndexText()
        {
            OrganisationsController controller = new OrganisationsController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }
    }
}
