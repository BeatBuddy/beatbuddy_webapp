using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
        private IOrganisationManager organisationManager; 

        //[ClassInitialize()]
        //public void OrganisationManagerTestInitialize()
        //{
        //   organisationManager = new OrganisationManager();
        //}

        [TestMethod]
        public void TestAddOrganisation()
        {
            var mockSet = new Mock<DbSet<Organisation>>();
            var mockContext = new Mock<EFDbContext>();
            mockContext.Setup(m => m.Organisations).Returns(mockSet.Object);
            organisationManager = new OrganisationManager(mockContext.Object);

            mockSet.Verify(m => m.Add(It.IsAny<Organisation>()), Times.Never);

            User organiser = new User()
            {
                FirstName = "Jonah",
                LastName = "Jordan"
            };
            organisationManager.CreateOrganisation("Jonah's Songs", "", "black", "", organiser);
            mockSet.Verify(m => m.Add(It.IsAny<Organisation>()), Times.Once);

        }
    }
}
