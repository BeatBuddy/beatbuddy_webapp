using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using BB.UI.Web.MVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BB.UI.Web.MVC.Tests.Helpers;
using Moq;

namespace BB.UI.Web.MVC.Tests.Controllers
{
    [TestClass]
    public class CommentControllerTest
    {
        private CommentController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            controller = CreateCommentControllerAs("jonah@gmail.com");
            DbInitializer.Initialize();
        }

        CommentController CreateCommentControllerAs(string userName)
        {

            var mock = new Mock<ControllerContext>();
            mock.SetupGet(p => p.HttpContext.User.Identity.Name).Returns(userName);
            mock.SetupGet(p => p.HttpContext.Request.IsAuthenticated).Returns(true);

            var controller = new CommentController(DbInitializer.CreatePlaylistManager())
            {
                ControllerContext = mock.Object
            };
            return controller;
        }

        [TestMethod]
        public void TestAddComment()
        {
            var identity = new GenericIdentity("jonah@gmail.com");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);

            var commentResult = (controller.GetComments(1) as PartialViewResult);

            Assert.IsNotNull(commentResult);

            var comments = commentResult.Model as IEnumerable<Comment>;

            Assert.IsNotNull(comments);

            var commentCount = comments.Count();

            var createCommentResult = controller.Create(new Comment
            {
                Text = "Hello world!",
                TimeStamp = DateTime.Now,
                User = new User { Id = 1, Email = "jonah@gmail.com" }
            }, 1);

            Assert.IsNotNull(createCommentResult);

            commentResult = (controller.GetComments(1) as PartialViewResult);
            comments = commentResult?.Model as IEnumerable<Comment>;

            Assert.IsNotNull(comments);

            var newCommentCount = comments.Count();

            Assert.AreEqual(commentCount + 1, newCommentCount);
        }
    }
}
