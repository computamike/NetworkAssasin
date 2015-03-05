using System;
using System.Management;

namespace NetworkAssassin
{
    public class NetworkAdapter
    {
        private ManagementObject ManagementObject { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }
        public NetworkState? Status { get; set; }

        public NetworkAdapter(ManagementObject managementObject)
        {
            ManagementObject = managementObject;
            Id = managementObject["DeviceID"] as string;
            Name = managementObject["ProductName"] as string;
            Enabled = (managementObject["NetEnabled"] as bool?).GetValueOrDefault();
            Description = managementObject["Description"] as string;
            object status = managementObject["NetConnectionStatus"];
            if (status != null)
            {
                Status = (NetworkState)Enum.Parse(typeof(NetworkState), status.ToString());
            }
        }

        public bool Toggle()
        {
            ManagementObject.InvokeMethod(Enabled ? "Disable" : "Enable", null);
            return Enabled = !Enabled;
        }
    }
}
