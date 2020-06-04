using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace Client
{
    public partial class ConnectDialog : Form
    {
        public int PORT;                        //Port used(1666)
        public string IP_ADDR;                  //IP Adress entered
        public Socket ConnectingSocket { get; private set; } = null;    //property to get the connected socket


        
        public ConnectDialog()
        {
            InitializeComponent();
            _btnConnect.Click += _btnConnect_Click;
            
        }

        private void _btnConnect_Click(object sender, EventArgs e)
        {
            //create a new connecting socket if its null
            if(ConnectingSocket == null)
            {
                //Validate the IP in the textbox
                ConnectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (ValidateIP(_txbAddr.Text))
                {
                    //if good, then parse it out and get the port too
                    PORT = int.Parse(_txbPort.Text);
                    IP_ADDR = _txbAddr.Text;
                }
                
                try
                {
                    //begin connecting
                    ConnectingSocket.BeginConnect(IP_ADDR, PORT, CB_Connect, ConnectingSocket);
                }
                catch(SocketException se)
                {
                    MessageBox.Show(se.Message,"Connect Dialog Error");
                }
                
            }
        }

        /// <summary>
        /// Checks if the IP is valid or not
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public bool ValidateIP(string IP)
        {
            if (string.IsNullOrWhiteSpace(IP)) return false;
            string[] temp = IP.Split('.');
            if (temp.Length > 4) return false;

            return temp.All(x => byte.TryParse(x, out byte result));

        }

        /// <summary>
        /// call baack if the connection is good
        /// </summary>
        /// <param name="ar"></param>
        public void CB_Connect(IAsyncResult ar)
        {
            Socket cSok = (Socket)ar.AsyncState;
            try
            {
                //end the connection when its connected and close the dialog
                cSok.EndConnect(ar);
                MessageBox.Show("Connected!", "", MessageBoxButtons.OK);
                this.DialogResult = DialogResult.OK;
                this.Close();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
