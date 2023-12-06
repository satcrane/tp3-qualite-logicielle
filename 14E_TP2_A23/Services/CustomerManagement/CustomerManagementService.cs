using _14E_TP2_A23.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace _14E_TP2_A23.Services.CustomerManagement
{
    public partial class CustomerManagementService : ObservableObject, ICustomerManagementService
    {
        #region Propriétés
        private readonly IDALService _dal;
        #endregion

        #region Constructeur
        public CustomerManagementService(IDALService dalService)
        {
            _dal = dalService;
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Ajoute un client dans la base de données
        /// </summary>
        /// <param name="customer">Le client</param>
        /// <returns>True si le client est ajouté</returns>
        /// <exception cref="Exception">Si le client existe déjà</exception>
        public async Task<bool> AddCustomer(Customer customer)
        {
            try
            {
                var customerAlreadyExists = await _dal.FindCustomerByEmailAsync(customer.Email);
                if (customerAlreadyExists != null)
                {
                    throw new Exception("Le client existe déjà");
                }

                return await _dal.AddCustomerAsync(customer);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Récupérer tous les clients de la base de données
        /// </summary>
        /// <returns>La liste des clients</returns>
        public async Task<ObservableCollection<Customer>> GetAllCustomers()
        {
            try
            {
                return await _dal.GetAllCustomersAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Mettre à jour un client dans la base de données
        /// </summary>
        /// <param name="customer">Le client à mettre à jour</param>
        /// <returns>True si le client a ete modifie</returns>
        /// <exception cref="Exception">Si le client existe déjà</exception></exception>
        public async Task<bool> UpdateCustomer(Customer customer)
        {
            try
            {
                var customerAlreadyExists = await _dal.FindCustomerByEmailAsync(customer.Email);
                if (customerAlreadyExists == null)
                {
                    throw new Exception("Le client n'existe pas");
                }
                return await _dal.UpdateCustomerAsync(customer);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
