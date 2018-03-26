using AspNet.Identity.MongoDB;
using MongoDB.Bson.Serialization.Attributes;
using MRP.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP.DAL.Models
{
    [BsonIgnoreExtraElements]
    public class User : IdentityUser
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public MedicalInstitution MadicalInstitution { get; set; }
    }
}
