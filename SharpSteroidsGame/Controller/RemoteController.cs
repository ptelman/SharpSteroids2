using SharpSteroids.Model.Enum;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SharpSteroids.Controller
{
    public class RemoteController : IDisposable
    {
        public Directions lastDirection { get; private set; }
        private Socket socket;
        private TcpListener myList;
        private Task gettingDirectionsTask;

        public void Initialize()
        {
            IPAddress ipAd = IPAddress.Parse("192.168.1.67");
            myList = new TcpListener(ipAd, 8686);

            myList.Start();
            socket = myList.AcceptSocket();

            gettingDirectionsTask = new Task(() =>
            {
                this.GetLastDirectionsWhileTrue();
            });
            gettingDirectionsTask.Start();
        }

        private void GetLastDirectionsWhileTrue()
        {
            lastDirection = Directions.None;
            while (true)
            {
                byte[] b = new byte[4];
                int k = socket.Receive(b);

                this.lastDirection = (Directions)b[0];
            }
        }

        public void Dispose()
        {
            gettingDirectionsTask.Dispose();
            socket.Close();
            myList.Stop();
        }
    }
}