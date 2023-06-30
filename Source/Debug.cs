using System;
using System.Collections.Generic;
using Verse;

namespace NoQuestsWithoutComms {
	static class D {

		public static void Text(string line, int depth = 0) {
			Log.Message(line);
		}

		public static void Debug(string title, bool suppressModSignature = false) {
			if (NQWCMod.settings.allowDebugOutput) {
				if (suppressModSignature) { 
					D.Text(title, 0);
				} else { 
					D.Text("NoQuestsWithoutComms: " + title, 0);
				}
			}
		}

		public static void Verbose(string title) {
			D.Text("NoQuestsWithoutComms: " + title, 0);
		}
	}
}
