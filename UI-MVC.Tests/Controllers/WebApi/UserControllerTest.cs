using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BB.UI.Web.MVC.Controllers.Web_API;
using BB.BL.Domain;
using BB.UI.Web.MVC.Tests.Helpers;
using System.Net.Http;
using System.Linq;
using System.Web;
using Moq;

namespace BB.UI.Web.MVC.Tests.Controllers.WebApi
{
    [TestClass]
    public class UserControllerTest
    {
        private UserController controller;
        

        [TestInitialize]
        public void TestInitialize() {
            controller = new UserController(ContextEnum.BeatBuddyTest);
            DbInitializer.Initialize();
            

        }

        private static IDisposable _webApp;

        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
           // _webApp = WebApp.Start<Startup>("http://*:6969/");
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            _webApp.Dispose();
        }


        /*
        [TestMethod]
        public void Login()
        {

            //HttpContext.Current.Session["admin@admin.com"] = "admin";

            var body = new List<KeyValuePair<string, string>>();
            body.Add(new KeyValuePair<string, string>("grant_type", "password"));
            body.Add(new KeyValuePair<string, string>("username", "admin@admin.com"));
            body.Add(new KeyValuePair<string, string>("password", "password"));
            HttpResponseMessage val;
            var httpBody =
             string.Join("&",
                   body.Select(kvp =>
                        string.Format("{0}={1}", kvp.Key, kvp.Value)));
            
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var requestUri = new Uri("http://localhost:6969/api/token");

                var response = httpClient.PostAsync(requestUri, new FormUrlEncodedContent(body)).Result;
                response.EnsureSuccessStatusCode();
                val = response;
            }

            Assert.IsNotNull(val);
            

        }*/

    }

     
    }

