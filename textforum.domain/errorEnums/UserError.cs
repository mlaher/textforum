using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace textforum.domain.errorEnums
{
    public enum UserError
    {
        INVALID_CREDENTIALS, 
        USER_NOT_SAVING,
        USER_EXISTS,
        USER_NOT_FOUND
    }
}
