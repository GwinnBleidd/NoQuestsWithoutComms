using Verse;
using HarmonyLib;
using System.Reflection;
using System.Linq;
using RimWorld;
using System;

namespace NoQuestsWithoutComms
{
    [StaticConstructorOnStartup]
    public class PatchMain
    {
        static public bool hasTribalSignalfire = false;
        static public bool hasNopowerCommsSimplified = false;
        static public DateTime lastCheckComms = DateTime.Now;
        static public bool cachedResult = false;

        static PatchMain() {

            hasTribalSignalfire = ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Tribal Signal Fire (Continued)" || m.PackageId == "Mlie.TribalSignalFire");
            hasNopowerCommsSimplified = ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Nopower Comms Simplified");

			var harmony = new Harmony("eBae.NoQuestsWithoutComms");
			var assembly = Assembly.GetExecutingAssembly();
			harmony.PatchAll(assembly);

//            MethodInfo NQWC_target1 = AccessTools.Method(typeof(IncidentWorker_GiveQuest), "CanFireNowSub");
//            MethodInfo NQWC_target2 = AccessTools.Method(typeof(StorytellerComp_SingleOnceFixed), "MakeIntervalIncidents");
             
//            HarmonyMethod NQWCPrefix = new HarmonyMethod(AccessTools.Method(typeof(NoQuestsWithoutComms_Patch.Patch1), "NQWC_Prefix"));
//            HarmonyMethod NQWCPostfix = new HarmonyMethod(AccessTools.Method(typeof(NoQuestsWithoutComms_Patch.Patch2), "NQWC_Postfix"));

//            instance.Patch(NQWC_target1, NQWCPrefix, null, null, null);
//            instance.Patch(NQWC_target2, null, NQWCPostfix, null, null);

/*
           if (ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Real Ruins"))
            {
               MethodInfo RealRuinsTarget = AccessTools.Method(AccessTools.TypeByName("RealRuins.IncidentWorker_RuinsFound"), "CanFireNowSub");
               HarmonyMethod RealRuinsPrefix = new HarmonyMethod(AccessTools.Method(typeof(NoQuestsWithoutComms_RealRuinsPatch.Patch1), "NQWC_RealRuinsPrefix"));
               instance.Patch(RealRuinsTarget, RealRuinsPrefix, null, null, null);
            }
*/
           /*
            if (ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Vanilla Factions Expanded - Medieval") && !(NQWCSettings.allowVFEMTournaments))
            {
                MethodInfo VFEMTarget = AccessTools.Method(AccessTools.TypeByName("VFEMedieval.IncidentWorker_QuestMedievalTournament"), "CanFireNowSub");
                HarmonyMethod VFEMPrefix = new HarmonyMethod(AccessTools.Method(typeof(NoQuestsWithoutComms_VFEMPatch.Patch1), "NQWC_VFEMPrefix"));
                instance.Patch(VFEMTarget, VFEMPrefix, null, null, null);
            }*/
        }
    }
}

