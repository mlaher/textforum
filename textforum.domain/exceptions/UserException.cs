using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using textforum.domain.errorEnums;
using textforum.domain.errorHelpers;
using textforum.domain.models;

namespace textforum.domain.exceptions
{
    [Serializable]
    public class UserException : AppException
    {
        public UserException(UserError userError, string correlationId) : base(UserErrorHelper.GetErrorCode(userError), correlationId)
        {
            FriendlyError = UserErrorHelper.GetFriendlyErrorMessage(userError);
        }
    }
}
