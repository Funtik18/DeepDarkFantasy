using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DDF.UI.GUI {

    public class NavigationButton : MonoBehaviour {
        [HideInInspector]
        public int id;
        
        public Button button;
        public Image icon;

        public Color hoverColor;
        public Color curentColor;

        public UnityAction<NavigationButton> onGetThis;
        public UnityAction onClick;

        private void Awake() {
            button.onClick.AddListener(delegate { onGetThis.Invoke(this); onClick.Invoke(); });
        }


	}
}