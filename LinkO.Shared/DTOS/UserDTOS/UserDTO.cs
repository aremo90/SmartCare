using LinkO.Shared.DTOS.EnumDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.UserDTOS
{
    public class UserDTO
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GenderDTO Gender { get; set; }
        public string Email { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = null!;
    }
}
