using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using HealthCatalystTest.Controllers;
using HealthCatalystTest.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace HealthCatalystTest.Tests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestAbout()
        {
            var mockUsersSet = new Mock<DbSet<UserInformationModel>>();

            var mockUserContext = new Mock<UserInformationContext>();
            mockUserContext.Setup(m => m.UserInformation).Returns(mockUsersSet.Object);

            var homeService = new HomeController(mockUserContext.Object);


            var about = homeService.About() as ViewResult;
            Assert.AreEqual(about.ViewName, "About");
            

        }

        private HomeController createHomeController(List<UserInformationModel> users, NameValueCollection inputParameters)
        {
            var userData = users.AsQueryable();

            var mockUsersSet = new Mock<DbSet<UserInformationModel>>();
            mockUsersSet.As<IQueryable<UserInformationModel>>().Setup(u => u.Provider).Returns(userData.Provider);
            mockUsersSet.As<IQueryable<UserInformationModel>>().Setup(u => u.Expression).Returns(userData.Expression);
            mockUsersSet.As<IQueryable<UserInformationModel>>().Setup(u => u.ElementType).Returns(userData.ElementType);
            mockUsersSet.As<IQueryable<UserInformationModel>>().Setup(u => u.GetEnumerator()).Returns(userData.GetEnumerator());


            var mockUserContext = new Mock<UserInformationContext>();
            mockUserContext.Setup(u => u.UserInformation).Returns(mockUsersSet.Object);

            var homeService = new HomeController(mockUserContext.Object);

            var mockRequest = new Mock<HttpRequestBase>();
            var queryString = inputParameters;
            mockRequest.Setup(r => r.QueryString).Returns(queryString);
            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
            var mockConrollerContext = new Mock<ControllerContext>();
            mockConrollerContext.Setup(cc => cc.HttpContext).Returns(mockContext.Object);

            homeService.ControllerContext = mockConrollerContext.Object;

            return homeService;
        }

        [TestMethod]
        public void TestQueryOneUser()
        {

            //Data

            var userData = new List<UserInformationModel>
            {
                new UserInformationModel { FirstName = "test", LastName = "test"}
            };

            var queryString = new NameValueCollection {
                { "search_criteria", "test" },
                {"page_size", "20" },
                {"page_number", "0" } };


            var homeService = createHomeController(userData, queryString);

            var result = homeService.Search().Data as List<UserInformationModel>;

            Assert.AreEqual(result[0].FirstName, "test");
            Assert.AreEqual(result[0].LastName, "test");
            
        }

        [TestMethod]
        public void TestNoResults()
        {
            var userData = new List<UserInformationModel>
            {
                new UserInformationModel { FirstName = "test", LastName = "test"}
            };

           var queryString = new NameValueCollection {
                { "search_criteria", "asdf" },
                {"page_size", "20" },
                {"page_number", "0" } };

            var homeService = createHomeController(userData, queryString);

            var result = homeService.Search().Data as List<UserInformationModel>;

            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public void TestMultiResults()
        {
            var userData = new List<UserInformationModel>
            {
                new UserInformationModel { FirstName = "test", LastName = "test", id = 1},
                new UserInformationModel { FirstName = "test1", LastName = "test2", id = 2  }
            };
            
            var queryString = new NameValueCollection {
                { "search_criteria", "test" },
                {"page_size", "20" },
                {"page_number", "0" } };

            var homeService = createHomeController(userData, queryString);

            var result = homeService.Search().Data as List<UserInformationModel>;

            Assert.AreEqual(result.Count, 2);
        }

        [TestMethod]
        public void TestOneOfMulti()
        {
            var userData = new List<UserInformationModel>
            {
                new UserInformationModel { FirstName = "test", LastName = "test", id = 1},
                new UserInformationModel { FirstName = "test1", LastName = "test2", id = 2  }
            };

            var queryString = new NameValueCollection {
                { "search_criteria", "test1" },
                {"page_size", "20" },
                {"page_number", "0" } };

            var homeService = createHomeController(userData, queryString);

            var result = homeService.Search().Data as List<UserInformationModel>;

            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0].FirstName, "test1");
        }

        [TestMethod]
        public void TestNoDuplicateOnDuplicateSearch()
        {
            var userData = new List<UserInformationModel>
            {
                new UserInformationModel { FirstName = "test", LastName = "test", id = 1},
                new UserInformationModel { FirstName = "test1", LastName = "test2", id = 2  }
            };

            var queryString = new NameValueCollection {
                { "search_criteria", "test test" },
                {"page_size", "20" },
                {"page_number", "0" } };

            var homeService = createHomeController(userData, queryString);

            var result = homeService.Search().Data as List<UserInformationModel>;

            Assert.AreEqual(result.Count, 2);
        }

        [TestMethod]
        public void TestStrictMatchReturnOne()
        {
            var userData = new List<UserInformationModel>
            {
                new UserInformationModel { FirstName = "test", LastName = "test", id = 1},
                new UserInformationModel { FirstName = "test1", LastName = "test2", id = 2  }
            };

            var queryString = new NameValueCollection {
                { "search_criteria", "test" },
                {"page_size", "20" },
                {"page_number", "0" },
                {"strict_match", "true" } };

            var homeService = createHomeController(userData, queryString);

            var result = homeService.Search().Data as List<UserInformationModel>;

            Assert.AreEqual(result.Count, 1);
        }
    }
}
