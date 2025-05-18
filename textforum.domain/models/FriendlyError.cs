using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace textforum.domain.models
{
    public class FriendlyError
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public string CorrelationId { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this); // Serializing the exception to JSON
        }
    }
}
