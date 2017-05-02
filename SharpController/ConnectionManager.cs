using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Sockets;
using System.IO;
using SharpController.Model;

namespace SharpController
{
    public class ConnectionManager
    {
        public bool Initialized = false;
        private TcpClient tcpClient;
        private Stream stream;

        public void Initialize()
        {
            tcpClient = new TcpClient();
            tcpClient.Connect("192.168.1.67", 8686);

            stream = tcpClient.GetStream();
            Initialized = true;
        }

        public void Send(Directions direction)
        {
            byte[] byteArray = BitConverter.GetBytes((int)direction);

            stream.Write(byteArray, 0, byteArray.Length);
        }
    }
}