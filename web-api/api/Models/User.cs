using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace api.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string ProfilePicture { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public string PasswordKey { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }

        public List<UserRole> UserRoles { get; set; }
        public string Token { get; set; }
    }
}