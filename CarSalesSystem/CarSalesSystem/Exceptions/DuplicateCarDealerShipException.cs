using System;

namespace CarSalesSystem.Exceptions
{
    public class DuplicateCarDealerShipException : Exception
    {
        public DuplicateCarDealerShipException(string message) : base(message)
        {
        }
    }
}
