using SharpSteroids.Helpers;
using SharpSteroids.Model;
using SharpSteroids.Model.Interfaces;
using System;

namespace SharpSteroids.Base.Model.Objects
{
    public class Ship : IGameObject
    {
        public float Angle = 0.0f;

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

        public void MoveForwards()
        {
            var cordsToMove = TrigonometryHelper.MoveByIntoDirection(5, Angle);

            this._coordinates.x += cordsToMove.x;
            this._coordinates.y += cordsToMove.y;
        }
    }
}