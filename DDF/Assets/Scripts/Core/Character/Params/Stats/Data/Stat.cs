﻿using DDF.Character.Perks;
using System.Collections.Generic;

namespace DDF.Character.Stats {
	public class Stat {
		public string statName;

		public virtual string Output() {
			return statName;
		}
	}
	public class StatInt : Stat {
		public int amount;

		string regex = "";

		/// <summary>
		/// Костыль спасающий от выхода за ноль при распределении доступных очков.
		/// </summary>
		public bool IsCanDecreace {
			get {
				return ( amount - calculateExtra() ) <= 1 ? false : true ;
			}
		}
		List<PerkInt> extra;

		public StatInt(string newName, int max, string _regex = "" ) {
			statName = newName;
			amount = max;
			regex = _regex;

			extra = new List<PerkInt>();
		}
		public void AddExtra( PerkInt perk ) {
			extra.Add(perk);
		}
		public override string Output() {
			int extr = calculateExtra();
			if (extr != 0)
				return statName + "|" + (amount - extr) + "<color=lightblue>(+" + extr + ")</color> = " + amount + regex;
			else
				return statName + "|" + amount + regex;
		}
		private int calculateExtra() {
			int extr = 0;
			for (int i = 0; i < extra.Count; i++) {
				extr += extra[i].amount;
			}
			return extr;
		}
	}
	public class StatFloat : Stat {
		public float amount;

		string regex = "";

		public StatFloat( string newName, float max, string _regex = "" ) {
			statName = newName;
			amount = max;
			regex = _regex;
		}
		public override string Output() {
			return statName + "|" + amount + regex;
		}
	}
	public class StatRegularInt : StatInt {
		public int currentInamount;

		string regex = "";

		public StatRegularInt( string newName, int value, int max, string _regex = "/" ) : base(newName, max) {
			currentInamount = value;
			regex = _regex;
		}
		public override string Output() {
			return statName + "|" + currentInamount + regex + amount;
		}
	}
	public class StatRegularFloat : StatFloat {
		public float currentInamount;

		string regex = "";

		public StatRegularFloat( string newName, float value, float max, string _regex = "/" ) : base(newName, max) {
			currentInamount = value;
			regex = _regex;
		}
		public override string Output() {
			return statName + "|" + currentInamount + regex + amount;
		}
	}
}