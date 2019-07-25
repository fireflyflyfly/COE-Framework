using System;
using System.Text;
using RestSharp;

namespace COE.Core.Exceptions
{
    class COEResponseInvalidException : Exception
    {
        private string _message;

        public override string Message => _message;

        public COEResponseInvalidException()
        {
        }

        public COEResponseInvalidException(string message) : base(message)
        {
            _message = message;
        }

        public COEResponseInvalidException(string response, RestRequest request)
        {
            StringBuilder reaquestParameters = new StringBuilder();
            foreach (var parameter in request.Parameters)
            {
                reaquestParameters.AppendLine(parameter.ToString());
            }
            _message = $"The following response has errors :\n{response}\n Request parameters :\n{reaquestParameters}"; 
        }

        public COEResponseInvalidException(string message, Exception innerException) : base(message, innerException)
        {
            _message = message;
        }
    }
}
