using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV_Client.Classes
{

    public class Field
    {
        public bool pu_OpenSpace;
        public bool pu_ShipSpace;
        public double pu_LowerXBarrier;
        public double pu_UpperXBarrier;
        public double pu_LowerYBarrier;
        public double pu_UpperYBarrier;

        public Field()
        {

        }

        public Field(double LowerXBarrier, double UpperXBarrier, double LowerYBarrier, double UpperYBarrier)
        {
            this.pu_OpenSpace = true;
            this.pu_ShipSpace = false;
            this.pu_LowerXBarrier = LowerXBarrier;
            this.pu_LowerYBarrier = LowerYBarrier;
            this.pu_UpperXBarrier = UpperXBarrier;
            this.pu_UpperYBarrier = UpperYBarrier;
        }
    }
}
