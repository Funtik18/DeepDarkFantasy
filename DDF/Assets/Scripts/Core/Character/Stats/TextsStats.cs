using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Character.Stats {
    public class TextsStats : MonoBehaviour {
        
		public List<TextStat> textsStats;
		public void UpdateAllTxt() {
			for(int i = 0; i < textsStats.Count; i++) {
				textsStats[i].UpdateText();
			}
		}
	}
}