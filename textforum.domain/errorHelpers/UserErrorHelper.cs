using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using textforum.domain.errorEnums;

namespace textforum.domain.errorHelpers
{
    public static class UserErrorHelper
    {
        private static readonly Dictionary<UserError, string> FriendlyErrorMessages = new Dictionary<UserError, string>()
        {
            {
                UserError.INVALID_CREDENTIALS,
                "Invalid user authentication credentials provided."
            },
            { 
                UserError.USER_NOT_SAVING, 
                "An error has occured while trying to save user. Please try again in a while or contact support if problem persists." 
            },
            {
                UserError.USER_EXISTS,
                "This user being added already exists"
            },
            {
                UserError.USER_NOT_FOUND,
                "Invalid user credendentials provided"
            }

        };

        public static string GetFriendlyErrorMessage(UserError errorCode)
        {
            return FriendlyErrorMessages.ContainsKey(errorCode)
                ? FriendlyErrorMessages[errorCode]
                : "An unknown user error has occurred.";
        }

        public static string GetErrorCode(UserError errorCode)
        {
            return errorCode.ToString();
        }
    }
}
