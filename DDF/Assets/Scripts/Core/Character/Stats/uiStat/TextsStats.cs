using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.Character.Stats {

    public class TextsStats : MonoBehaviour {
        [SerializeField]
        private List<TextStat> textStats;

        public void Init(Queue<Stat> stats, Queue<UnityAction> increaseActions, Queue<UnityAction> decreaseActions) {
            if (textStats.Count != stats.Count) Debug.LogError("ERror");

            for (int i = 0; i < textStats.Count; i++) {
                textStats[i].SetTrack(stats.Dequeue(), increaseActions.Dequeue(), decreaseActions.Dequeue());
            }
        }
        public void UpdateAllTXT() {
            for(int i = 0; i < textStats.Count; i++) {
                textStats[i].UpdateText();
            }
		}
    }
}