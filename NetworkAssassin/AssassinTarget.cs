using System.Management;
using ROOT.CIMV2.Win32;

namespace NetworkAssassin
{
    class AssassinTarget
    {
        public AssassinTarget(NetworkAdapter adapter)
        {
            Id = adapter.DeviceID;
            Name = adapter.Name;
            Status = adapter.NetConnectionStatus;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }
}
