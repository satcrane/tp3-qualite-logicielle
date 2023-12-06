using _14E_TP2_A23.Models;
using System;
using System.Threading.Tasks;

namespace _14E_TP2_A23.Services.Authentication
{
    /// <summary>
    /// Interface du service d'authentification
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Affiche si l'utilisateur est connecté
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// Connecter un utilisateur
        /// </summary>
        /// <param name="username">Nom d'utilisateur</param>
        /// <param name="password">Mot de passe NON HACHÉ</param>
        /// <returns>True si connexion réussie</returns>
        Task<bool> Login(string username, string password);

        /// <summary>
        /// Créer un compte d'employé
        /// </summary>
        /// <param name="employee">L'employé à ajouter</param>
        /// <returns>True si employé est créé</returns>
        /// <exception cref="Exception">Levée si le employé avec ce nom existe déja.</exception>
        Task<bool> Signup(Employee employee);

        /// <summary>
        /// Déconnecter l'utilisateur
        /// </summary>
        void Logout();


        /// <summary>
        /// Récupérer l'employé connecté
        /// </summary>
        Employee? GetCurrentLoggedInUser();
    }
}
