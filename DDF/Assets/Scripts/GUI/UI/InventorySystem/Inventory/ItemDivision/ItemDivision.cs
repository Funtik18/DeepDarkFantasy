using DDF.Help;
using DDF.UI.Inventory.Items;
using UnityEngine;
using UnityEngine.UI;

namespace DDF.UI.Inventory {
    [RequireComponent(typeof(CanvasGroup))]
    public class ItemDivision : MonoBehaviour {

        public static ItemDivision _instance;

        private CanvasGroup canvasGroup;

        public Button increase;
        public Button decrease;
        public Slider slider;
        public Button ok;
        public Button nook;


        private bool isHide = true;
        public bool IsHide { get { return isHide; } }

        private Item currentItem;

        private void Awake() {
            _instance = this;

            canvasGroup = GetComponent<CanvasGroup>();

            increase.onClick.AddListener(delegate { slider.value++; });
            decrease.onClick.AddListener(delegate { slider.value--; });
            ok.onClick.AddListener(delegate { CloseDivision(); });
            nook.onClick.AddListener(delegate { CloseDivision(); });
        }

        public void SetPosition( Vector3 position ) {
            transform.position = position;
        }
        public void SetCurrentItem( Item item ) {
            currentItem = item;
        }


        public void OpenDivision() {

            slider.minValue = 1;
            slider.maxValue = currentItem.itemStackCount;
            slider.value = slider.minValue + 1;

            HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup, true);

            isHide = false;
        }
        public void CloseDivision() {
            HelpFunctions.CanvasGroupSeer.DisableGameObject(canvasGroup);

            isHide = true;
        }
    }
}