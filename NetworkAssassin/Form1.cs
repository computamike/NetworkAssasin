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
using System.Threading;
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

        /// <summary>
        /// Tracks current state of network, is it dead or alive?
        /// </summary>
        private bool Enabled = true;

        /// <summary>
        /// Collection of "assassin targets" (network adapters).
        /// </summary>
        private List<AssassinTarget> AssassinTargets = new List<AssassinTarget>();

        /// <summary>
        /// On click issues an assasinate (or revival) attempt for identified adapters.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var item in AssassinTargets)
            {
                if (item.Status == 2)
                {// Kill it with FIRE
                    SelectQuery query = new SelectQuery("Win32_NetworkAdapter", "DeviceID=" + item.Id);
                    ManagementObjectSearcher search = new ManagementObjectSearcher(query);
                 
                    foreach (var adapt in search.Get())
                    {
                        
                        ManagementObject adapter =  (ManagementObject)adapt;
                        if (Enabled)
                        {
                            toolStripLabel1.Text = string.Format("Disabling \"{0}\"...", item.Name);
                            adapter.InvokeMethod("Disable", null);
                        }
                        else
                        {
                            toolStripLabel1.Text = string.Format("Enabling \"{0}\"...", item.Name);
                            adapter.InvokeMethod("Enable", null);
                        }
                        // Sleep to give user a bit of time to read messages.
                        Thread.Sleep(500); 
                    }
                }
            }

            toolStripLabel1.Text = Enabled ? "Networks assassinated." : "Networks revived.";
            Enabled = !Enabled;
        }

        /// <summary>
        /// Overrides paint event to add a background image of awesomeness...
        /// </summary>
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

        /// <summary>
        /// When the form loads, issues a scan for adapters.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
 
            // Get Network States
            AssassinTargets.Clear();
            AssassinTargets = GetDeviceSettings();

            toolStripLabel1.Text = "Targets loaded...";

        }

        /// <summary>
        /// Scans for adapters, and exposes them as a list.
        /// </summary>
        private List<AssassinTarget> GetDeviceSettings()
        {
            List<AssassinTarget> states = new List<AssassinTarget>();
            SelectQuery AllNetworkAdapterQuery = new SelectQuery("Win32_NetworkAdapter");
            ManagementObjectSearcher search = new ManagementObjectSearcher(AllNetworkAdapterQuery);
            var res = search.Get();

            foreach (ManagementObject result in search.Get())
            {
                states.Add(new AssassinTarget(new NetworkAdapter(result)));
            }

            return states;
        }
    }
}
