using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF {
    public class GameProcess {

        private static GameState state;
        public static GameState State {
			get {
                return state;
			}
		}
        public static void Pause() {
            Time.timeScale = 0.2f;
            state = GameState.pause;
        }
        public static void Resume() {
            Time.timeScale = 1f;
            state = GameState.stream;
        }
    }
    public enum GameState {
        stream,
        pause,
        slowmo
    }
}