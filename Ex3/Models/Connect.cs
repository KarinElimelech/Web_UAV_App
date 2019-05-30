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
        private TcpClient client;
        private static Connect self = null;
        private BinaryWriter binaryWriter;
        private BinaryReader binaryReader;
        private bool connected = false;

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

        

        /**
         * Open a new Tcp Client connection to the server.
         * */
        public void Open(string ip, int port)
        {
            if (connected) return;
            this.client = new TcpClient(ip, port);
            stream = client.GetStream();
            stream.Flush();
            Console.WriteLine("Conncted");
            binaryReader = new BinaryReader(this.stream);
            binaryWriter = new BinaryWriter(this.stream);

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
        private double GetInfo(string toSend)
        {
            // convert the command string to an array of bytes.
            binaryWriter.Write(Encoding.ASCII.GetBytes(toSend));
            char c;
            string input = "";
            while ((c = binaryReader.ReadChar()) != '\n')
            {
                input += c;
            }
            stream.Flush();
            return Parser(input);
        }

        // TODO - check if there is problem here when we read 4hz
        private double Parser(string toParse)
        {
            string[] words = toParse.Split('\'');
            return Convert.ToDouble(words[1]);
        }

        public double Lon
        {
            get
            {
                return GetInfo("get /position/longitude-deg\r\n");
            }
        }

        public double Lat
        {
            get
            {
                return GetInfo("get /position/latitude-deg\r\n");
            }
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