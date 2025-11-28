using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.AuthDTOS
{
    public record UserInfoDTO (string FirstName , string LastName , [EmailAddress]string Email , string Gender , DateOnly DateOfBirth ,[Phone] string PhoneNumber);

}
