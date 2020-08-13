using DDF.Character.Stats;
using System.Collections.Generic;


namespace DDF.Character.Perks {
	public abstract class Perk {
		public string perkName;

		public Perk( string newName) {
			perkName = newName;
		}

		public virtual string Output() {
			return perkName;
		}

		public abstract void Calculate();
	}
	public class PerkInt : Perk {
		private StatInt stat;
		public int amount;
		public int cost;

		public PerkInt(string newName, Stat trackStat, int value, int value2) : base(newName) {
			stat = trackStat as StatInt;
			amount = value;
			cost = value2;
		}

		public override string Output() {
			return perkName + "|" + cost;
		}

		public override void Calculate() {
			stat.amount += amount;
		}
	}
	public class PerkFloat : Perk {
		private StatFloat stat;
		public float amount;
		public int cost;


		public PerkFloat(string newName, Stat trackStat, float value, int value2 ) : base(newName) {
			stat = trackStat as StatFloat;
			amount = value;
			cost = value2;
		}
		public override string Output() {
			return perkName + "|" + cost;
		}

		public override void Calculate() {
			stat.amount += amount;
		}
	}
	public class ComplexPerk : Perk {
		List<PerkInt> intPerks;
		List<PerkFloat> floatPerks;
		public int cost;

		public ComplexPerk(string newName, List<Perk> perks, int value2 ) : base(newName) {
			for (int i = 0; i < perks.Count; i++) {
				if(perks[i] is PerkInt perkInt) {
					intPerks.Add(perkInt);
				}
				if (perks[i] is PerkFloat perkFloat) {
					floatPerks.Add(perkFloat);
				}
			}
			cost = value2;
		}

		public override string Output() {
			return perkName + "|" + cost;
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