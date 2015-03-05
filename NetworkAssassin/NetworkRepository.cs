using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace NetworkAssassin
{
    public static class NetworkRepository
    {
        /// <summary>
        /// Scans for adapters, and exposes wraps them in a collection of <c>NetworkAdapter</c> objects.
        /// </summary>
        public static IEnumerable<NetworkAdapter> GetNetworkAdapters()
        {
            const string query = "SELECT DeviceID, ProductName, Description, NetEnabled, NetConnectionStatus FROM Win32_NetworkAdapter WHERE Manufacturer <> 'Microsoft'";
            var objectSearcher = new ManagementObjectSearcher(new ObjectQuery(query));

            return from ManagementObject managementObject in objectSearcher.Get()
                   select new NetworkAdapter(managementObject);
        }

        /// <summary>
        /// Scans for adapters that match a DeviceID, and exposes wraps them in a collection of <c>NetworkAdapter</c> objects.
        /// </summary>
        public static IEnumerable<NetworkAdapter> GetNetworkAdapters(string deviceId)
        {
            string query = "SELECT DeviceID, ProductName, Description, NetEnabled, NetEnabled, NetConnectionStatus FROM Win32_NetworkAdapter WHERE DeviceID = " + deviceId;
            var objectSearcher = new ManagementObjectSearcher(new ObjectQuery(query));

            return from ManagementObject managementObject in objectSearcher.Get()
                   select new NetworkAdapter(managementObject);
        }
    }
}
