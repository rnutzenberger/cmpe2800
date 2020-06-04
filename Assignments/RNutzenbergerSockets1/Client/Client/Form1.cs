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
using NumGuessTransport;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace Client
{
    public partial class Form1 : Form
    {
        
        Socket commSocket = null;       //communicating socket
        Thread th;                      //thread to process info
        int Guess;                      //Guess being saved
        NumGuessCtoS ngCtoS;            //transporter
        public Form1()  
        {
            InitializeComponent();
            _btnConnect.Click += _btnConnect_Click;
            trackBar1.ValueChanged += TrackBar1_ValueChanged;
            Shown += Form1_Shown;
            _btnGuess.Click += _btnGuess_Click;
        }

        private void _btnGuess_Click(object sender, EventArgs e)
        {
            //as long as the communitcation socket is not null, send the data to the server
            if(commSocket != null)
            {
                ngCtoS = new NumGuessCtoS(Guess);
                SendData(ngCtoS);
            }
        }
        /// <summary>
        /// Sends data to the Server to unpack
        /// </summary>
        /// <param name="CtoS"></param>
        private void SendData(NumGuessCtoS CtoS)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, CtoS);
            commSocket.Send(ms.GetBuffer(), (int)ms.Length, SocketFlags.None);
        }

        /// <summary>
        /// Opens the Connection dialog and starts the thread
        /// </summary>
        private void StartConnect()
        {

            ConnectDialog connectDialog = new ConnectDialog();
            if (connectDialog.ShowDialog() == DialogResult.OK)
            {
                //enable UI controls and start the thread
                trackBar1.Enabled = true;
                _btnGuess.Enabled = true;
                commSocket = connectDialog.ConnectingSocket;
                _lblStatus.Text = "Connected! Start guessing.";
                th = new Thread(new ThreadStart(ProcessingThread));
                th.IsBackground = true;
                th.Start();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
        }

        //change guess value and label on value changed
        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            _lblGuess.Text = trackBar1.Value.ToString();
            Guess = trackBar1.Value;
        }

        //starts the connection
        private void _btnConnect_Click(object sender, EventArgs e)
        {
            StartConnect();
        }

        /// <summary>
        /// Thread used to process data comming in from the Server
        /// </summary>
        private void ProcessingThread()
        {
            //need to hold the data somehow
            byte[] buff = new byte[2000];
            commSocket.ReceiveTimeout = 0;

            while (true)
            {
                //receive the bytes
                int bytes = 0;
                try
                {
                    bytes = commSocket.Receive(buff);
                }
                catch (Exception err)
                {
                    //if theres an erro, handle it by resetting the UI 
                    Console.WriteLine("ProcessingThread : " + err.Message);
                    commSocket = null;
                    Invoke((Action)(() => _lblStatus.Text = "Lost connection to server"));
                    Invoke((Action)(() => ResetUI()));
                    Invoke((Action)(() => _btnGuess.Enabled = false));
                    Invoke((Action)(() => trackBar1.Enabled = false));

                }

                //bytes is 0, then connection is done
                if (bytes == 0)
                {
                    try
                    {
                        //shutdown the socket
                        commSocket.Shutdown(SocketShutdown.Both);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("ProcessingThread : " + err.Message);
                    }
                    try
                    {
                        //close it and set it to null
                        commSocket.Close();
                        commSocket = null;
                    }
                    catch(Exception err)
                    {
                        Console.WriteLine(err.Message);
                    }
                    
                    //if an invoke is needed then reset the UI
                    if (InvokeRequired)
                    {
                        try
                        {
                            Invoke((Action)(() => ResetUI()));
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        
                    }
                    
                }

               
                try
                {
                    //get the info from the server
                    BinaryFormatter bf = new BinaryFormatter();
                    object frame = bf.Deserialize(new MemoryStream(buff));
                    if (frame is NumGuessStoC)
                    {
                        //if the response is too high, too low, then set label and reduce trackbar min/max
                        var f = frame as NumGuessStoC;
                        if (f.GuessResponse == NumGuessStoC.GuessResponses.TooHigh)
                        {
                            Invoke((Action)(() => _lblStatus.Text = "Guess was too high"));
                            Invoke((Action)(() => trackBar1.Maximum = Guess - 1));

                        }
                        else if (f.GuessResponse == NumGuessStoC.GuessResponses.TooLow)
                        {
                            Invoke((Action)(() => _lblStatus.Text = "Guess was too low"));
                            Invoke((Action)(() => trackBar1.Minimum = Guess + 1));
                        }
                        else
                        {
                            Invoke((Action)(() => _lblStatus.Text = "Correct! New number generated"));
                            Invoke((Action)(() => ResetUI()));
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        /// <summary>
        /// Resets the UI
        /// </summary>
        private void ResetUI()
        {
            trackBar1.Maximum = 1000;
            trackBar1.Minimum = 1;
            trackBar1.Value = 1;
        }
    }
}
