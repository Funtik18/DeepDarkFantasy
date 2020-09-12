using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DDF {
    /// <summary>
    /// Класс ссылка, в этом объекте создаются объекты на выброс.
    /// </summary>
    public class ThrowPoint : MonoBehaviour {
        public static ThrowPoint _instance { get; private set; }

        public static void Init() {
            if (_instance == null)
                _instance = FindObjectOfType<ThrowPoint>();
        }
    }

}
