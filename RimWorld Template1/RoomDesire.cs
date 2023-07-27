﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace nuff.PersonalizedBedrooms
{
    public class RoomDesire : IExposable, ILoadReferenceable
    {
        //todo

        internal RoomDesireDef def;

        internal string label;

        //public RoomDesireDef DesDef => def as RoomDesireDef;

        RoomDesireWorker worker;

        //will be filled by RoomDesireMain
        internal HashSet<RoomDesire> upgradesFrom = new HashSet<RoomDesire>();

        //will be filled by RoomDesireMain
        internal HashSet<RoomDesire> incompatibleWith = new HashSet<RoomDesire>();

        internal List<ThingRequirement> requiredThings = new List<ThingRequirement>();

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
                worker = (RoomDesireWorker)Activator.CreateInstance(def.workerClass, this);
            }
            else
            {
                Log.Error("Error: RoomDesire \"" + label + "\" has an invalid RoomDesireWorker. Using default worker.");
                worker = (RoomDesireWorker)Activator.CreateInstance(typeof(RoomDesireWorker_Default), this);
            }
            //obsolete: now uses RequiredThings
            /*
            List<ThingDef> sat1 = def.satisfyingThings ?? new List<ThingDef>();
            for (int i = 0; i < sat1.Count; i++)
            {
                satisfyingThingsExpanded.Add(sat1[i]);
            }
            */

            List<ThingRequirement> requiredThings = def.requiredThings;
            if (!requiredThings.NullOrEmpty<ThingRequirement>())
            {
                for (int i = 0; i < requiredThings.Count; i++)
                {
                    List<ThingDef> satisfyingThings = requiredThings[i].satisfyingThings;
                    List<string> satisfyingTags = requiredThings[i].satisfyingTags;
                    if (!satisfyingThings.NullOrEmpty<ThingDef>())
                    {
                        for (int j = 0; j < satisfyingThings.Count; j++)
                        {
                            requiredThings[i].satisfyingThingsExpanded.Add(satisfyingThings[j]);
                        }
                    }
                    else if (!satisfyingTags.NullOrEmpty<string>())
                    {
                        //TODO, do I really need a tags expanded?
                    }
                    else
                    {
                        Log.Error("Error: Desire \"" + label + "\" has no satisfying things or tags.");
                    }
                }
            }

            List<string> sat2 = def.satisfyingTerrains ?? new List<string>();
            for (int i = 0; i < sat2.Count; i++)
            {
                foreach (TerrainDef td in RoomDesireMain.terrainHashSet)
                {
                    //Implied Defs for carpet start with the
                    if (td.defName.StartsWith(sat2[i]))
                    {
                        satisfyingTerrainsExpanded.Add(td);
                    }
                }
            }
            def.registeredDesire = this;
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

        public void ExposeData()
        {
            Scribe_Defs.Look(ref def, "def");
            Scribe_Values.Look(ref label, "label");
            Scribe_Collections.Look(ref upgradesFrom, "upgradesFrom", LookMode.Reference);
            Scribe_Collections.Look(ref incompatibleWith, "incompatibleWith", LookMode.Reference);
            Scribe_Collections.Look(ref satisfyingThingsExpanded, "satisfyingThingsExpanded", LookMode.Def);
            Scribe_Collections.Look(ref satisfyingTerrainsExpanded, "satisfyingTerrainsExpanded", LookMode.Def);
            Scribe_Values.Look(ref desireTier, "desireTier");

            if (Scribe.mode == LoadSaveMode.Saving)
            {
                string workerClassName = worker?.GetType()?.FullName;
                Scribe_Values.Look(ref workerClassName, "workerClass");
            }
            else if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                string workerClassName = null;
                Scribe_Values.Look(ref workerClassName, "workerClass");
                if (!string.IsNullOrEmpty(workerClassName))
                {
                    Type workerClass = GenTypes.GetTypeInAnyAssembly(workerClassName);
                    worker = (RoomDesireWorker)Activator.CreateInstance(workerClass, this);
                }
            }
        }

        public string GetUniqueLoadID()
        {
            return "Desire_" + def.defName;
        }
    }
}
