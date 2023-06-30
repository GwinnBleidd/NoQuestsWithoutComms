using Verse;
using UnityEngine;

namespace NoQuestsWithoutComms
{
    public class NQWCSettings : ModSettings
    {
        public bool allowLocalIncidents;
        public bool allowDebugOutput;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref allowLocalIncidents, "allowLocalIncidents", true);
            Scribe_Values.Look(ref allowDebugOutput, "allowDebugOutput", false);
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
            listingStandard.CheckboxLabeled("Allow Local Incidents", ref settings.allowLocalIncidents, "When on, local incidents ");
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