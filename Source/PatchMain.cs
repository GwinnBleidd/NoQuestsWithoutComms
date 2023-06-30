using Verse;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System;

namespace NoQuestsWithoutComms {
	[StaticConstructorOnStartup]
	public class PatchMain {
		static public bool hasTribalSignalfire = false;
		static public bool hasNopowerCommsSimplified = false;
		static public bool hasIndustrialAge = false; 
		static public bool hasMedievalOverhaul = false; 

		static public DateTime lastCheckComms = DateTime.Now;
		static public bool cachedResult = false;

		static public List<string> allowedQuestsAndIncidents = new List<string>() { 
			"Beggars",
			"Hospitality_Refugee",
			"Intro_Wimp", 
			"Intro_Deserter",
			"RefugeePodCrash_Baby",
			"RefugeePodCrash",
			"ReliquaryPilgrims",
			"WandererJoins",
			"WandererJoinAbasia",
		};

		static PatchMain() {

			hasTribalSignalfire = ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Tribal Signal Fire (Continued)" || m.PackageId == "Mlie.TribalSignalFire");
			hasNopowerCommsSimplified = ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Nopower Comms Simplified" || m.PackageId == "Meltup.NopowerCommsSimplified");
			hasIndustrialAge = ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Industrial Age - Objects and Furniture (Continued)" || m.PackageId == "Mlie.CallofCthulhuIndustrialAge");
			hasMedievalOverhaul = ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Medieval Overhaul" || m.PackageId == "DankPyon.Medieval.Overhaul");

			D.Debug("hasTribalSignalfire = " + hasTribalSignalfire);
			D.Debug("hasNopowerCommsSimplified = " + hasNopowerCommsSimplified);
			D.Debug("hasIndustrialAge = " + hasIndustrialAge);
			D.Debug("hasMedievalOverhaul = " + hasMedievalOverhaul);

			if (NQWCMod.settings.allowLocalIncidents) { 
				D.Debug("Local Incidents Allowed:");
				foreach(string s in allowedQuestsAndIncidents) { 
					D.Debug("		" + s, true);
				}
			}

			var harmony = new Harmony("eBae.NoQuestsWithoutComms");
			var assembly = Assembly.GetExecutingAssembly();
			harmony.PatchAll(assembly);
		}
	}
}

