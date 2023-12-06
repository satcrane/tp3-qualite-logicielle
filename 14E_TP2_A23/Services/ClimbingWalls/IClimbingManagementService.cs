using _14E_TP2_A23.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _14E_TP2_A23.Services.ClimbingWalls
{
    /// <summary>
    /// Interface du service de gestion des murs d'escalade
    /// </summary>
    public interface IClimbingManagementService
    {
        /// <summary>
        /// Réupère tous les murs d'escalade
        /// </summary>
        Task<ObservableCollection<ClimbingWall>> GetAllClimbingWalls();

        /// <summary>
        /// Récupère toutes les voies d'esclades
        /// </summary>
        Task<ObservableCollection<ClimbingRoute>> GetAllClimbingRoutes();

        /// <summary>
        /// Ajouter une voie d'escalade
        /// </summary>
        /// <param name="climbingRoute">Voie d'escalade à ajouter</param>
        /// <returns>True si l'ajout a réussi, false sinon</returns>
        Task<bool> AddClimbingRoute(ClimbingRoute climbingRoute);

        /// <summary>
        /// Déassigner une voie d'escalade de son mur
        /// </summary>
        /// <param name="climbingRoute">Voie d'escalade à déassigner</param>
        /// <returns>True si la déassignation a réussi, false sinon</returns>
        Task<bool> UnassignClimbingRoute(ClimbingRoute climbingRoute);

        /// <summary>
        /// Assigner une voie d'escalade à un mur
        /// </summary>
        /// <param name="climbingRoute">Voie d'escalade à assigner</param>
        /// <param name="climbingWall">Mur d'escalade au quel assigner la voie d'escalade</parm>
        /// <returns>True si l'assignation a réussi, false sinon</returns>
        Task<bool> AssignClimbingRouteToClimbingWall(ClimbingRoute climbingRoute, ClimbingWall climbing);

        /// <summary>
        /// Ajouter une note de difficulté à une voie d'escalade
        /// </summary>
        /// <param name="climbingRoute">Voie d'escalade à noter</param>
        /// <param name="difficulty">Note de difficulté</param>
        Task<bool> AddClimbingRouteDifficultyRating(ClimbingRoute climbingRoute, double difficulty);
    }
}
