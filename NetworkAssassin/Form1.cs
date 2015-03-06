using NetworkAssassin.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
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
        private bool _enabled = true;

        /// <summary>
        /// Collection of "assassin targets" (network adapters).
        /// </summary>
        private List<NetworkAdapter> _networkAdapters = new List<NetworkAdapter>();

        /// <summary>
        /// On click issues an assasinate (or revival) attempt for identified adapters.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            var connected = _networkAdapters.Where(t => t.Status == NetworkState.Connected).ToList();

            if (!connected.Any())
            {
                toolStripLabel1.Text = "Unable to find any network adapters which are currently connected to a network.";
                return;
            }

            foreach (var item in connected) // Kill them with FIRE
            {
                foreach (var adapter in NetworkRepository.GetNetworkAdapters(item.Id))
                {
                    toolStripLabel1.Text = String.Format(_enabled ? "Disabling \"{0}\"" : "Enabling \"{0}\"", adapter.Name);
                    adapter.Toggle();
                    Thread.Sleep(500); // Sleep to give user a bit of time to read messages.
                }
            }

            toolStripLabel1.Text = _enabled ? "Networks assassinated." : "Networks revived.";
            _enabled = !_enabled;
        }

        /// <summary>
        /// Overrides paint event to add a background image of awesomeness...
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ColorMatrix cm = new ColorMatrix { Matrix33 = 0.55f };
            ImageAttributes ia = new ImageAttributes();
            ia.SetColorMatrix(cm);
            var x = Width - Resources.NetworkAssasin.Width;
            e.Graphics.DrawImage(Resources.NetworkAssasin,
                new Rectangle(x, 0, Resources.NetworkAssasin.Width, Resources.NetworkAssasin.Height),
                0, 0, Resources.NetworkAssasin.Width, Resources.NetworkAssasin.Height, GraphicsUnit.Pixel, ia);
        }

        /// <summary>
        /// When the form loads, issues a scan for adapters.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            _networkAdapters.Clear();
            _networkAdapters = NetworkRepository.GetNetworkAdapters().ToList();
            toolStripLabel1.Text = "Targets loaded...";
        }
    }
}
