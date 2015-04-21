using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SV_Client.Classes.ProgramLogic.PlayField
{
    public class EnemyPlayField : AbstractPlayField
    {
        public EnemyPlayField(int sizeX, int sizeY)
            : base(sizeX, sizeY)
        {

        }

        public EnemyPlayField()
        {
            
        }
    }
}
