using Microsoft.Xna.Framework;
using SharpSteroids.Helpers;
using SharpSteroids.Model;
using SharpSteroids.Model.Interfaces;
using System;

namespace SharpSteroids.Base.Model.Objects
{
    public class Ship : IGameObject
    {
        private float speedX = 0f;
        private float speedY = 0f;

        private Coordinates _coordinates;
        private int textureWidth;
        private int textureHeight;
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

        public Coordinates Coordinates
        {
            get
            {
                return _coordinates;
            }
        }

        public Ship(Coordinates cords, int textureWidth, int textureHeight)
        {
            this._coordinates = new Coordinates(cords.x, cords.y);
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
        }

        public void Move()
        {
            if (!(this.Coordinates.x < 0 && this.speedX < 0) && !(this.Coordinates.x > GameSharedItems.windowWidth && this.speedX > 0))
                this._coordinates.x += speedX;

            if (!(this.Coordinates.y > GameSharedItems.windowHeight && this.speedY > 0) && !(this.Coordinates.y < 0 && this.speedY < 0))
                this._coordinates.y += speedY;
        }

        public void MoveForwards()
        {
            var speedToAdd = TrigonometryHelper.MoveByIntoDirection(0.2f, Angle);

            this.speedX += speedToAdd.x;
            this.speedY += speedToAdd.y;
        }
    }
}