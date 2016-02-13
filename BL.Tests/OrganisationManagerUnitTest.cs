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
        private Mock<DbSet<Organisation>> _organisationMockSet;
        private Mock<EFDbContext> _organisatorMockContext; 
        
        [TestInitialize]
        public void Initialize()
        {
            var organisations = new List<Organisation>();
            var sourceList = organisations.AsQueryable();
            _organisatorMockContext = new Mock<EFDbContext>();
            _organisationMockSet = new Mock<DbSet<Organisation>>();
            _organisationMockSet.As<IQueryable<Organisation>>().Setup(m => m.Provider).Returns(sourceList.Provider);
            _organisationMockSet.As<IQueryable<Organisation>>().Setup(m => m.Expression).Returns(sourceList.Expression);
            _organisationMockSet.As<IQueryable<Organisation>>().Setup(m => m.ElementType).Returns(sourceList.ElementType);
            _organisationMockSet.As<IQueryable<Organisation>>().Setup(m => m.GetEnumerator()).Returns(() => sourceList.GetEnumerator());
            _organisatorMockContext.Setup(m => m.Organisations).Returns(_organisationMockSet.Object);
            _organisationMockSet.Setup(m => m.Add(It.IsAny<Organisation>())).Callback((Organisation s) => organisations.Add(s));
        }

        [TestMethod]
        public void TestAddNewOrganisation()
        {
            User organiser = new User()
            {
                FirstName = "Jonah",
                LastName = "Jordan"
            };   
            _organisationManager = new OrganisationManager(_organisatorMockContext.Object);
            _organisationManager.CreateOrganisation("Jonah's Songs","www.google.be", "black", "sleutel", organiser);
            
            _organisationMockSet.Verify(m => m.Add(It.IsAny<Organisation>()), Times.Once);
            _organisatorMockContext.Verify(m => m.SaveChanges(), Times.Once);

            Organisation organisation = _organisationMockSet.Object.ElementAt(0);
            Assert.AreEqual(_organisationMockSet.Object.Count(), 1);
            Assert.AreEqual(organisation.Name, "Jonah's Songs");
            Assert.AreNotEqual(organisation.Name, "Test name");
            Assert.IsTrue(organisation.Users.ContainsKey(organiser));
            Assert.IsTrue(organisation.Playlists != null);
            Assert.IsNotNull(organisation.Id);
        }

        [TestMethod]
        public void TestReadOrganisation()
        {
            
        }
    }
}
