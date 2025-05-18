using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace textforum.domain.models
{
    public class TechnicalError
    {
        public Exception Exception {  get; set; }
        public FriendlyError FriendlyError { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this); // Serializing the exception to JSON
        }
    }
}
