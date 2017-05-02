using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace SharpController
{
    [Activity(Label = "SharpController", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private TcpClient tcpclnt;
        private Stream stm;
        private int i = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.buttonConnect);

            button.Click += delegate
            {
                this.buttonOneOnClick();
            };

            tcpclnt = new TcpClient();
            tcpclnt.Connect("192.168.1.67", 8686);

            stm = tcpclnt.GetStream();
        }

        public void buttonOneOnClick()
        {
            i = (i + 1) % 4;
            byte[] ba = BitConverter.GetBytes(i);

            stm.Write(ba, 0, ba.Length);
        }
    }
}