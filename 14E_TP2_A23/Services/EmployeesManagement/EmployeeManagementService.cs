using _14E_TP2_A23.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace _14E_TP2_A23.Services.EmployeesManagement
{
    /// <summary>
    /// Service de gestion des employées
    /// </summary>
    public partial class EmployeeManagementService : ObservableObject, IEmployeeManagementService
    {
        #region Propriétés
        private readonly IDALService _dal;
        #endregion

        #region Constructeur
        public EmployeeManagementService(IDALService dalService)
        {
            _dal = dalService;
        }

        #endregion

        #region Méthodes
        /// <summary>
        /// Récupérer tous les employées de la base de données
        /// </summary>
        /// <returns>Liste des employées</returns>
        public async Task<ObservableCollection<Employee>> GetAllEmployees()
        {
            try
            {
                return await _dal.GetAllEmployeesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Modifie un employé
        /// </summary>
        /// <param name="customer">Employé à modifier</param>
        /// <returns>True si la modification est réussie</returns>
        /// <exception cref="Exception">Si l'employé n'existe pas</exception>
        public async Task<bool> UpdateEmployee(Employee employee)
        {
            try
            {
                var employeeAlreadyExists = await _dal.FindEmployeeByUsernameAsync(employee.Username);
                if (employeeAlreadyExists == null)
                {
                    throw new Exception("L'employé n'existe pas");
                }

                return await _dal.UpdateEmployeeAsync(employee);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
