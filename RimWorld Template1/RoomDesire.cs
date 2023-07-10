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

        internal RoomDesireDef def;

        //public RoomDesireDef DesDef => def as RoomDesireDef;

        internal RoomDesireWorker worker;

        internal Pawn pawn;

        public RoomDesire(RoomDesireDef def, Pawn pawn)
        {
            this.def = def;
            this.pawn = pawn;
            if (def.workerClass.IsSubclassOf(typeof(RoomDesireWorker)))
            {
                worker = (RoomDesireWorker)Activator.CreateInstance(def.workerClass);
            }
            else
            {
                //TODO throw new Exception("Invalid worker type")
            }
        }

        public bool Met
        {
            get
            {
                return worker.IsMet();
            }
        }
    }
}
