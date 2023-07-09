using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace nuff.PersonalizedBedrooms
{
    public class RoomDesire
    {
        //todo

        RoomDesireDef def;

        RoomDesireWorker worker;

        Pawn pawn;

        public RoomDesire(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public bool Met
        {
            get
            {
                //TODO
                return true;
            }
        }
    }
}
