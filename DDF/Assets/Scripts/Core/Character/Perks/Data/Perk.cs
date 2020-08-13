using DDF.Character.Stats;
using UnityEngine;


namespace DDF.Character.Perks {
	public abstract class Perk {
		public string perkName;

		public abstract void Calculate();
	}
	public class PerkInt : Perk {
		private StatInt stat;
		private int amount;


		public PerkInt( Stat trackStat, int value) {
			stat = trackStat as StatInt;
			amount = value;
		}

		public override void Calculate() {
			stat.amount += amount;
		}
	}
}