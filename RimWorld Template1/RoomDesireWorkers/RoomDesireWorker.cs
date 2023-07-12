using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace nuff.PersonalizedBedrooms
{
    public abstract class RoomDesireWorker
    {
        public RoomDesire parent;

        public RoomDesireWorker(RoomDesire parent)
        {
            this.parent = parent;
        }

        public abstract bool IsMet(Pawn pawn);
    }
}
