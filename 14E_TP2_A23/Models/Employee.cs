using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _14E_TP2_A23.Models
{
    /// <summary>
    /// Représente un employé
    /// </summary>
    public class Employee
    {
        #region Propriétés
        /// <summary>
        /// Id de l'employé. Auto-généré par MongoDB
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Nom d'utilisateur de l'employé
        /// </summary>
        [BsonElement("username")]
        public string? Username { get; set; }

        /// <summary>
        /// Mot de passe de l'employé hashé
        /// </summary>
        [BsonElement("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Sel utilisé pour hasher le mot de passe (salt)
        /// </summary>
        [BsonElement("salt")]
        public string? Salt { get; set; }

        /// <summary>
        /// Si employé est administrateur
        /// </summary>
        [BsonElement("isAdmin")]
        public bool? IsAdmin { get; set; }
        #endregion

        #region Constructeur
        public Employee()
        {

        }

        public Employee(string username, string password, bool? isAdmin)
        {
            Username = username;
            Password = password;

            if (isAdmin == null)
                IsAdmin = false;
            else
                IsAdmin = isAdmin;
        }

        #endregion

    }
}
