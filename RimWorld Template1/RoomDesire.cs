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

        RoomDesireWorker worker;

        HashSet<ThingDef> satisfyingThingsExpanded;

        HashSet<TerrainDef> satisfyingTerrainsExpanded;

        RoomDesire(RoomDesireDef def)
        {
            this.def = def;

            if (def.workerClass.IsSubclassOf(typeof(RoomDesireWorker)))
            {
                worker = (RoomDesireWorker)Activator.CreateInstance(def.workerClass);
            }
            else
            {
                //TODO throw new Exception("Invalid worker type")
            }
        }

        public bool IsMet(Pawn pawn)
        {
            return worker.IsMet(pawn);
        }
    }
}
