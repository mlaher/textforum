namespace textforum.domain.models
{
    public class App
    {
        public string IPAddresses { get; set; }
        private HashSet<string> _ipAddressesList = new HashSet<string>();
        public HashSet<string> IPAddressesList
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(IPAddresses) && _ipAddressesList.Count == 0)
                {
                    _ipAddressesList = IPAddresses.Split(',').ToHashSet();
                }    

                return _ipAddressesList;
            }
        }
        private HashSet<string> _machineNamesList = new HashSet<string>();
        public HashSet<string> MachineNamesList
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(MachineNames) && _machineNamesList.Count == 0)
                {
                    _machineNamesList = MachineNames.Split(',').ToHashSet();
                }

                return _machineNamesList;
            }
        }
        public string MachineNames { get; set; }
    }
}
