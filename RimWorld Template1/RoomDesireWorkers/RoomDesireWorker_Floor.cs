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

        public override bool IsMet(Pawn pawn, Room room)
        {
            HashSet<TerrainDef> satisfyingTerrains = parent.satisfyingTerrainsExpanded;
            int cellsPercentageNeeded = parent.def.minimumQuantity;
            int cellsSatisfying = 0;
            int cellsTotal = 0;
            foreach (IntVec3 cell in room.Cells)
            {
                cellsTotal++;
                if (satisfyingTerrains.Contains(cell.GetTerrain(room.Map)))
                {
                    cellsSatisfying++;
                }
            }
            Log.Warning(parent.def.defName);
            Log.Warning(cellsPercentageNeeded.ToString());
            Log.Warning(((float)cellsPercentageNeeded /100).ToString());
            Log.Warning(cellsTotal.ToString());
            Log.Warning(cellsSatisfying.ToString());
            return ((float)cellsSatisfying / (float)cellsTotal) >= ((float)cellsPercentageNeeded / 100);
        }
    }
}
