using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Shared.CommonResult
{
    public class Error
    {

        public string Code { get;}
        public string Description { get;}
        public ErrorType Type { get;}

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public static Error Failure(string Code = "General Failure", string Description = "General Failure occured !")
        {
            return new Error(Code, Description , ErrorType.Failure);
        }
        public static Error Validation(string Code = "Validation Error", string Description = "Validation Error occured !")
        {
            return new Error(Code , Description, ErrorType.Validation);
        }
        public static Error NotFound(string Code = "Not Found", string Description = "Error 404 Not Found !")
        {
            return new Error(Code , Description, ErrorType.NotFound);
        }
        public static Error Unauthorized(string Code = "Unauthorized Error", string Description = "Unauthorized occured !")
        {
            return new Error(Code , Description , ErrorType.Unauthorized);
        }
        public static Error Forbidden(string Code = "Forbidden Error", string Description = "Forbidden Error occured !")
        {
            return new Error(Code , Description , ErrorType.Forbidden);
        }
        public static Error InvalidCredentials(string Code = "InvalidCredentials Errors", string Description = "InvalidCredentials Errors occured !")
        {
            return new Error(Code , Description , ErrorType.InvalidCredentials);
        }


    }
}
