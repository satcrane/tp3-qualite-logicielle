using _14E_TP2_A23.Models;
using _14E_TP2_A23.Services.EmployeesManagement;
using _14E_TP2_A23.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.ObjectModel;

namespace _14E_TP2_A23_Tests
{
    // Convention des methodes : < MethodName > _should_ < expectation > _when_<condition>

    [TestClass]
    public class EmployeeManagementServiceTests
    {
        [TestMethod]
        public async Task GetAllEmployees_should_return_all_employees_when_called()
        {
            // Arrange
            var dalServiceMock = new Mock<IDALService>();
            var employees = new ObservableCollection<Employee>
            {
                new Employee { Username = "employee1", Password = "password1" },
                new Employee { Username = "employee2", Password = "password2" }
            };

            dalServiceMock.Setup(dal => dal.GetAllEmployeesAsync()).ReturnsAsync(employees);
            var employeeManagementService = new EmployeeManagementService(dalServiceMock.Object);

            // Act
            var result = await employeeManagementService.GetAllEmployees();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UpdateEmployee_should_fail_when_employee_does_not_exist()
        {
            // Arrange
            var dalServiceMock = new Mock<IDALService>();
            var employeeToUpdate = new Employee { Username = "nonexistent", Password = "password" };

            dalServiceMock.Setup(dal => dal.FindEmployeeByUsernameAsync(employeeToUpdate.Username)).ReturnsAsync(() => null);
            var employeeManagementService = new EmployeeManagementService(dalServiceMock.Object);

            // Act
            await employeeManagementService.UpdateEmployee(employeeToUpdate);
        }

        [TestMethod]
        public async Task UpdateEmployee_should_return_true_when_employee_is_updated()
        {
            // Arrange
            var dalServiceMock = new Mock<IDALService>();
            var existingEmployee = new Employee { Username = "existingUser", Password = "password" };

            dalServiceMock.Setup(dal => dal.FindEmployeeByUsernameAsync(existingEmployee.Username)).ReturnsAsync(existingEmployee);
            dalServiceMock.Setup(dal => dal.UpdateEmployeeAsync(It.IsAny<Employee>())).ReturnsAsync(true);
            var employeeManagementService = new EmployeeManagementService(dalServiceMock.Object);

            // Act
            var result = await employeeManagementService.UpdateEmployee(existingEmployee);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
