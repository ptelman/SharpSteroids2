using SharpSteroids.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSteroids.Helpers
{
    public static class TrigonometryHelper
    {
        public static Coordinates MoveByIntoDirection(float howFar, float angle)
        {
            float y = -(float)(Math.Cos(angle) * howFar);
            float x = (float)(Math.Sin(angle) * howFar);
            return new Coordinates(x, y);
        }
    }
}
