using ROOT.CIMV2.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkAssassin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private bool Enabled = true; 
        private Dictionary<string, int> NetworkStates = new Dictionary<string,int>();


        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var item in NetworkStates.Keys )
            {
                if (NetworkStates[item] == 2)
                {// Kill it with FIRE
                    SelectQuery query = new SelectQuery("Win32_NetworkAdapter", "DeviceID="+item.ToString() );
                    ManagementObjectSearcher search = new ManagementObjectSearcher(query);
                 
                    foreach (var adapt in search.Get())
                    {
                        
                        ManagementObject adapter =  (ManagementObject)adapt;
                        if (Enabled)
                        { adapter.InvokeMethod("Disable", null); }
                        else
                        { adapter.InvokeMethod("Enable", null); }
                    }
                }
            }
            Enabled = !Enabled;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ColorMatrix cm = new ColorMatrix();
            cm.Matrix33 = 0.55f;
            ImageAttributes ia = new ImageAttributes();
            ia.SetColorMatrix(cm);
            var x = this.Width - Properties.Resources.NetworkAssasin.Width;
            e.Graphics.DrawImage(Properties.Resources.NetworkAssasin, new Rectangle(x, 0, Properties.Resources.NetworkAssasin.Width, Properties.Resources.NetworkAssasin.Height), 0, 0, Properties.Resources.NetworkAssasin.Width, Properties.Resources.NetworkAssasin.Height, GraphicsUnit.Pixel, ia);

           
        }
        private void Form1_Load(object sender, EventArgs e)
        {
 
            // Get Network States
            NetworkStates.Clear();
            NetworkStates = GetDeviceSettings();


        }

        private Dictionary<string, int> GetDeviceSettings()
        {
            Dictionary<string, int> NetworkStates = new Dictionary<string, int>();
            SelectQuery AllNetworkAdapterQuery = new SelectQuery("Win32_NetworkAdapter");
            ManagementObjectSearcher search = new ManagementObjectSearcher(AllNetworkAdapterQuery);
            var res = search.Get();

            foreach (ManagementObject result in search.Get())
            {
                NetworkAdapter adapter = new NetworkAdapter(result);

                NetworkStates.Add(adapter.DeviceID, adapter.NetConnectionStatus);
            }
            return NetworkStates;

        }
    }
}
