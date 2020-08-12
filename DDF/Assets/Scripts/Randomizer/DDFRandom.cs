using UnityEngine;

namespace DDF.Randomizer {
    public class DDFRandom {

        public DDFRandom() {
		}

        public float RandomBetween(float x, float y ) {
            float output = Random.Range(x, y);
            return output;
        }
    }
}