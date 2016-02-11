using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using BB.BL.Domain.Organisations;
using BB.BL.Domain.Playlists;
using BB.BL.Domain.Users;
using BB.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BB.BL.Tests
{
    [TestClass]
    public class OrganisationManagerUnitTest
    {
        private IOrganisationManager _organisationManager; 

       
        [TestMethod]
        public void TestAddOrganisation()
        {
            var mockSet = new Mock<DbSet<Organisation>>();
            var mockContext = new Mock<EFDbContext>();
            mockContext.Setup(m => m.Organisations).Returns(mockSet.Object);
            _organisationManager = new OrganisationManager(mockContext.Object);
            mockSet.Verify(m => m.Add(It.IsAny<Organisation>()), Times.Never);
            User organiser = new User()
            {
                FirstName = "Jonah",
                LastName = "Jordan"
            };
            _organisationManager.CreateOrganisation("Jonah's Songs","www.google.be", "black", "sleutel", organiser);

            mockSet.Verify(m => m.Add(It.IsAny<Organisation>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
