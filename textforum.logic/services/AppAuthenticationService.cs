using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using textforum.domain.interfaces;
using textforum.domain.models;

namespace textforum.logic.services
{
    public class AppAuthenticationService : IAppAuthenticationService
    {
        private readonly ValidApps _validApps;

        public AppAuthenticationService(IOptions<ValidApps> validApps)
        {
            _validApps = validApps.Value;
        }

        public bool AuthenticateApp(string appToken, string ipAddress, string machineIdentifier)
        {
            if (!_validApps.Apps.ContainsKey(appToken))
                return false;

            var validApp = _validApps.Apps[appToken];

            if (!string.IsNullOrWhiteSpace(validApp.IPAddresses))
            {
                if (!validApp.IPAddressesList.Contains(ipAddress))
                    return false;
            }

            if (!string.IsNullOrWhiteSpace(validApp.MachineNames))
            {
                if (!validApp.MachineNamesList.Contains(machineIdentifier))
                    return false;
            }

            return true;
        }
    }
}
