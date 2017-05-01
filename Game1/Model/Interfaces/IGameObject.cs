using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSteroids.Model.Interfaces
{
    public interface IGameObject
    {
        Coordinates Coordinates { get;}
        void Move();
    }
}
