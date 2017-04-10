using SharpSteroids.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSteroids.Model;

namespace SharpSteroids.Base.Model.Objects
{
    public class Asteroid : IGameObject
    {
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
    }
}
