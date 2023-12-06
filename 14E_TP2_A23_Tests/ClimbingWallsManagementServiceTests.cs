
using _14E_TP2_A23.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using _14E_TP2_A23.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _14E_TP2_A23.Services.ClimbingWalls;

namespace _14E_TP2_A23_Tests

{
    // Convention des methodes : < MethodName > _should_ < expectation > _when_<condition>

    [TestClass]
    public class ClimbingManagementTests
    {
        [TestMethod]
        public async Task GetAllClimbingWallsAsync_should_return_walls_when_walls_exist()
        {
            var dalServiceMock = new Mock<IDALService>();
            var climbingWall1 = new ClimbingWall();
            var climbingWall2 = new ClimbingWall();


            // Arrange
            dalServiceMock.Setup(x => x.GetAllClimbingWallsAsync())
                         .ReturnsAsync(new ObservableCollection<ClimbingWall> 
                         { climbingWall1, climbingWall2 });


            var climbingWallsService = new ClimbingManagementService(dalServiceMock.Object);

            // Act
            var result = await climbingWallsService.GetAllClimbingWalls();

            // Assert
            Assert.AreEqual(2, result.Count);
        }


        [TestMethod]
        public async Task GetAllClimbingWalls_should_fail_when_no_walls_exist()
        {
            var dalServiceMock = new Mock<IDALService>();


            // Arrange
            var mockCollection = new List<ClimbingWall> { };
            dalServiceMock.Setup(x => x.GetAllClimbingWallsAsync())
                         .ReturnsAsync(new ObservableCollection<ClimbingWall>());

            var climbingWallsService = new ClimbingManagementService(dalServiceMock.Object);

            // Act
            var result = await climbingWallsService.GetAllClimbingWalls();

            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task AddClimbingRoutes_should_fail_when_route_already_exists()
        {
            var dalServiceMock = new Mock<IDALService>();

            // Arrange
            var existingRoute = new ClimbingRoute { Name = "newRoute" };
            dalServiceMock.Setup(dal => dal.FindClimbingRouteByNameAsync(existingRoute.Name)).ReturnsAsync(existingRoute);

            var climbingManagementService = new ClimbingManagementService(dalServiceMock.Object);

            // Act
            var newRoute = new ClimbingRoute { Name = "newRoute" };
            await climbingManagementService.AddClimbingRoute(newRoute);

        }

        [TestMethod]
        public async Task AddClimbingRoute_should_return_true_when_route_is_added()
        {
            var dalServiceMock = new Mock<IDALService>();

            // Arrange
            dalServiceMock.Setup(dal => dal.FindClimbingRouteByNameAsync("newRoute")).ReturnsAsync((ClimbingRoute)null);
            dalServiceMock.Setup(dal => dal.AddClimbingRouteAsync(It.IsAny<ClimbingRoute>())).ReturnsAsync(true);

            // Act
            var newRoute = new ClimbingRoute { Name = "newRoute" };

            var climbingManagementService = new ClimbingManagementService(dalServiceMock.Object);
            var result = await climbingManagementService.AddClimbingRoute(newRoute);

            // Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public async Task GetAllClimbingRoutesAsync_should_return_walls_when_walls_exist()
        {
            var dalServiceMock = new Mock<IDALService>();
            var climbingRoute1 = new ClimbingRoute();
            var climbingRoute2 = new ClimbingRoute();


            // Arrange
            dalServiceMock.Setup(x => x.GetAllClimbingRoutesAsync())
                         .ReturnsAsync(new ObservableCollection<ClimbingRoute>
                         { climbingRoute1, climbingRoute2 });


            var climbingWallsService = new ClimbingManagementService(dalServiceMock.Object);

            // Act
            var result = await climbingWallsService.GetAllClimbingRoutes();

            // Assert
            Assert.AreEqual(2, result.Count);
        }



        [TestMethod]
        public async Task GetAllClimbingRoutesAsync_should_return_empty_collection_when_no_routes_exist()
        {
            var dalServiceMock = new Mock<IDALService>();


            // Arrange
            var mockCollection = new List<ClimbingRoute> { };
            dalServiceMock.Setup(x => x.GetAllClimbingRoutesAsync())
                         .ReturnsAsync(new ObservableCollection<ClimbingRoute>());

            var climbingWallsService = new ClimbingManagementService(dalServiceMock.Object);

            // Act
            var result = await climbingWallsService.GetAllClimbingRoutes();

            //Assert
            Assert.AreEqual(0, result.Count);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UnassignClimbingRoute_should_fail_when_no_route_exists()
        {
            var dalServiceMock = new Mock<IDALService>();


            // Arrange
            var existingRoute = new ClimbingRoute { Name = "testRoute" };
            dalServiceMock.Setup(dal => dal.FindClimbingRouteByNameAsync(existingRoute.Name)).ReturnsAsync((ClimbingRoute)null);


            var climbingWallsService = new ClimbingManagementService(dalServiceMock.Object);

            // Act
            var result = await climbingWallsService.UnassignClimbingRoute(existingRoute);
        }


        [TestMethod]
        public async Task UnassignClimbing_should_return_true_when_route_and_wall_exist()
        {
            var dalServiceMock = new Mock<IDALService>();


            // Arrange
            var existingRoute = new ClimbingRoute { Name = "testRoute" };
            dalServiceMock.Setup(dal => dal.FindClimbingRouteByNameAsync(existingRoute.Name)).ReturnsAsync(existingRoute);
            dalServiceMock.Setup(dal => dal.UnassignClimbingRouteAsync(existingRoute)).ReturnsAsync(true);

            var climbingWallsService = new ClimbingManagementService(dalServiceMock.Object);

            // Act
            var result = await climbingWallsService.UnassignClimbingRoute(existingRoute);

            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public async Task AssignClimbingRoute_should_return_true_when_wall_and_route_exist() 
        {
            var dalServiceMock = new Mock<IDALService>();

            var existingRoute = new ClimbingRoute { Name = "testRoute" };
            var existingWall = new ClimbingWall { Location = "Zone-A" };

            dalServiceMock.Setup(dal => dal.FindClimbingRouteByNameAsync(existingRoute.Name)).ReturnsAsync(existingRoute);
            dalServiceMock.Setup(dal => dal.FindClimbingWallByNameAsync(existingWall.Location)).ReturnsAsync(existingWall);
            dalServiceMock.Setup(dal => dal.AssignClimbingRouteToClimbingWallAsync(existingRoute, existingWall)).ReturnsAsync(true);

            var climbingWallsService = new ClimbingManagementService(dalServiceMock.Object);

            // Act
            var result = await climbingWallsService.AssignClimbingRouteToClimbingWall(existingRoute, existingWall);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task AssignClimbingRoute_should_fail_when_route_doesntt_exist()
        {
            var dalServiceMock = new Mock<IDALService>();

            var existingRoute = new ClimbingRoute { Name = "testRoute" };
            var existingWall = new ClimbingWall { Location = "Zone-A" };

            dalServiceMock.Setup(dal => dal.FindClimbingRouteByNameAsync(existingRoute.Name)).ReturnsAsync((ClimbingRoute)null);
            dalServiceMock.Setup(dal => dal.FindClimbingWallByNameAsync(existingWall.Location)).ReturnsAsync(existingWall);
            dalServiceMock.Setup(dal => dal.AssignClimbingRouteToClimbingWallAsync(existingRoute, existingWall)).ReturnsAsync(true);

            var climbingWallsService = new ClimbingManagementService(dalServiceMock.Object);

            // Act
            var result = await climbingWallsService.AssignClimbingRouteToClimbingWall(existingRoute, existingWall);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task AssignClimbingRoute_should_fail_when_wall_doesnt_exist()
        {
            var dalServiceMock = new Mock<IDALService>();

            var existingRoute = new ClimbingRoute { Name = "testRoute" };
            var existingWall = new ClimbingWall { Location = "Zone-A" };

            dalServiceMock.Setup(dal => dal.FindClimbingRouteByNameAsync(existingRoute.Name)).ReturnsAsync(existingRoute);
            dalServiceMock.Setup(dal => dal.FindClimbingWallByNameAsync(existingWall.Location)).ReturnsAsync((ClimbingWall)null);
            dalServiceMock.Setup(dal => dal.AssignClimbingRouteToClimbingWallAsync(existingRoute, existingWall)).ReturnsAsync(true);

            var climbingWallsService = new ClimbingManagementService(dalServiceMock.Object);

            // Act
            var result = await climbingWallsService.AssignClimbingRouteToClimbingWall(existingRoute, existingWall);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task AssignClimbingRoute_should_fail_when_wall_and_route_doesnt_exist()
        {
            var dalServiceMock = new Mock<IDALService>();

            var existingRoute = new ClimbingRoute { Name = "testRoute" };
            var existingWall = new ClimbingWall { Location = "Zone 2" };

            dalServiceMock.Setup(dal => dal.FindClimbingRouteByNameAsync(existingRoute.Name)).ReturnsAsync((ClimbingRoute)null);
            dalServiceMock.Setup(dal => dal.FindClimbingWallByNameAsync(existingWall.Location)).ReturnsAsync((ClimbingWall)null);
            dalServiceMock.Setup(dal => dal.AssignClimbingRouteToClimbingWallAsync(existingRoute, existingWall)).ReturnsAsync(false);

            var climbingWallsService = new ClimbingManagementService(dalServiceMock.Object);

            // Act
            var result = await climbingWallsService.AssignClimbingRouteToClimbingWall(existingRoute, existingWall);
        }

        [TestMethod]
        public async Task AssignClimbingRoute_should_fail_when_route_is_already_assigned()
        {
            var dalServiceMock = new Mock<IDALService>();

            var existingRoute = new ClimbingRoute { Name = "testRoute" };
            var existingWall = new ClimbingWall { Location = "Zone 2" };

            dalServiceMock.Setup(dal => dal.FindClimbingRouteByNameAsync(existingRoute.Name)).ReturnsAsync(existingRoute);
            dalServiceMock.Setup(dal => dal.FindClimbingWallByNameAsync(existingWall.Location)).ReturnsAsync(existingWall);
            dalServiceMock.Setup(dal => dal.AssignClimbingRouteToClimbingWallAsync(existingRoute, existingWall)).ReturnsAsync(false);

            var climbingWallsService = new ClimbingManagementService(dalServiceMock.Object);

            // Act
            var result = await climbingWallsService.AssignClimbingRouteToClimbingWall(existingRoute, existingWall);

            Assert.IsFalse(result);
        }
    }
}
