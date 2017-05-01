using Microsoft.Xna.Framework;
using SharpSteroids.Helpers;
using SharpSteroids.Model;
using SharpSteroids.Model.Interfaces;
using System;

namespace SharpSteroids.Base.Model.Objects
{
    public class Shoot : IGameObject
    {
        public float Angle;

        

        private Coordinates _coordinates;
        private int textureWidth;
        private int textureHeight;

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)_coordinates.x,
                    (int)_coordinates.y,
                    (int)(textureWidth * GameSharedItems.shootScale),
                    (int)(textureHeight * GameSharedItems.shootScale));
            }
        }

        public Coordinates Coordinates
        {
            get
            {
                return _coordinates;
            }
        }

        public Shoot(Coordinates cords, int textureWidth, int textureHeight)
        {
            this._coordinates = cords;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
        }

        public void Move()
        {
            var cordsToMove = TrigonometryHelper.MoveByIntoDirection(8, Angle);

            this._coordinates.x += cordsToMove.x;
            this._coordinates.y += cordsToMove.y;
        }
    }
}