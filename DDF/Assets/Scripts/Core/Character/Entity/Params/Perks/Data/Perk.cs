using DDF.Character.Stats;
using System.Collections.Generic;


namespace DDF.Character.Perks {
	public abstract class Perk {
		public string perkName;

		public Perk( string newName) {
			perkName = newName;
		}

		public abstract string PerkNameAndCost();
		public abstract string PerkBuffs();
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

		public override string PerkNameAndCost() {
			return perkName + "|" + cost;
		}
		public override string PerkBuffs() {
			return stat.varName + "|" + amount;
		}

		public override void Calculate() {
			stat.AddExtra(this);
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
		public override string PerkNameAndCost() {
			return perkName + "|" + cost;
		}
		public override string PerkBuffs() {
			return stat.varName + "|" + amount;
		}

		public override void Calculate() {
			stat.amount += amount;
		}
	}
	public class PerkComplex : Perk {
		List<PerkInt> intPerks;
		List<PerkFloat> floatPerks;
		List<PerkComplex> complexPerks;
		public int cost;

		public PerkComplex(string newName, List<Perk> perks, int value2 ) : base(newName) {
			intPerks = new List<PerkInt>();
			floatPerks = new List<PerkFloat>();
			complexPerks = new List<PerkComplex>();
			for (int i = 0; i < perks.Count; i++) {
				if(perks[i] is PerkInt perkInt) {
					intPerks.Add(perkInt);
				}
				if (perks[i] is PerkFloat perkFloat) {
					floatPerks.Add(perkFloat);
				}
				if (perks[i] is PerkComplex complexPerk) {
					complexPerks.Add(complexPerk);
				}
			}
			cost = value2;
		}

		public override string PerkNameAndCost() {
			return perkName + "|" + cost;
		}
		public override string PerkBuffs() {
			string output = "";
			for (int i = 0; i < intPerks.Count; i++) {
				output += intPerks[i].perkName + "|" + intPerks[i].amount + "\n";
			}
			for (int i = 0; i < floatPerks.Count; i++) {
				output += floatPerks[i].perkName + "|" + floatPerks[i].amount + "\n";
			}
			for (int i = 0; i < complexPerks.Count; i++) {
				output += complexPerks[i].perkName + "|" /*+ complexPerks[i].amount*/ + "\n";
			}
			return output;
		}

		public override void Calculate() {
			for (int i = 0; i < intPerks.Count; i++) {
				intPerks[i].Calculate();
			}
			for (int i = 0; i < floatPerks.Count; i++) {
				floatPerks[i].Calculate();
			}
			for (int i = 0; i < complexPerks.Count; i++) {
				complexPerks[i].Calculate();
			}
		}
	}
}