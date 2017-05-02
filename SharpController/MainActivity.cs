using Android.App;
using Android.Hardware;
using Android.OS;
using Android.Widget;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Android.Runtime;
using Android.Content;

namespace SharpController
{
    [Activity(Label = "SharpController", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ISensorEventListener
    {
        private ConnectionManager manager;
        private DateTime lastMessageSend = DateTime.Now;
        private SensorManager sensorManager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.buttonConnect);
            sensorManager = (SensorManager)GetSystemService(Context.SensorService);

            button.Click += delegate
            {
                this.buttonOneOnClick();
            };
        }

        public void buttonOneOnClick()
        {
            manager = new ConnectionManager();
            manager.Initialize();
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            //nothing
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (DateTime.Now > lastMessageSend.AddMilliseconds(50) && manager != null && manager.Initialized)
            {
                lastMessageSend = DateTime.Now;
                float x = e.Values[0], y = e.Values[1], z = e.Values[2];

                if (x > 4)
                    manager.Send(Model.Directions.Left);
                else if (x < -4)
                    manager.Send(Model.Directions.Right);
                else if (y < 4)
                    manager.Send(Model.Directions.Up);
            }

        }

        protected override void OnResume()
        {
            base.OnResume();
            sensorManager.RegisterListener(this, sensorManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Ui);
        }

        protected override void OnPause()
        {
            base.OnPause();
            sensorManager.UnregisterListener(this);
        }
    }
}