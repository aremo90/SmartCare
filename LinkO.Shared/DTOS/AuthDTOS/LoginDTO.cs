using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.DTOS.AuthDTOS
{
    public record LoginDTO ([EmailAddress]string Email , string Password);
}
