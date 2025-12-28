using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.AuthDTOS
{
    public class UpdateUserInfo
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? PublicId { get; set; }
        public string? ProfilePicture {  get; set; }
        public string? userFcmToken { get; set; }
    }
}
