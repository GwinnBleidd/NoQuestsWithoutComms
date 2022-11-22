using System;
using System.Collections.Generic;
using Verse;
using HarmonyLib;
using RimWorld;

namespace NoQuestsWithoutComms
{
    [HarmonyPatch(typeof(IncidentWorker_GiveQuest))]
    [HarmonyPatch("CanFireNowSub")]
    [HarmonyPatch(new Type[] { typeof(IncidentParms)})]

    internal class IncidentWorker_GiveQuest_CanFireNowSub {
        public static bool Prefix(ref bool __result, IncidentWorker_GiveQuest __instance) {
            D.Debug("IncidentWorker_GiveQuest_CanFireNowSub. Incident = " + __instance.def.defName + ". ScriptDef = " + (__instance.def.questScriptDef != null ? __instance.def.questScriptDef.defName : "NULL"));

            if (NQWCMod.settings.allowLocalIncidents) {
                if (__instance.def.defName == "GiveQuest_Random" || (__instance.def.questScriptDef != null && PatchMain.allowedQuestsAndIncidents.Contains(__instance.def.questScriptDef.defName))) {
                    D.Debug("Allowed incident: " + __instance.def.questScriptDef?.defName);
                    __result = true;
                    return true;
                }
            }

            TimeSpan interval = DateTime.Now - PatchMain.lastCheckComms;
            if (interval.TotalSeconds >= 2f) {
                DateTime scriptStart = DateTime.Now;

                bool tmpResult = false;
                List<Map> maps = Find.Maps;
                for (int i = 0; i < maps.Count; i++) {
                    if (CommsConsoleUtility.PlayerHasPoweredCommsConsole(maps[i])) {
                        D.Debug("Player has powered Comms Console");
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
                                    D.Debug("Player has working Signal Fire");
                                    tmpResult = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!tmpResult && PatchMain.hasIndustrialAge) {
                    for (int i = 0; i < maps.Count && !tmpResult; i++) {
                        if (!tmpResult) {
                            foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("Estate_Radio"))) {
                                if (building.Faction == Faction.OfPlayer) {
                                    CompPowerTrader compPowerTrader = building.TryGetComp<CompPowerTrader>();
                                    if (compPowerTrader == null || compPowerTrader.PowerOn) {
                                        tmpResult = true;
                                        D.Debug("Player has powered Estate_Radio");
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
                                    D.Debug("Player has working BirdPostMessageTable");
                                    tmpResult = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!tmpResult && PatchMain.hasMedievalOverhaul) {
                    for (int i = 0; i < maps.Count && !tmpResult; i++) {
                        foreach (Building building in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("DankPyon_ScribeTable"))) {
                            if(building.Faction == Faction.OfPlayer) {
                                CompPowerTrader compPowerTrader = building.TryGetComp<CompPowerTrader>();
                                if (compPowerTrader == null || compPowerTrader.PowerOn) {
                                    D.Debug("Player has working Scribe Table");
                                    tmpResult = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                PatchMain.lastCheckComms = DateTime.Now;
                PatchMain.cachedResult = tmpResult;

                TimeSpan ttr = DateTime.Now - scriptStart;

                D.Debug("TTR = " + ttr.TotalSeconds + "s, or " + ttr.TotalMilliseconds + "ms");
            }

            __result = PatchMain.cachedResult;
            return PatchMain.cachedResult;
        }
    }

    [HarmonyPatch(typeof(IncidentWorker_GiveQuest))]
    [HarmonyPatch("GiveQuest")]
    [HarmonyPatch(new Type[] { typeof(IncidentParms), typeof(QuestScriptDef)})]
    internal class IncidentWorker_GiveQuest_GiveQuest {
        public static bool Prefix(IncidentWorker_GiveQuest __instance, IncidentParms parms, QuestScriptDef questDef) {
            D.Debug("IncidentWorker_GiveQuest_GiveQuest. QuestScriptDef = " + questDef.defName);
            if (!PatchMain.cachedResult) { //can't have quests
                if (NQWCMod.settings.allowLocalIncidents) { // but local quests are allowed
                    if (!PatchMain.allowedQuestsAndIncidents.Contains(questDef.defName)) {
                        // quest is suppressed
                        D.Debug("Quest was SUPPRESSED");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}