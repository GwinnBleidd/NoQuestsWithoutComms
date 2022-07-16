using System;
using System.Collections.Generic;
using Verse;

namespace NoQuestsWithoutComms {
	static class D {

		public static void Text(string line, int depth = 0) {
			Log.Message(line);
		}

		public static void Debug(string title) {
			if (NQWCMod.settings.allowDebugOutput) {
				D.Text("NoQuestsWithoutComms: " + title, 0);
			}
		}

		public static void Verbose(string title) {
		    D.Text("NoQuestsWithoutComms: " + title, 0);
		}
	}
}
