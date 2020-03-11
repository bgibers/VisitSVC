using System;
using System.ComponentModel.DataAnnotations;

namespace Visit.Service.Models
{
    public class RegisterModelApi
    {
        public string Username { get; set; }
        public string Password { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public string Avi { get; set; }
        
        public long FacebookId { get; set; }
        public int? FkBirthLocationId { get; set; }
        public int? FkResidenceLocationId { get; set; }
    }
}