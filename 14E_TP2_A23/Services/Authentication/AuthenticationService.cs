using _14E_TP2_A23.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;

namespace _14E_TP2_A23.Services.Authentication
{
    public partial class AuthenticationService : ObservableObject, IAuthenticationService
    {
        #region Propriétés
        private readonly IDALService _dal;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsLoggedIn))]
        private Employee? _currentEmployee;

        /// <summary>
        /// Affiche si l'utilisateur est connecté
        /// </summary>
        public bool IsLoggedIn => CurrentEmployee != null;
        #endregion

        #region Constructeur
        public AuthenticationService(IDALService dalService)
        {
            _dal = dalService;
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Tente de connecter un utilisateur avec les identifiants fournis.
        /// </summary>
        /// <param name="username">Nom d'utilisateur.</param>
        /// <param name="password">Mot de passe NON HACHE.</param>
        /// <returns>True si la connexion est réussie, sinon une exception est levée.</returns>
        /// <exception cref="Exception">Levée si le DAL est non défini, si l'identifiant est incorrect, ou si le mot de passe ne correspond pas.</exception>
        public async Task<bool> Login(string username, string password)
        {
            var employee = await _dal.FindEmployeeByUsernameAsync(username);
            if (employee == null)
            {
                throw new Exception("Identifiant et/ou mot de passe incorrect.");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, employee.Password))
            {
                throw new Exception("Nom d'utilisateur et/ou mot de passe incorrect");
            }

            CurrentEmployee = employee;

            return true;
        }

        /// <summary>
        /// Créer un compte d'employé
        /// </summary>
        /// <param name="employee">L'employé à ajouter</param>
        /// <returns>True si employé est créé</returns>
        /// <exception cref="Exception">Levée si le employé avec ce nom existe déja.</exception>
        public async Task<bool> Signup(Employee employee)
        {
            var existingEmployee = await _dal.FindEmployeeByUsernameAsync(employee.Username);
            if (existingEmployee != null)
            {
                throw new Exception("Un utilisateur avec son nom existe deja.");
            }

            await _dal.AddEmployeeAsync(employee);

            return true;
        }

        /// <summary>
        /// Déconnecte l'utilisateur courant et efface toutes ses données de session.
        /// </summary>
        public void Logout()
        {
            CurrentEmployee = null;
        }

        /// <summary>
        /// Récupère l'utilisateur courant.
        /// </summary>
        public Employee? GetCurrentLoggedInUser()
        {
            return CurrentEmployee;
        }


        #endregion
    }
}
