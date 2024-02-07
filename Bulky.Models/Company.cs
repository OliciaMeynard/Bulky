using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [ValidateNever]
        public string? StreetAddress { get; set; }
        [ValidateNever]
        public string? City { get; set; }
        [ValidateNever]
        public string? State { get; set; }
        [ValidateNever]
        public string? PostalCode { get; set; }
        [ValidateNever]
        public string? PhoneNumber { get; set; }
    }
}
