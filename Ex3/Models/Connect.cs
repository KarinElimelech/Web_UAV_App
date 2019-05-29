using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ex3.Models
{
    public class Connect
    {
        private readonly static object locker = new object();
        private NetworkStream stream = null;
        private static Connect self = null;
        private bool connected;
        private const string lon = "get /position/longitude-deg\r\n";
        private const string lat = "get /position/latitude-deg\r\n";
        private TcpListener listen;

        /**
         * The Instance static property for the Singleton getter.
         * */
        public static Connect Instance
        {
            get
            {
                lock (locker)
                {
                    if (null == self)
                    {
                        self = new Connect();
                    }
                    return self;
                }
            }
        }

        

        bool isConnected()
        {
            return this.connected;
        }

        TcpClient client = new TcpClient();

        /**
         * Open a new Tcp Client connection to the server.
         * */
        public void Open(string ip, int port)
        {
            IPEndPoint ep1 = new IPEndPoint(IPAddress.Parse(ip), port);
            this.listen = new TcpListener(IPAddress.Parse(ip), port);
            listen.Start();
            while (!client.Connected)
            {
                client.Connect(ep1);
                Debug.WriteLine("Command channel :You are connected");
                this.connected = true;
            }
            this.stream = client.GetStream();
        }

        /**
         * closes the client and the network stream.
         * */
        public void Close()
        {
            stream.Close();
            client.Close();
        }

        /**
         * Sends the string to the server.
         * */
        private void Sender(string toSend)
        {
           // convert the command string to an array of bytes.
           byte[] buffer = System.Text.Encoding.ASCII.GetBytes(toSend.ToString());
           stream.Write(buffer, 0, buffer.Length);
           Console.WriteLine("command: " + toSend);
           stream.Flush();     
        }

        private string Reader()
        {
            string msg="";
            Byte[] bytes;
            if (client.ReceiveBufferSize > 0)
            {
                bytes = new byte[client.ReceiveBufferSize];
                stream.Read(bytes, 0, client.ReceiveBufferSize);
                msg = Encoding.ASCII.GetString(bytes);
            }
            return msg;
        }

        /**
         * Sends all commands to the server, waiting two seconds between commands.
         * */
        public void Send(List<string> cmds)
        {
            if (null == client) { return; }
            Sender(lon);
            
            Sender(lat);
        }

        /**
         * Is the connection open
         * */
        public bool IsConnected()
        {
            return this.client != null;
        }


    }
}