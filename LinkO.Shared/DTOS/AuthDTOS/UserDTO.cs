using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.AuthDTOS
{
    public record UserDTO(string Email , string FirstName , string Token , string Role);
}
