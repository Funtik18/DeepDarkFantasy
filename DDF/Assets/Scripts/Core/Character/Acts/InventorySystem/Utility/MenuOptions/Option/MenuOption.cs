using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DDF.UI.Inventory {
    [Serializable]
    [RequireComponent(typeof(Button))]
    public class MenuOption : MonoBehaviour {
        [SerializeField]
        private string option;
        public string Option {
            get {
                return option;
            } set {
				if (option != value) {
                    option = value;
                    text.text = option;
				}
            } }
        [SerializeField]
        private TMPro.TextMeshProUGUI text;
        [SerializeField]
        private UnityEvent currentEvent = new UnityEvent();

		private void Awake() {
            GetComponent<Button>().onClick.AddListener(delegate { currentEvent?.Invoke(); });
		}
        public void SetAction( UnityAction call ) {
            currentEvent.RemoveAllListeners();
            currentEvent.AddListener(call);
		}

        
    }
}