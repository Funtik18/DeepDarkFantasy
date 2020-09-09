using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI {
	/// <summary>
	/// Класс ссылка, в этом объекте создаётся динамический ui.
	/// </summary>
    public class DinamicUI : MonoBehaviour {
        public static DinamicUI _instance { get; private set; }

        void Awake() {
            if (_instance != null) {
                Destroy(this.gameObject);
                return;
            }
            _instance = this;
        }
    }
}