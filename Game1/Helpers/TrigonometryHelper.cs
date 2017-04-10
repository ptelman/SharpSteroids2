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
        public static Coordinates MoveByIntoDirection(int howFar, float angle)
        {
            int y = -(int)(Math.Cos(angle) * howFar);
            int x = (int)(Math.Sin(angle) * howFar);
            return new Coordinates(x, y);
        }
    }
}
