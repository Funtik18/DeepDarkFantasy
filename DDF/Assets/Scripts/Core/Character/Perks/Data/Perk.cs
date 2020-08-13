using DDF.Character.Stats;
using System.Collections.Generic;


namespace DDF.Character.Perks {
	public abstract class Perk {
		public string perkName;

		public Perk( string newName) {
			perkName = newName;
		}

		public abstract void Calculate();
	}
	public class PerkInt : Perk {
		private StatInt stat;
		private int amount;

		public PerkInt(string newName, Stat trackStat, int value) : base(newName) {
			stat = trackStat as StatInt;
			amount = value;
		}

		public override void Calculate() {
			stat.amount += amount;
		}
	}
	public class PerkFloat : Perk {
		private StatFloat stat;
		private float amount;


		public PerkFloat(string newName, Stat trackStat, float value ) : base(newName) {
			stat = trackStat as StatFloat;
			amount = value;
		}

		public override void Calculate() {
			stat.amount += amount;
		}
	}
	public class ComplexPerk : Perk {
		List<PerkInt> intPerks;
		List<PerkFloat> floatPerks;

		public ComplexPerk(string newName, List<Perk> perks) : base(newName) {
			for (int i = 0; i < perks.Count; i++) {
				if(perks[i] is PerkInt perkInt) {
					intPerks.Add(perkInt);
				}
				if (perks[i] is PerkFloat perkFloat) {
					floatPerks.Add(perkFloat);
				}
			}
		}

		public override void Calculate() {
			for (int i = 0; i < intPerks.Count; i++) {
				intPerks[i].Calculate();
			}
			for (int i = 0; i < floatPerks.Count; i++) {
				floatPerks[i].Calculate();
			}
		}
	}
}