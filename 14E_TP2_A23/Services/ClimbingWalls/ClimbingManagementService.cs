using _14E_TP2_A23.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace _14E_TP2_A23.Services.ClimbingWalls
{
    /// <summary>
    /// Service de gestion des murs d'escalade
    /// </summary>
    public partial class ClimbingManagementService : ObservableObject, IClimbingManagementService
    {
        #region Propriétés
        private readonly IDALService _dal;
        #endregion

        #region Constructeur
        public ClimbingManagementService(IDALService dalService)
        {
            _dal = dalService;
        }


        #endregion

        #region Méthodes

        /// <summary>
        /// Réupère tous les murs d'escalades
        /// </summary>
        /// <returns>Une liste des mures d'escalades</returns>
        public async Task<ObservableCollection<ClimbingWall>> GetAllClimbingWalls()
        {
            try
            {
                return await _dal.GetAllClimbingWallsAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Réupère tous les murs d'escalades
        /// </summary>
        /// <returns>Une liste des murs d'esclades</returns>
        public async Task<ObservableCollection<ClimbingRoute>> GetAllClimbingRoutes()
        {
            try
            {
                return await _dal.GetAllClimbingRoutesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Ajouter une voie d'escalade
        /// </summary>
        /// <param name="climbingRoute">Voie d'escalade à ajouter</param>
        public async Task<bool> AddClimbingRoute(ClimbingRoute climbingRoute)
        {
            try
            {
                var climbingRouteAlreadyExists = await _dal.FindClimbingRouteByNameAsync(climbingRoute.Name);
                if (climbingRouteAlreadyExists != null)
                {
                    throw new Exception("La voie d'escalade avec ce nom existe déjà");
                }

                return await _dal.AddClimbingRouteAsync(climbingRoute);
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
        /// <returns></returns>
        public async Task<bool> UnassignClimbingRoute(ClimbingRoute climbingRoute)
        {
            try
            {
                var climbingRouteExists = await _dal.FindClimbingRouteByNameAsync(climbingRoute.Name);
                if (climbingRouteExists == null)
                {
                    throw new Exception("La voie d'escalade n'existe pas");
                }
                return await _dal.UnassignClimbingRouteAsync(climbingRoute);
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
        public async Task<bool> AssignClimbingRouteToClimbingWall(ClimbingRoute climbingRoute, ClimbingWall climbingWall)
        {
            try
            {
                var climbingRouteExists = await _dal.FindClimbingRouteByNameAsync(climbingRoute.Name);
                if (climbingRouteExists == null)
                {
                    throw new Exception("La voie d'escalade n'existe pas");
                }

                var climbingWallExists = await _dal.FindClimbingWallByNameAsync(climbingWall.Location);
                if (climbingWallExists == null)
                {
                    throw new Exception("Le mur d'escalade n'existe pas");
                }

                return await _dal.AssignClimbingRouteToClimbingWallAsync(climbingRoute, climbingWall);
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
        public async Task<bool> AddClimbingRouteDifficultyRating(ClimbingRoute climbingRoute, double difficulty)
        {
            try
            {
                var climbingRouteExists = await _dal.FindClimbingRouteByNameAsync(climbingRoute.Name);
                if (climbingRouteExists == null)
                {
                    throw new Exception("La voie d'escalade n'existe pas");
                }

                return await _dal.AddClimbingRouteDifficultyRatingAsync(climbingRoute, difficulty);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
