using DDF.Help;
using UnityEngine;
using UnityEngine.UI;

namespace DDF.Inventory {
    /// <summary>
    /// Отвечает за картинку предмета в инвентаре.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class InventoryModel : MonoBehaviour {

        [SerializeField]
        private Image icon;
        public Image Icon {
            get => this.icon; 
            set => this.icon = value;
        }


        [SerializeField]
        private Image hightlight;
        public Image Hightlight { 
            get => this.hightlight; 
            set => this.hightlight = value;
        }


        [SerializeField]
        private TMPro.TextMeshProUGUI stackSize;
        private int LastStackCount = 1;
        public int StackCount {
            protected set {
                if (LastStackCount != value) {
                    //the stack count has changed for our item, update the text
                    LastStackCount = value;
                    if (value < 2) stackSize.enabled = false;
                    else {
                        stackSize.text = value.ToString();
                        stackSize.enabled = true;
                    }
                } else if (value < 2) stackSize.enabled = false;
            }

            get {
                if (string.IsNullOrEmpty(stackSize.text)) return 0;
                else return int.Parse(stackSize.text);
            }
        }

        private Item item = null;
        public Item referenceItem {
			set {
                item = value;
                if (item == null) return;
                reference = item.GetId();
                //StackCount = (int)item.StackCount;
            }
			get {
                return item;

            }
		}


		public string reference = HelpFunctions.Crypto.GetNewGuid();
    }
}