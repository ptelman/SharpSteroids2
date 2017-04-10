using SharpSteroids.Helpers;
using SharpSteroids.Model;
using SharpSteroids.Model.Interfaces;
using System;

namespace SharpSteroids.Base.Model.Objects
{
    public class Ship : IGameObject
    {
        public float Angle = 0.0f;

        private float speedX = 0f;
        private float speedY = 0f;
        private float speedAngle = 0f;

        private Coordinates _coordinates;
        public Coordinates Coordinates
        {
            get
            {
                return _coordinates;
            }
        }

        public Ship(Coordinates cords)
        {
            this._coordinates = new Coordinates(cords.x, cords.y);
        }

        public void Move(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
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