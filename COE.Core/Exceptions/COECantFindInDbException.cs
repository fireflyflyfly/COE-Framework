using System;

namespace COE.Core.Exceptions
{
    class COECantFindInDbException : Exception
    {
        public COECantFindInDbException()
        {
        }

        public COECantFindInDbException(string message) : base(message)
        {
        }

        public COECantFindInDbException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
