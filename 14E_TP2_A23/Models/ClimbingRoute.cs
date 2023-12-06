using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;

namespace _14E_TP2_A23.Models
{
    /// <summary>
    /// Représente une voie d'escalade
    /// </summary>
    public class ClimbingRoute
    {
        #region Propriétés
        /// <summary>
        /// Id de la voie. Auto-généré par MongoDB
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Mur auquel la voie est associée
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("wallId")]
        public string? WallId { get; set; }

        /// <summary>
        /// Moyenne des notes de difficulté de la voie. Utilisé pour la coloration des routes
        /// </summary>
        [BsonIgnore]
        public double AverageDifficultyRating { get; set; }

        /// <summary>
        /// Nom de la voie
        /// </summary>
        [BsonElement("name")]
        public string Name { get; set; }


        /// <summary>
        /// Difficulté de la voie
        /// </summary>
        [BsonElement("difficulty")]
        public double Difficulty { get; set; }

        /// <summary>
        /// Couleur des prises de la voie
        /// </summary>
        [BsonElement("holdsColor")]
        public string HoldsColor { get; set; }

        /// <summary>
        /// Notes de difficulté de la voie
        /// </summary>
        [BsonElement("difficultyRatings")]
        public List<double> DifficultyRatings { get; set; }

        #endregion

        #region Propriétés "computed"
        /// <summary>
        /// Si assignée à un mur.
        /// Utilisé pour la coloration des routes.
        /// </summary>
        [BsonIgnore]
        public bool IsAssignedToAWall { get; set; }

        /// <summary>
        /// Si assignée au mur actuel.
        /// Utilisé pour la coloration des routes.
        /// </summary>
        [BsonIgnore]
        public bool IsAssignedToCurrentAWall { get; set; }

        /// <summary>
        /// Nom du mur auquel la voie est assignée
        /// </summary>
        [BsonIgnore]
        public string? WallNameRouteIsAssigned { get; set; }

        #endregion

        #region Constructeur
        public ClimbingRoute()
        {
            DifficultyRatings = new List<double>();
            //Ratings = new List<ClimbingRouteEvaluation>();
        }

        public ClimbingRoute(float difficulty, string holdsColor)
        {
            Difficulty = difficulty;
            HoldsColor = holdsColor;
            DifficultyRatings = new List<double>();
            //Ratings = new List<ClimbingRouteEvaluation>();
        }

        public ClimbingRoute(float difficulty, string holdsColor, List<double> difficultyRatings)
        {
            Difficulty = difficulty;
            HoldsColor = holdsColor;
            DifficultyRatings = difficultyRatings;
            //Ratings = ratings;
        }

        #endregion

    }
}
