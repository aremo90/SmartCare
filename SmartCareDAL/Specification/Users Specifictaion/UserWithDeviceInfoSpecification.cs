using SmartCareBLL.Specification;
using SmartCareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareDAL.Specification.Users_Specifictaion
{
    public class UserWithDeviceInfoSpecification : Specification<User>
    {
        public UserWithDeviceInfoSpecification() :base(null)
        {
            AddInclude(P => P.Device);
        }

        public UserWithDeviceInfoSpecification(int id) : base(u => u.Id == id)
        {
            AddInclude(P => P.Device);
        }
    }
}
