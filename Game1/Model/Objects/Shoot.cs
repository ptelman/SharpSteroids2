using SharpSteroids.Helpers;
using SharpSteroids.Model;
using SharpSteroids.Model.Interfaces;
using System;

namespace SharpSteroids.Base.Model.Objects
{
    public class Shoot : IGameObject
    {
        public float Angle;

        public Shoot(Coordinates cords)
        {
            this._coordinates = cords;
        }

        private Coordinates _coordinates;

        public Coordinates Coordinates
        {
            get
            {
                return _coordinates;
            }
        }

        public void Move(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void MoveInDirectionByOne()
        {
            var cordsToMove = TrigonometryHelper.MoveByIntoDirection(8, Angle);

            this._coordinates.x += cordsToMove.x;
            this._coordinates.y += cordsToMove.y;
        }
    }
}