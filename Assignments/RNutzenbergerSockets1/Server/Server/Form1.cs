using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using NumGuessTransport;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;



namespace Server
{
    public partial class Form1 : Form
    {
        
        Random rnd = new Random();                              //Random number generator
        int secretNum;                                          //secretNumber saved
        Thread th;                                              //Thread for processing
        private delegate void delVoidVoid();                    //used to reset the number
        Socket connSocket;                                      //connection socket
        Socket listeningSocket;                                 //listening socket
        NumGuessStoC ngStoC;                                    //Vehicle to send data to client
        public Form1()
        {
            InitializeComponent();
            Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Listen();
            GenGuess();
        }

        
        /// <summary>
        /// Receives and Process data from the client
        /// </summary>
        private void ProcessThread()
        {
            //receive data in byte array
            byte[] buff = new byte[2000];
            //no timeouts
            connSocket.ReceiveTimeout = 0;
            
            while (true)
            {
                
                int bytes = 0;
                try
                {
                    //block on receive - will throw hard disconnect
                    bytes = connSocket.Receive(buff);
                }
                catch(Exception err)
                {
                    //on hard disconnect, then reset and listen again
                    Console.WriteLine("ProcessThread : " + err.Message);
                    listeningSocket = null;
                    connSocket = null;
                    Listen();
                }
                //communication is complete
                if(bytes == 0)
                {
                    //reset and lsiten again
                    listeningSocket.Close();
                    //connSocket.Close();
                    listeningSocket = null;
                    connSocket = null;
                    Listen();
                }

                //have data to process if we hit here
                BinaryFormatter bf = new BinaryFormatter();
                object frame = bf.Deserialize(new MemoryStream(buff));
                if(frame is NumGuessCtoS)
                {
                    var f = frame as NumGuessCtoS;
                    //if the guessed number is greater than the secret num
                    if(f.GuessNumber > secretNum)
                    {
                        //set the returning answer to too high
                        ngStoC = new NumGuessStoC(NumGuessStoC.GuessResponses.TooHigh);
                    }
                    //too low
                    else if(f.GuessNumber < secretNum)
                    {
                        //set enum to too low
                        ngStoC = new NumGuessStoC(NumGuessStoC.GuessResponses.TooLow);
                    }
                    //else its right
                    else
                    {
                        //set to correct and generate a new number to start the game again
                        ngStoC = new NumGuessStoC(NumGuessStoC.GuessResponses.Correct);
                        try
                        {
                            Invoke(new delVoidVoid(GenGuess)); 
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        
                    }
                    //send the data to the client
                    SendData(ngStoC);

                }
            }

        }

        /// <summary>
        /// Sends data back to the client
        /// </summary>
        /// <param name="StoC">Contains an enum for client to know</param>
        private void SendData(NumGuessStoC StoC)
        {
            //bundle up the data to send away
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, StoC);
            try
            {
                connSocket.Send(ms.GetBuffer(), (int)ms.Length, SocketFlags.None);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        /// <summary>
        /// Waits and listens to incoming connections
        /// </summary>
        private void Listen()
        {
            //create the listening socket
            listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //bind to port 1666
                listeningSocket.Bind(new IPEndPoint(IPAddress.Any, 1666));

            }
            catch (Exception se)
            {
                Console.WriteLine(se.Message);
            }
            //try listen for connection
            try
            {
                listeningSocket.Listen(5);
            }
            catch (Exception se)
            {
                Console.WriteLine(se.Message);
            }

            //connection found, begin to accept the connection
            try
            {
                listeningSocket.BeginAccept(cbAccept, listeningSocket);
            }
            catch(Exception se)
            {
                Console.WriteLine(se.Message);
            }
        }
        /// <summary>
        /// callback to accept the connection
        /// </summary>
        /// <param name="ar"></param>
        private void cbAccept(IAsyncResult ar)
        {
            //use a new socket for connecting
            Socket lSok = (Socket)(ar.AsyncState);
            try
            {
                //end the accept and close the listening socket
                //start the manual processing thread
                connSocket = lSok.EndAccept(ar);
                lSok.Close();
                listeningSocket.Close();
                listeningSocket = null;
                th = new Thread(new ThreadStart(ProcessThread));
                th.IsBackground = true;
                th.Start();

            }
            catch (Exception se)
            {
                Console.WriteLine(se.Message);
            }
        }

        /// <summary>
        /// Generates a new random number from 1-1000
        /// </summary>
        private void GenGuess()
        {
            secretNum = rnd.Next(1, 1001);
            _lblNum.Text = secretNum.ToString();
        }
    }
}
