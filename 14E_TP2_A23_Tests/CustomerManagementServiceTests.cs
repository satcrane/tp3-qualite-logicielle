using _14E_TP2_A23.Models;
using _14E_TP2_A23.Services.CustomerManagement;
using _14E_TP2_A23.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.ObjectModel;

namespace _14E_TP2_A23_Tests
{
    // Convention des methodes : < MethodName > _should_ < expectation > _when_<condition>

    [TestClass]
    public class CustomerManagementServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task AddCustomer_shoud_fail_when_customer_already_exists()
        {
            // Arrange
            var email = "123@email.com";
            var customer = new Customer { Email = email };

            var dalServiceMock = new Mock<IDALService>();
            dalServiceMock.Setup(x => x.FindCustomerByEmailAsync(email)).ReturnsAsync(customer);

            var customerManagementService = new CustomerManagementService(dalServiceMock.Object);

            // Act
            var result = await customerManagementService.AddCustomer(customer);
        }

        [TestMethod]
        public async Task AddCustomer_should_return_true_when_customer_is_added()
        {
            // Arrange
            var email = "123@email.com";
            var customer = new Customer { Email = email };

            var dalServiceMock = new Mock<IDALService>();
            dalServiceMock.Setup(x => x.FindCustomerByEmailAsync(email)).ReturnsAsync(() => null);
            dalServiceMock.Setup(x => x.AddCustomerAsync(customer)).ReturnsAsync(true);

            var customerManagementService = new CustomerManagementService(dalServiceMock.Object);

            // Act
            var result = await customerManagementService.AddCustomer(customer);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetAllCustomers_should_return_empty_collection_when_no_customer_exist()
        {
            // Arrange
            var dalServiceMock = new Mock<IDALService>();
            dalServiceMock.Setup(x => x.GetAllCustomersAsync()).ReturnsAsync(new ObservableCollection<Customer>());

            var customerManagementService = new CustomerManagementService(dalServiceMock.Object);

            // Act
            var result = await customerManagementService.GetAllCustomers();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetAllCustomers_should_return_customers_when_customers_exist()
        {
            // Arrange
            var customer1 = new Customer { Email = "123@email.com" };
            var customer2 = new Customer { Email = "abc@email.com" };

            var dalServiceMock = new Mock<IDALService>();
            dalServiceMock.Setup(x => x.AddCustomerAsync(customer1)).ReturnsAsync(true);
            dalServiceMock.Setup(x => x.AddCustomerAsync(customer2)).ReturnsAsync(true);
            dalServiceMock.Setup(x => x.GetAllCustomersAsync()).ReturnsAsync(new ObservableCollection<Customer> { customer1, customer2 });

            var customerManagementService = new CustomerManagementService(dalServiceMock.Object);

            // Act
            var result = await customerManagementService.GetAllCustomers();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UpdateCustomer_should_fail_when_customer_does_not_exist()
        {
            // Arrange
            var customer = new Customer { Email = "" };
            var dalServiceMock = new Mock<IDALService>();
            dalServiceMock.Setup(x => x.FindCustomerByEmailAsync(customer.Email)).ReturnsAsync(() => null);

            var customerManagementService = new CustomerManagementService(dalServiceMock.Object);

            // Act
            await customerManagementService.UpdateCustomer(customer);
        }

        [TestMethod]
        public async Task UpdateCustomer_should_return_true_when_customer_is_updated()
        {
            // Arrange
            var oldCustomer = new Customer { Email = "123@email.com", IsMembershipActive = false };
            var expectedCustomer = new Customer { Email = "123@email.com", IsMembershipActive = true };

            var dalServiceMock = new Mock<IDALService>();
            dalServiceMock.Setup(dal => dal.FindCustomerByEmailAsync(oldCustomer.Email))
                .ReturnsAsync(oldCustomer);

            dalServiceMock.Setup(dal => dal.UpdateCustomerAsync(It.IsAny<Customer>()))
                .Callback<Customer>(cust => cust.IsMembershipActive = expectedCustomer.IsMembershipActive)
                .ReturnsAsync(true);

            var customerManagementService = new CustomerManagementService(dalServiceMock.Object);

            // Act
            var result = await customerManagementService.UpdateCustomer(expectedCustomer);

            // Assert
            Assert.IsTrue(result);
        }
    }
}

