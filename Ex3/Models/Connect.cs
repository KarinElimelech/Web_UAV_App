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
        private TcpClient client = null;
        private static Connect self = null;
        private BinaryWriter binaryWriter;
        private BinaryReader binaryReader;

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

        
        /**
         * Open a new Tcp Client connection to the server.
         * */
        public void Open(string ip, int port)
        {
            if (client != null) return;
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
        public string this[string toSend]
        {
            get
            {
                lock(locker) {
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
            }
        }


        /**
         * the function gets string and parse it.
         */
        private string Parser(string toParse)
        {
            string[] words = toParse.Split('\'');
            return words[1];
        }

        /**
         * Is the connection open
         * */
        public bool IsConnected()
        {
            return client != null;
        }

    }
}