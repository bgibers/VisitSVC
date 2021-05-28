using System;
using System.ComponentModel.DataAnnotations;

namespace Visit.Service.Models.Requests
{
    public class RegisterRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8}$", 
            ErrorMessage = "Password must meet requirements")]
        public string Password { get; set; }
        
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public string Title { get; set; }
        public string Education { get; set; }
        public long FacebookId { get; set; }
        public string BirthLocation { get; set; }
        public string ResidenceLocation { get; set; }
    }
}