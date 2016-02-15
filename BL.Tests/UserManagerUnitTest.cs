using System;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using BB.BL.Domain.Users;
using BB.DAL;
using System.Collections.Generic;
using System.Linq;

namespace BB.BL.Tests
{
    [TestClass]
    public class UserManagerUnitTest
    {
        private IUserManager _userManager;
        private Mock<DbSet<User>> _userMockSet;
        private Mock<EFDbContext> _userMockContext;

        [TestInitialize]
        public void Initialize()
        {
            var users = new List<User>()
            {
                new User()
                {
                    Email = "jonah.jordan@gmail.com"
                },
                new User()
                {
                    Email = "maarten.vangiel@gmail.com"
                }
            };
            var sourceList = users.AsQueryable();
            _userMockContext = new Mock<EFDbContext>();
            _userMockSet = new Mock<DbSet<User>>();
            _userMockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(sourceList.Provider);
            _userMockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(sourceList.Expression);
            _userMockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(sourceList.ElementType);
            _userMockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => sourceList.GetEnumerator());
            _userMockContext.Setup(m => m.User).Returns(_userMockSet.Object);
            _userMockSet.Setup(m => m.Add(It.IsAny<User>())).Callback((User u) => users.Add(u));

        }

        [TestMethod]
        public void TestAddNewUser()
        {
            _userManager = new UserManager(_userMockContext.Object);
            _userManager.CreateUser("lennart.boeckx@gmail.com", "password1", "boeckx", "lennart", "lb", null);

            _userMockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once);
            _userMockContext.Verify(m => m.SaveChanges(), Times.Once);

            var user = _userMockSet.Object.ElementAt(2);
            Assert.AreEqual(_userMockSet.Object.Count(), 3);
            Assert.AreEqual(user.Email, "lennart.boeckx@gmail.com");
            Assert.AreNotEqual(user.Email, "test");
            Assert.IsTrue(user.Roles != null);
            Assert.IsNotNull(user.Id);
        }
    }
}
