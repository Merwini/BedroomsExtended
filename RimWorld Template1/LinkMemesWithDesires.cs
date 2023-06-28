using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    [StaticConstructorOnStartup]
    class LinkMemesWithDesires
    {
        static LinkMemesWithDesires()
        {
            //on startup, check if Ideology is active
            //if it is, check each RoomDesireDef for associated memes
            //if it has an associated meme, add the meme to the disctionary as a key, add the desire to that key's list value

            //at some point, maybe a gameComponent, check ideoligions for what memes they have active
        }

        internal static Dictionary<MemeDef, List<RoomDesireDef>> memeToDesireDictionary;
    }
}
