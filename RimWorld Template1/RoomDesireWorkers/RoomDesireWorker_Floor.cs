using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    public class RoomDesireWorker_Floor : RoomDesireWorker
    {
         public RoomDesireWorker_Floor(RoomDesire parent) : base(parent)
        {
        }

        public override bool IsMet(Pawn pawn)
        {
            //TODO
            bool isMet = true;

            Room room = pawn.ownership.OwnedBed.GetRoom();
            HashSet<TerrainDef> terrainSet = new HashSet<TerrainDef>();
            foreach (IntVec3 cell in room.Cells)
            {
                terrainSet.Add(cell.GetTerrain(room.Map));
            }
            List<TerrainDef> satisfyingTerrains = parent.def.satisfyingTerrains;
            foreach (TerrainDef td in terrainSet)
            {
                if (!satisfyingTerrains.Contains(td))
                {
                    isMet = false;
                    break;
                }
            }

            return isMet;
        }
    }
}
