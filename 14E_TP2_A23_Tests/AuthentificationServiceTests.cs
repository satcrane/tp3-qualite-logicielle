using _14E_TP2_A23.Models;
using _14E_TP2_A23.Services;
using _14E_TP2_A23.Services.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace _14E_TP2_A23_Tests
{
    // Convention des methodes : < MethodName > _should_ < expectation > _when_<condition>

    [TestClass]
    public class AuthenticationServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task Login_shoud_fail_when_employee_doesn_not_exists()
        {
            // Arrange
            var username = "user";
            var password = "password";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var employee = new Employee { Username = username, Password = hashedPassword };

            var dalServiceMock = new Mock<IDALService>();

            dalServiceMock.Setup(x => x.FindEmployeeByUsernameAsync(employee.Username)).ReturnsAsync(() => null);
            var authenticationService = new AuthenticationService(dalServiceMock.Object);

            // Act
            var result = await authenticationService.Login(employee.Username, employee.Password);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task Login_should_fail_when_password_is_invalid()
        {
            // Arrange
            var username = "user";
            var password = "password";
            var fakePassword = "fakePassword";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var employee = new Employee { Username = username, Password = hashedPassword };

            var dalServiceMock = new Mock<IDALService>();

            dalServiceMock.Setup(x => x.FindEmployeeByUsernameAsync(employee.Username)).ReturnsAsync(employee);
            var authenticationService = new AuthenticationService(dalServiceMock.Object);

            // Act
            var result = await authenticationService.Login(username, fakePassword);
        }

        [TestMethod]
        public async Task Login_should_return_true_when_credentials_are_valid()
        {
            // Arrange
            var dalServiceMock = new Mock<IDALService>();
            var username = "user";
            var password = "password";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var employee = new Employee { Username = username, Password = hashedPassword };

            dalServiceMock.Setup(x => x.FindEmployeeByUsernameAsync(username)).ReturnsAsync(employee);
            var authenticationService = new AuthenticationService(dalServiceMock.Object);

            // Act
            var result = await authenticationService.Login(username, password);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task Signup_should_fail_when_employee_already_exists()
        {
            // Arrange
            var dalServiceMock = new Mock<IDALService>();
            var username = "user";
            var password = "password";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var employee = new Employee { Username = username, Password = hashedPassword };

            dalServiceMock.Setup(x => x.FindEmployeeByUsernameAsync(username)).ReturnsAsync(employee);
            var authenticationService = new AuthenticationService(dalServiceMock.Object);

            // Act
            var result = await authenticationService.Signup(employee);
        }

        [TestMethod]
        public async Task Signup_should_return_true_when_employee_is_created()
        {
            // Arrange
            var dalServiceMock = new Mock<IDALService>();
            var username = "user";
            var password = "password";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var employee = new Employee { Username = username, Password = hashedPassword };

            dalServiceMock.Setup(x => x.FindEmployeeByUsernameAsync(username)).ReturnsAsync(() => null);
            var authenticationService = new AuthenticationService(dalServiceMock.Object);

            // Act
            var result = await authenticationService.Signup(employee);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Logout_should_logout_current_employees_when_logging_out()
        {
            // 1. Login > puis > 2. Logout

            // Arrange
            var dalServiceMock = new Mock<IDALService>();
            var username = "user";
            var password = "password";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var employee = new Employee { Username = username, Password = hashedPassword };

            dalServiceMock.Setup(x => x.FindEmployeeByUsernameAsync(username)).ReturnsAsync(employee);
            var authenticationService = new AuthenticationService(dalServiceMock.Object);

            // Act
            var result = await authenticationService.Login(username, password);

            // Act
            authenticationService.Logout();

            // Assert
            Assert.IsNull(authenticationService.CurrentEmployee);
        }
    }
}
