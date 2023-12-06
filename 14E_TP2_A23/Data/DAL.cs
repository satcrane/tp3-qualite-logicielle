using _14E_TP2_A23.Models;
using _14E_TP2_A23.Services;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace _14E_TP2_A23.Data
{
    /// <summary>
    /// Représente la couche d'accès aux données.
    /// </summary>
    public sealed class DAL : IDALService
    {
        #region Propriétés
        /// <summary>
        /// Nom de la base de donnée
        /// </summary>
        private static readonly string DB_NAME = "TP3DB";

        /// <summary>
        /// Nom de la collection des utilisateurs
        /// </summary>
        private static readonly string COLLECTION_EMPLOYEES = "Employees";

        /// <summary>
        /// Nom de la collection des clients
        /// </summary>
        private static readonly string COLLECTION_CUSTOMERS = "Customers";

        /// <summary>
        /// Nom de la collection des murs d'escalade
        /// </summary>
        private static readonly string COLLECTION_CLIMBING_WALLS = "ClimbingWalls";

        /// <summary>
        /// Nom de la collection des voies
        /// </summary>
        private static readonly string COLLECTION_CLIMBING_ROUTES = "ClimbingRoutes";

        ///// <summary>
        ///// Client Mongo
        ///// </summary>
        private readonly IMongoClient _mongoClient;

        /// <summary>
        /// Base de donnée Mongo
        /// </summary>
        private readonly IMongoDatabase _database;

        #endregion

        #region Constructeur
        public DAL(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            _database = _mongoClient.GetDatabase(DB_NAME);
        }
        #endregion

        #region Méthodes

        /// <summary>
        /// Ajoute un employé dans la base de donnée
        /// </summary>
        /// <param name="employee">Employé</param>
        /// <returns>True si opération a fonctionné</returns>
        /// <exception cref="Exception">Lève une exception si la collection n'existe pas ou si l'employé existe déjà.</exception>
        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            // Récupérer la collection
            var collectionEmployee = _database.GetCollection<Employee>(COLLECTION_EMPLOYEES);
            if (collectionEmployee == null)
            {
                throw new Exception($"La collection {COLLECTION_EMPLOYEES} n'existe pas");
            }

            var employeeExists = await collectionEmployee.Find(e => e.Username == employee.Username).FirstOrDefaultAsync();
            if (employeeExists != null)
            {
                throw new Exception("L'employé existe déjà");
            }

            var salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(employee.Password, salt);

            employee.Salt = salt;
            employee.Password = passwordHash;

            await collectionEmployee.InsertOneAsync(employee);
            return true;
        }

        /// <summary>
        /// Trouver un employé par son nom d'utilisateur
        /// </summary>
        /// <param name="username">Nom d'utilisateur</param>
        /// <returns>L'employé</returns>
        /// <exception cref="Exception">Lève une exception si la collection Employees n'existe pas dans la base de données.</exception>
        public async Task<Employee?> FindEmployeeByUsernameAsync(string username)
        {
            var collectionEmployee = _database.GetCollection<Employee>(COLLECTION_EMPLOYEES);
            if (collectionEmployee == null)
            {
                throw new Exception($"La collection {COLLECTION_EMPLOYEES} n'existe pas");
            }

            var employee = await collectionEmployee.Find(e => e.Username == username).FirstOrDefaultAsync();

            return employee;
        }

        /// <summary>
        /// Récupérer tous les employés de la base de données
        /// </summary>
        /// <returns>Les employés</returns>
        public async Task<ObservableCollection<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                var collectionEmployees = _database.GetCollection<Employee>(COLLECTION_EMPLOYEES);
                if (collectionEmployees == null)
                {
                    throw new Exception($"La collection {COLLECTION_EMPLOYEES} n'existe pas");
                }

                var employees = await collectionEmployees.Find(e => true).ToListAsync();
                return new ObservableCollection<Employee>(employees);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Mettre à jour un employé
        /// </summary>
        /// <exception cref="Exception">Lève une exception si la collection Employees n'existe pas dans la base de données, si l'employé n'existe pas</exception>
        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                var collectionEmployees = _database.GetCollection<Employee>(COLLECTION_EMPLOYEES);
                if (collectionEmployees == null)
                {
                    throw new Exception($"La collection {COLLECTION_EMPLOYEES} n'existe pas");
                }

                var employeeExists = await collectionEmployees.Find(c => c.Username == employee.Username).FirstOrDefaultAsync();

                if (employeeExists == null)
                {
                    throw new Exception("L'employé n'existe pas");
                }

                var newEmployee = Builders<Employee>.Update
                    .Set(c => c.IsAdmin, employee.IsAdmin);

                var result = await collectionEmployees.UpdateOneAsync(c => c.Username == employee.Username, newEmployee);

                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Ajouter un client dans la base de données
        /// </summary>
        /// <param name="customer">Le client à ajouter</param>
        /// <returns>True si le client est ajouté</returns>
        /// <exception cref="Exception">Lève une exception si la collection Customers n'existe pas dans la base de données</exception>
        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            try
            {
                var collectionCustomers = _database.GetCollection<Customer>(COLLECTION_CUSTOMERS);
                if (collectionCustomers == null)
                {
                    throw new Exception($"La collection {COLLECTION_CUSTOMERS} n'existe pas");
                }

                await collectionCustomers.InsertOneAsync(customer);

                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Trouver un client par son email
        /// </summary>
        /// <param name="email">Email du client</param>
        /// <returns></returns>
        /// <exception cref="Exception">Lève une exception si la collection Customers n'existe pas dans la base de données, si le client n'existe pas</exception>
        public async Task<Customer?> FindCustomerByEmailAsync(string email)
        {
            try
            {
                var collectionCustomers = _database.GetCollection<Customer>(COLLECTION_CUSTOMERS);
                if (collectionCustomers == null)
                {
                    throw new Exception($"La collection {COLLECTION_CUSTOMERS} n'existe pas");
                }

                var customer = await collectionCustomers.Find(c => c.Email == email).FirstOrDefaultAsync();
                return customer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Récupérer tous les clients de la base de données
        /// </summary>
        /// <returns>Une liste des clients</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ObservableCollection<Customer>> GetAllCustomersAsync()
        {
            try
            {
                var collectionCustomers = _database.GetCollection<Customer>(COLLECTION_CUSTOMERS);
                if (collectionCustomers == null)
                {
                    throw new Exception($"La collection {COLLECTION_CUSTOMERS} n'existe pas");
                }

                var costumers = await collectionCustomers.Find(c => true).ToListAsync();
                return new ObservableCollection<Customer>(costumers);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Mettre à jour un client dans la base de données
        /// </summary>
        /// <param name="customer">Le client à mettre a jour</param>
        /// <returns>True si le client a été modifié</returns>
        /// <exception cref="Exception">Lève une exception si la collection Customers n'existe pas dans la base de données, si le client n'existe pas</exception>
        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                var collectionCustomers = _database.GetCollection<Customer>(COLLECTION_CUSTOMERS);
                if (collectionCustomers == null)
                {
                    throw new Exception($"La collection {COLLECTION_CUSTOMERS} n'existe pas");
                }

                var customerExists = await collectionCustomers.Find(c => c.Email == customer.Email).FirstOrDefaultAsync();
                if (customerExists == null)
                {
                    throw new Exception("Le client n'existe pas");
                }

                var newCustomer = Builders<Customer>.Update
                    .Set(c => c.FullName, customer.FullName)
                    .Set(c => c.Email, customer.Email)
                    .Set(c => c.IsMembershipActive, customer.IsMembershipActive)
                    .Set(c => c.MembershipStartDate, customer.MembershipStartDate);

                var result = await collectionCustomers.UpdateOneAsync(c => c.Email == customer.Email, newCustomer);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Méthodes ClimbingWalls

        /// <summary>
        /// Récupérer tous les murs d'escalade
        /// </summary>
        /// <returns>Les murs d'escalades</returns>
        public async Task<ObservableCollection<ClimbingWall>> GetAllClimbingWallsAsync()
        {
            try
            {
                var collectionClimbingWalls = _database.GetCollection<ClimbingWall>(COLLECTION_CLIMBING_WALLS);
                if (collectionClimbingWalls == null)
                {
                    throw new Exception($"La collection {COLLECTION_CLIMBING_WALLS} n'existe pas");
                }

                var climbingWalls = await collectionClimbingWalls.Find(c => true).ToListAsync();
                return new ObservableCollection<ClimbingWall>(climbingWalls);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Trouver un mur d'escalade par son nom
        /// </summary>
        /// <param name="name">Nom du mur d'escalade</param>
        /// <returns>Le mur d'escalade</returns>
        public async Task<ClimbingWall?> FindClimbingWallByNameAsync(string name)
        {
            try
            {
                var collectionClimbingWalls = _database.GetCollection<ClimbingWall>(COLLECTION_CLIMBING_WALLS);
                if (collectionClimbingWalls == null)
                {
                    throw new Exception($"La collection {COLLECTION_CLIMBING_WALLS} n'existe pas");

                }

                return await collectionClimbingWalls.Find(el => el.Location == name).FirstOrDefaultAsync();
            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Récupérer toutes les voies d'escalade
        /// </summary>
        /// <returns>Les vois d'escalades</returns>
        public async Task<ObservableCollection<ClimbingRoute>> GetAllClimbingRoutesAsync()
        {
            try
            {
                var collectionClimbingRoutes = _database.GetCollection<ClimbingRoute>(COLLECTION_CLIMBING_ROUTES);
                if (collectionClimbingRoutes == null)
                {
                    throw new Exception($"La collection {COLLECTION_CLIMBING_ROUTES} n'existe pas");
                }

                var climbingRoutes = await collectionClimbingRoutes.Find(c => true).ToListAsync();
                return new ObservableCollection<ClimbingRoute>(climbingRoutes);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Trouver une voie d'escalade par son nom
        /// </summary>
        /// <param name="name">Le nom de la voie</param>
        /// <returns>La voie trouvée</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ClimbingRoute?> FindClimbingRouteByNameAsync(string name)
        {
            try
            {
                var collectionClimbingRoutes = _database.GetCollection<ClimbingRoute>(COLLECTION_CLIMBING_ROUTES);
                if (collectionClimbingRoutes == null)
                {
                    throw new Exception($"La collection {COLLECTION_CLIMBING_ROUTES} n'existe pas");

                }

                return await collectionClimbingRoutes.Find(el => el.Name == name).FirstOrDefaultAsync();

            }

            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Ajouter une voie d'esclade
        /// </summary>
        /// <param name="climbingRoute">La voie d'escalade à ajouter</param>
        /// <returns>True si ajoutée</returns>
        public async Task<bool> AddClimbingRouteAsync(ClimbingRoute climbingRoute)
        {
            try
            {
                var collectionClimbingRoutes = _database.GetCollection<ClimbingRoute>(COLLECTION_CLIMBING_ROUTES);
                if (collectionClimbingRoutes == null)
                {
                    throw new Exception($"La collection {COLLECTION_CLIMBING_ROUTES} n'existe pas");

                }

                await collectionClimbingRoutes.InsertOneAsync(climbingRoute);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Déassigner une voie d'escalade à un mur
        /// </summary>
        /// <param name="climbingRoute">La voie d'escalade à déassigner</param>
        /// <returns>True si la voie d'escalade n'est plus associée à un mur</returns>
        public async Task<bool> UnassignClimbingRouteAsync(ClimbingRoute climbingRoute)
        {
            try
            {
                var collectionClimbingRoutes = _database.GetCollection<ClimbingRoute>(COLLECTION_CLIMBING_ROUTES);
                if (collectionClimbingRoutes == null)
                {
                    throw new Exception($"La collection {COLLECTION_CLIMBING_ROUTES} n'existe pas");
                }

                var unassignedClimbingRoute = Builders<ClimbingRoute>.Update
                    .Set(c => c.WallId, null);

                await collectionClimbingRoutes.UpdateOneAsync(c => c.Name == climbingRoute.Name, unassignedClimbingRoute);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Assigner une voie d'escalade à un mur
        /// </summary>
        /// <param name="climbingRoute">Voie d'escalade à assigner</param>
        /// <param name="climbingWall">Mur d'escalade au quel assigner la voie d'escalade</parm>
        /// <returns>True si l'assignation a réussi, false sinon</returns>
        public async Task<bool> AssignClimbingRouteToClimbingWallAsync(ClimbingRoute climbingRoute, ClimbingWall climbingWall)
        {
            try
            {

                var collectionClimbingRoutes = _database.GetCollection<ClimbingRoute>(COLLECTION_CLIMBING_ROUTES);
                if (collectionClimbingRoutes == null)
                {
                    throw new Exception($"La collection {COLLECTION_CLIMBING_ROUTES} n'existe pas");
                }

                var collectionClimbingWalls = _database.GetCollection<ClimbingWall>(COLLECTION_CLIMBING_WALLS);
                if (collectionClimbingWalls == null)
                {
                    throw new Exception($"La collection {COLLECTION_CLIMBING_WALLS} n'existe pas");
                }

                // Retirer la voie d'escalade de son mur actuel
                var unassignedClimbingRoute = Builders<ClimbingRoute>.Update
                    .Set(c => c.WallId, null);
                await collectionClimbingRoutes.UpdateOneAsync(c => c.Name == climbingRoute.Name, unassignedClimbingRoute);

                // Assigner la voie d'escalade au mur
                var assignedClimbingRoute = Builders<ClimbingRoute>.Update
                    .Set(c => c.WallId, climbingWall.Id);
                await collectionClimbingRoutes.UpdateOneAsync(c => c.Name == climbingRoute.Name, assignedClimbingRoute);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Ajouter une note de difficulté à une voie d'escalade
        /// </summary>
        /// <param name="climbingRoute">Voie d'escalade à noter</param>
        /// <param name="difficulty">Note de difficulté</param>
        public async Task<bool> AddClimbingRouteDifficultyRatingAsync(ClimbingRoute climbingRoute, double difficulty)
        {
            try
            {
                var collectionClimbingRoutes = _database.GetCollection<ClimbingRoute>(COLLECTION_CLIMBING_ROUTES);
                if (collectionClimbingRoutes == null)
                {
                    throw new Exception($"La collection {COLLECTION_CLIMBING_ROUTES} n'existe pas");
                }

                var climbingRouteExists = collectionClimbingRoutes.Find(c => c.Name == climbingRoute.Name).FirstOrDefault();
                if (climbingRouteExists == null)
                {
                    throw new Exception("La voie d'escalade n'existe pas");
                }

                // Ajouter la note de difficulté à la voie d'escalade
                var climbingRouteDifficultyRating = Builders<ClimbingRoute>.Update
                     .Push(c => c.DifficultyRatings, difficulty);

                await collectionClimbingRoutes.UpdateOneAsync(c => c.Name == climbingRoute.Name, climbingRouteDifficultyRating);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion
    }

}