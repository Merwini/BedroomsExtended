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

        internal string label;

        //public RoomDesireDef DesDef => def as RoomDesireDef;

        RoomDesireWorker worker;

        //will be filled by RoomDesireMain
        internal HashSet<RoomDesire> upgradesFrom;

        //will be filled by RoomDesireMain
        internal HashSet<RoomDesire> incompatibleWith;

        internal HashSet<ThingDef> satisfyingThingsExpanded = new HashSet<ThingDef>();

        internal HashSet<TerrainDef> satisfyingTerrainsExpanded = new HashSet<TerrainDef>();

        internal int desireTier;

        internal RoomDesire(RoomDesireDef def)
        {
            this.def = def;
            this.label = def.label;
            this.desireTier = def.desireTier;
            //TODO finish filling fields
            if (def.workerClass.IsSubclassOf(typeof(RoomDesireWorker)))
            {
                worker = (RoomDesireWorker)Activator.CreateInstance(def.workerClass);
            }
            else
            {
                //TODO throw new Exception("Invalid worker type")
            }
            List<ThingDef> sat1 = def.satisfyingThings;
            for (int i = 0; i < sat1.Count; i++)
            {
                satisfyingThingsExpanded.Add(sat1[i]);
            }
            List<string> sat2 = def.satisfyingTerrains;
            for (int i = 0; i < sat2.Count; i++)
            {
                foreach (TerrainDef td in RoomDesireMain.terrainHashSet)
                {
                    if (td.defName.StartsWith(sat2[i]))
                    {
                        satisfyingTerrainsExpanded.Add(td);
                    }
                }
            }
        }

        public bool IsMet(Pawn pawn, Room room)
        {
            return worker.IsMet(pawn, room);
        }

        public HashSet<ThingDef> fillSatisfyingThings()
        {
            HashSet<ThingDef> satS = new HashSet<ThingDef>();
            List<ThingDef> satL = this.def.satisfyingThings;
            for (int i = 0; i < satL.Count; i++)
            {
                satS.Add(satL[i]);
            }
            return satS;
        }
    }
}
