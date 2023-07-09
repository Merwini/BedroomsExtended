using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nuff.PersonalizedBedrooms
{
    public class RoomDesireWorker_Floor : RoomDesireWorker
    {
         public RoomDesireWorker_Floor(RoomDesire parent) : base(parent)
        {
        }

        public override bool IsMet()
        {
            //TODO
            bool isMet = false;

            //TODO logic to check floors

            return isMet;
        }
    }
}
