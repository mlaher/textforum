using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using textforum.domain.models;

namespace textforum.domain.exceptions
{
    public class AppException : Exception
    {
        public string? FriendlyError { get; set; }
        public readonly string CorrelationId;

        public AppException(string message, string correlationId) : base(message)
        {
            CorrelationId = correlationId;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this); // Serializing the exception to JSON
        }

        public FriendlyError GetFriendlyErrorDetails()
        {
            return new FriendlyError()
            {
                HasError = true,
                Message = FriendlyError ?? "An unknown error occurred.",
                CorrelationId = CorrelationId
            };
        }

        public TechnicalError GetTechnicalErrorDetails()
        {
            return new TechnicalError()
            {
                FriendlyError = GetFriendlyErrorDetails(),
                Exception = this
            };
        }
    }
}
