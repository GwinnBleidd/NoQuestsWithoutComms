using Verse;
using HarmonyLib;
using System.Reflection;
using System.Linq;
using System;

namespace NoQuestsWithoutComms {
	[StaticConstructorOnStartup]
	public class PatchMain {
		static public bool hasTribalSignalfire = false;
		static public bool hasNopowerCommsSimplified = false;
		static public bool hasIndustrialAge = false; 

		static public DateTime lastCheckComms = DateTime.Now;
		static public bool cachedResult = false;

		static PatchMain() {

			hasTribalSignalfire = ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Tribal Signal Fire (Continued)" || m.PackageId == "Mlie.TribalSignalFire");
			hasNopowerCommsSimplified = ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Nopower Comms Simplified" || m.PackageId == "Meltup.NopowerCommsSimplified");
			hasIndustrialAge = ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Industrial Age - Objects and Furniture (Continued)" || m.PackageId == "Mlie.CallofCthulhuIndustrialAge");

			D.Debug("hasTribalSignalfire = " + hasTribalSignalfire);
			D.Debug("hasNopowerCommsSimplified = " + hasNopowerCommsSimplified);
			D.Debug("hasIndustrialAge = " + hasIndustrialAge);

			var harmony = new Harmony("eBae.NoQuestsWithoutComms");
			var assembly = Assembly.GetExecutingAssembly();
			harmony.PatchAll(assembly);
        }
    }
}

