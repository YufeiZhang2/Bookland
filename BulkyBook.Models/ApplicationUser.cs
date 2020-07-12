using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BulkyBook.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }
        
        [NotMapped] 
        public string Role { get; set; }
        
        [ForeignKey("CompanyId")]
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}