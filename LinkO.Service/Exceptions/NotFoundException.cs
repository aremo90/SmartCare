using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Service.Exceptions
{
    public abstract class NotFoundException(string message) : Exception(message){}

    public class AddressNotFoundException(int id) : NotFoundException($"Address With Id {id} Not Found"){}
    public class DeviceNotFoundException(int id) : NotFoundException($"Device With Id {id} Not Found"){}
    public class GpsNotFoundException(int id) : NotFoundException($"GPS Location Not Found"){}
    public class MedicineNotFoundException(int id) : NotFoundException($"Medicine With Id {id} Not Found"){}
    public class ProductNotFoundException(int id) : NotFoundException($"Product With Id {id} Not Found"){}
    public class BasketNotFoundException(string id) : NotFoundException($"Cart With Id {id} Not Found"){}
}
