using _14E_TP2_A23.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace _14E_TP2_A23.Services.EmployeesManagement
{

    /// <summary>
    /// Interface du service de gestion des employées
    /// </summary>
    public interface IEmployeeManagementService
    {
        /// <summary>
        /// Récupérer tous les employées de la base de données
        /// </summary>
        /// <returns>Les employés</returns>
        Task<ObservableCollection<Employee>> GetAllEmployees();

        /// <summary>
        /// Modifie un employé
        /// </summary>
        /// <param name="customer">Employé à modifier</param>
        /// <returns>True si la modification est réussie</returns>
        /// <exception cref="Exception">Si l'employé n'existe pas</exception>
        Task<bool> UpdateEmployee(Employee employee);
    }
}
