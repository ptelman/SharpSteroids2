using SharpSteroids.Helpers;
using SharpSteroids.Model;
using SharpSteroids.Model.Interfaces;
using System;

namespace SharpSteroids.Base.Model.Objects
{
    public class Ship : IGameObject
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
        private int windowWidth;
        private int windowHeight;

        private Coordinates _coordinates;
        public Coordinates Coordinates
        {
            get
            {
                return _coordinates;
            }
        }

        public Ship(Coordinates cords, int windowWidth, int windowHeight)
        {
            this._coordinates = new Coordinates(cords.x, cords.y);
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
        }

        public void Move(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            if (this._coordinates.x > this.windowWidth && !((this.Angle > Math.PI / 2 && this.Angle < Math.PI * 1.5 && this._coordinates.y < this.windowHeight/2) || (this.Angle > Math.PI * 1.5 && this.Angle < Math.PI * 2 && this._coordinates.y > this.windowHeight / 2)))
            {
                this.speedX = 0;
            }
            if (this._coordinates.x < 0 && !((this.Angle > Math.PI / 2 && this.Angle < Math.PI && this._coordinates.y < this.windowHeight / 2) || (this.Angle > 0 && this.Angle < Math.PI / 2 && this._coordinates.y > this.windowHeight / 2)))
            {
                this.speedX = 0;
            }
            if (this._coordinates.y > this.windowHeight && !((this.Angle > Math.PI * 1.5 && this.Angle < Math.PI * 2 && this._coordinates.x > this.windowWidth / 2) || (this.Angle > 0 && this.Angle < Math.PI / 2 && this._coordinates.x < this.windowWidth / 2)))
            {
                this.speedY = 0;
            }
            if (this._coordinates.y < 0 && !((this.Angle > Math.PI && this.Angle < Math.PI * 1.5 && this._coordinates.x > this.windowWidth / 2) || (this.Angle > Math.PI * 0.5 && this.Angle < Math.PI && this._coordinates.x < this.windowWidth / 2)))
            {
                this.speedY = 0;
            }
            this._coordinates.x += speedX;
            this._coordinates.y += speedY;
        }

        public void MoveForwards()
        {
            var speedToAdd = TrigonometryHelper.MoveByIntoDirection(0.2f, Angle);

            this.speedX += speedToAdd.x;
            this.speedY += speedToAdd.y;

            this.Move();
        }
    }
}