using _14E_TP2_A23.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace _14E_TP2_A23.Services.CustomerManagement
{
    /// <summary>
    /// Interface du service de gestion des clients
    /// </summary>
    public interface ICustomerManagementService
    {
        /// <summary>
        /// Ajoute un client
        /// </summary>
        /// <param name="customer">Client à ajouter</param>
        /// <returns>True si l'ajout est réussi</returns>
        /// <exception cref="Exception">Si le client existe déjà</exception>
        Task<bool> AddCustomer(Customer customer);

        /// <summary>
        /// Récupère tous les clients
        /// </summary>
        Task<ObservableCollection<Customer>> GetAllCustomers();

        /// <summary>
        /// Modifie un client
        /// </summary>
        /// <param name="customer">Le client à mettre à jour</param>
        /// <returns>True si la modification est réussie</returns>
        /// <exception cref="Exception">Si le client n'existe pas</exception>
        Task<bool> UpdateCustomer(Customer customer);
    }
}
