using SmartCareDAL.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartCareBLL.DTOS.AuthDTOS
{
    public class SignUpDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
            
        [Required]
        public string Password { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
