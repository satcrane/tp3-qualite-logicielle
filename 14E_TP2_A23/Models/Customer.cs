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
    /// Représente un client
    /// </summary>
    public class Customer
    {
        #region Propriétés
        /// <summary>
        /// Id du client. Auto-généré par MongoDB
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Nom complet du client
        /// </summary>
        [BsonElement("fullname")]
        public string FullName { get; set; }

        /// <summary>
        /// Courriel du client
        /// </summary>
        [BsonElement("email")]
        public string Email { get; set; }

        /// <summary>
        /// Date de début de l'abonnement du client
        /// </summary>
        [BsonElement("membershipStartDate")]
        public DateTime? MembershipStartDate { get; set; }

        /// <summary>
        /// Si l'abonnement du client est actif
        /// </summary>
        [BsonElement("isMembershipActive")]
        public bool IsMembershipActive { get; set; }
        #endregion

        #region Constructeur
        public Customer()
        {

        }

        public Customer(string fullname, string email, DateTime membershipStartDate, bool isMembershipActive)
        {
            FullName = fullname;
            Email = email;
            MembershipStartDate = membershipStartDate;
            IsMembershipActive = isMembershipActive;
        }

        #endregion
    }
}
