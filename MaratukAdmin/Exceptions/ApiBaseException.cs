using Microsoft.AspNetCore.Mvc;

namespace MaratukAdmin.Exceptions
{
    public class ApiBaseException : Exception
    {
        private readonly IDictionary<string, object> _extensions;
        public string Type { get; private set; }
        public string Instance { get; private set; }
        public int? Status { get; private set; }

        public ApiBaseException(int? statusCode,
                            string message = "",
                            string type = "",
                            string instance = "",
                            IDictionary<string, object> extensions = null) : base(message)
        {
            Type = type;
            Instance = instance;
            Status = statusCode;
            _extensions = extensions;
        }

        public ProblemDetails ToProblemDetails()
        {
            var problemDetails = new ProblemDetails
            {
                Status = Status,
            };

            if (!String.IsNullOrWhiteSpace(Instance))
            {
                problemDetails.Extensions.Add(nameof(Instance).ToLower(), Instance);
            }

            if (String.IsNullOrWhiteSpace(Type))
            {
                Type = $"https://httpstatuses.io/{Status}";
            }
            problemDetails.Extensions.Add(nameof(Type).ToLower(), Type);

            if (_extensions != null)
            {
                foreach (var extension in _extensions)
                {
                    problemDetails.Extensions.Add(extension.Key, extension.Value);
                }
            }

            return problemDetails;
        }
    }
}
