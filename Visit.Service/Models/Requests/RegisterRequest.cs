using System;
using System.ComponentModel.DataAnnotations;

namespace Visit.Service.Models.Requests
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8}$", 
            ErrorMessage = "Password must meet requirements")]
        public string Password { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        
        public long FacebookId { get; set; }
        public int? FkBirthLocationId { get; set; }
        public int? FkResidenceLocationId { get; set; }
    }
}