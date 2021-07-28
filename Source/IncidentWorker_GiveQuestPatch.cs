using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using HarmonyLib;
using RimWorld;
using System.Reflection;

namespace NoQuestsWithoutComms
{
    [HarmonyPatch(typeof(IncidentWorker_GiveQuest))]
    [HarmonyPatch("CanFireNowSub")]
    [HarmonyPatch(new Type[] { typeof(IncidentParms)})]
    class IncidentWorker_GiveQuest_CanFireNowSub {
        public static bool Prefix(ref bool __result, IncidentWorker_GiveQuest __instance) {
//            Log.Message("IncidentWorker_GiveQuest_CanFireNowSub " + __instance.def.defName, true);

            TimeSpan interval = DateTime.Now - PatchMain.lastCheckComms;
            if (interval.TotalSeconds >= 2f) {
//                Log.Message("IncidentWorker_GiveQuest_CanFireNowSub Got in", true);
                bool tmpResult = false;

                List<Map> maps = Find.Maps;
                for (int i = 0; i < maps.Count; i++) {
                    if (CommsConsoleUtility.PlayerHasPoweredCommsConsole(maps[i])) {
                        tmpResult = true;
                        break;
                    }
                }

                if (!tmpResult && PatchMain.hasTribalSignalfire) {
                    for (int i = 0; i < maps.Count && !tmpResult; i++) {
                        foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("SignalFire"))) {
                            if (building.Faction == Faction.OfPlayer) {
                                CompGlower compGlower = building.TryGetComp<CompGlower>();
                                if (compGlower.Glows) {
                                    tmpResult = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!tmpResult && PatchMain.hasIndustrialAge) {
                    for (int i = 0; i < maps.Count && !tmpResult; i++) {
/*
                        foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("Estate_Telegraph"))) {
                            if (building.Faction == Faction.OfPlayer) {
                                CompPowerTrader compPowerTrader = building.TryGetComp<CompPowerTrader>();
                                if (compPowerTrader == null || compPowerTrader.PowerOn) {
//                                    Log.Message("IncidentWorker_GiveQuest_CanFireNowSub Estate_Telegraph", true);
                                    tmpResult = true;
                                    break;
                                }
                            }
                        }
*/
                        if (!tmpResult) {
                            foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("Estate_Radio"))) {
                                if (building.Faction == Faction.OfPlayer) {
                                    CompPowerTrader compPowerTrader = building.TryGetComp<CompPowerTrader>();
                                    if (compPowerTrader == null || compPowerTrader.PowerOn) {
//                                        Log.Message("IncidentWorker_GiveQuest_CanFireNowSub Estate_Radio", true);
                                        tmpResult = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (!tmpResult && PatchMain.hasNopowerCommsSimplified) {
                    for (int i = 0; i < maps.Count && !tmpResult; i++) {
                        foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("BirdPostMessageTable"))) {
                            if(building.Faction == Faction.OfPlayer) {
                                CompPowerTrader compPowerTrader = building.TryGetComp<CompPowerTrader>();
                                if (compPowerTrader == null || compPowerTrader.PowerOn) {
                                    tmpResult = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                PatchMain.lastCheckComms = DateTime.Now;
                if (tmpResult) {
                    __result = tmpResult;
                }
                PatchMain.cachedResult = tmpResult;
            }

            return PatchMain.cachedResult;
        }
    }
/*
    public class NoQuestsWithoutComms_Patch
    {
        [HarmonyPatch(typeof(IncidentWorker_GiveQuest), "CanFireNowSub")]
        public class Patch1
        {
            [HarmonyPrefix]
            static bool NQWC_Prefix()
            {
                List<Map> maps = Find.Maps;
                for (int i = 0; i < maps.Count; i++)
                {
                    if (CommsConsoleUtility.PlayerHasPoweredCommsConsole(maps[i]))
                    {
                        return true;
                    }
                }
                if (ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Tribal Signal Fire (Continued)"))
                {
                    for (int i = 0; i < maps.Count; i++)
                    {
                        foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("SignalFire")))
                        {
                            if (building.Faction == Faction.OfPlayer)
                            {
                                CompGlower compGlower = building.TryGetComp<CompGlower>();
                                if (compGlower.Glows)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                if (ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Nopower Comms Simplified"))
                {
                    for (int i = 0; i < maps.Count; i++)
                    {
                        foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("BirdPostMessageTable")))
                        {
                            if(building.Faction == Faction.OfPlayer)
                            {
                                CompPowerTrader compPowerTrader = building.TryGetComp<CompPowerTrader>();
                                if (compPowerTrader == null || compPowerTrader.PowerOn)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(StorytellerComp_SingleOnceFixed), "MakeIntervalIncidents")]
        public class Patch2
        {
            static IEnumerable<FiringIncident> NQWC_Postfix(IIncidentTarget target, StorytellerComp_SingleOnceFixed __instance)
            {
                int num = Find.TickManager.TicksGame / 1000;
                StorytellerCompProperties_SingleOnceFixed newProps = (StorytellerCompProperties_SingleOnceFixed)__instance.props;

                if (newProps.minColonistCount > 0)
                {
                    if (target.StoryState.lastFireTicks.ContainsKey(newProps.incident))
                    {
                        yield break;
                    }
                    if (PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count < newProps.minColonistCount)
                    {
                        yield break;
                    }
                    num -= target.StoryState.GetTicksFromColonistCount(newProps.minColonistCount) / 1000;
                }
                if (num > newProps.fireAfterDaysPassed * 60 && !(target.StoryState.lastFireTicks.ContainsKey(newProps.incident)))
                {
                    if (newProps.skipIfColonistHasMinTitle != null)
                    {
                        List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists;
                        for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count; i++)
                        {
                            if (allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists[i].royalty != null && allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists[i].royalty.AllTitlesForReading.Any<RoyalTitle>() && allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists[i].royalty.MainTitle().seniority >= newProps.skipIfColonistHasMinTitle.seniority)
                            {
                                yield break;
                            }
                        }
                    }
                    Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
                    if (!newProps.skipIfOnExtremeBiome || (anyPlayerHomeMap != null && !anyPlayerHomeMap.Biome.isExtremeBiome))
                    {
                        IncidentDef incident = newProps.incident;
                        if (incident.TargetAllowed(target))
                        {
                            FiringIncident firingIncident = new FiringIncident(incident, __instance, __instance.GenerateParms(incident.category, target));
                            yield return firingIncident;
                        }
                    }
                }
                yield break;
            }
        }
    }
*/
}
	
