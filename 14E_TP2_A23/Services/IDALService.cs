using _14E_TP2_A23.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace _14E_TP2_A23.Services
{
    /// <summary>
    /// Représente un service DAL
    /// </summary>
    public interface IDALService
    {
        /// <summary>
        /// Ajouter un employé
        /// </summary>
        Task<bool> AddEmployeeAsync(Employee employee);

        /// <summary>
        /// Trouver un employé par son nom d'utilisateur
        /// </summary>
        Task<Employee?> FindEmployeeByUsernameAsync(string username);

        /// <summary>
        /// Récupérer tous les employés
        /// </summary>
        Task<ObservableCollection<Employee>> GetAllEmployeesAsync();

        /// <summary>
        /// Mettre à jour un employé
        /// </summary>
        /// <exception cref="Exception">Lève une exception si la collection Employees n'existe pas dans la base de données, si l'employé n'existe pas</exception>
        Task<bool> UpdateEmployeeAsync(Employee employee);

        /// <summary>
        /// Ajouter un client
        /// </summary>
        Task<bool> AddCustomerAsync(Customer customer);

        /// <summary>
        /// Trouver un client par son courriel
        /// </summary>
        Task<Customer?> FindCustomerByEmailAsync(string email);

        /// <summary>
        /// Trouver tous les clients
        /// </summary>
        /// <exception cref="Exception">Si la collection n'existe pas</exception>
        Task<ObservableCollection<Customer>> GetAllCustomersAsync();

        /// <summary>
        /// Mettre à jour un client
        /// </summary>
        /// <exception cref="Exception">Lève une exception si la collection Customers n'existe pas dans la base de données, si le client n'existe pas</exception>
        Task<bool> UpdateCustomerAsync(Customer customer);

        /// <summary>
        /// Récupérer tous les murs d'escalade
        /// </summary>
        Task<ObservableCollection<ClimbingWall>> GetAllClimbingWallsAsync();

        /// <summary>
        /// Trouver un mur d'escalade par son nom
        /// </summary>
        /// <param name="name">Nom du mur d'escalade</param>
        Task<ClimbingWall?> FindClimbingWallByNameAsync(string name);

        /// <summary>
        /// Récupérer toutes les voies d'escalades
        /// </summary>
        Task<ObservableCollection<ClimbingRoute>> GetAllClimbingRoutesAsync();

        /// <summary>
        /// Trouver une voie d'escalade par son nom
        /// </summary>
        /// <param name="name">Le nom de la voie d'escalade</param>
        /// <returns>La voie d'escalade trouvée</returns>
        Task<ClimbingRoute?> FindClimbingRouteByNameAsync(string name);

        /// <summary>
        /// Ajouter une voie d'escalade
        /// </summary>
        /// <param name="climbingRoute">Voie d'escalade à ajouter</param>
        /// <returns>True si la voie d'escalade a été ajoutée, false sinon</returns>
        Task<bool> AddClimbingRouteAsync(ClimbingRoute climbingRoute);

        /// <summary>
        /// Déassigner une voie d'escalade à un mur
        /// </summary>
        /// <param name="climbingRoute">La voie d'escalade à déassigner</param>
        /// <returns>True si la voie d'escalade n'est plus associée à un mur</returns>
        Task<bool> UnassignClimbingRouteAsync(ClimbingRoute climbingRoute);

        /// <summary>
        /// Assigner une voie d'escalade à un mur
        /// </summary>
        /// <param name="climbingRoute">Voie d'escalade à assigner</param>
        /// <param name="climbingWall">Mur d'escalade au quel assigner la voie d'escalade</parm>
        /// <returns>True si l'assignation a réussi, false sinon</returns>
        Task<bool> AssignClimbingRouteToClimbingWallAsync(ClimbingRoute climbingRoute, ClimbingWall climbingWall);

        /// <summary>
        /// Ajouter une note de difficulté à une voie d'escalade
        /// </summary>
        /// <param name="climbingRoute">Voie d'escalade à noter</param>
        /// <param name="difficulty">Note de difficulté</param>
        Task<bool> AddClimbingRouteDifficultyRatingAsync(ClimbingRoute climbingRoute, double difficulty);
    }
}
