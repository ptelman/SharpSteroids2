using SharpSteroids.Model;
using SharpSteroids.Model.Interfaces;
using System;

namespace SharpSteroids.Base.Model.Objects
{
    public class Asteroid : IGameObject
    {
        public float _angle = 0.0f;

        public float Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
                if (_angle > 2 * Math.PI)
                {
                    _angle -= 2 * (float)Math.PI;
                }
            }
        }

        private float speedX = 0f;
        private float speedY = 0f;

        private Coordinates _coordinates;

        public Coordinates Coordinates
        {
            get
            {
                return _coordinates;
            }
        }

        public Asteroid()
        {
            this._coordinates = new Coordinates(GameSharedItems.random.Next(0, GameSharedItems.windowWidth), -20);
            this.speedX = GameSharedItems.random.Next(2, 10) / 10;
            this.speedY = GameSharedItems.random.Next(3, 15) / 10;
        }

        public void Move()
        {
            this._coordinates.x += speedX;
            this._coordinates.y += speedY;
            this.Angle += 0.1f;
        }
    }
}