using Verse;
using UnityEngine;

namespace NoQuestsWithoutComms
{
    public class NQWCSettings : ModSettings
    {
        public bool allowVFEMTournaments;
        public bool allowDebugOutput;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref allowVFEMTournaments, "allowVFEMTournaments");
            Scribe_Values.Look(ref allowDebugOutput, "allowDebugOutput");
            base.ExposeData();
        }
    }

    public class NQWCMod : Mod
    {
        static public NQWCSettings settings;

        public NQWCMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<NQWCSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.CheckboxLabeled("allowVFEMTournamentsExplanation".Translate(), ref settings.allowVFEMTournaments);
            listingStandard.CheckboxLabeled("Allow Debug Output", ref settings.allowDebugOutput);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "No Quests Without Comms";
        }
    }
}