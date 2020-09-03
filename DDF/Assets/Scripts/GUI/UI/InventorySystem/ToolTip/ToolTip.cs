using DDF.Help;
using DDF.UI.Inventory.Items;
using UnityEngine;

namespace DDF.UI.Inventory {
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CanvasGroup))]
	public class ToolTip : MonoBehaviour {

		private static ToolTip _instance;

		private CanvasGroup canvasGroup;
		[HideInInspector]public RectTransform rect;

		private Item currentItem;
		[SerializeField] private TMPro.TextMeshProUGUI itemName;
		[SerializeField] private TMPro.TextMeshProUGUI itemPower;
		[SerializeField] private TMPro.TextMeshProUGUI itemPrimaryType;
		[SerializeField] private TMPro.TextMeshProUGUI itemSecondaryType;
		[SerializeField] private TMPro.TextMeshProUGUI itemDescription;
		[SerializeField] private TMPro.TextMeshProUGUI itemAnotation;
		[SerializeField] private TMPro.TextMeshProUGUI itemRarity;
		[SerializeField] private TMPro.TextMeshProUGUI itemDurarity;

		private bool isHide = true;

		public static ToolTip GetInstance() {
			if(_instance == null) {
				_instance = FindObjectOfType<ToolTip>();
			}
			return _instance;
		}

		private void Awake() {
			canvasGroup = GetComponent<CanvasGroup>();
			rect = GetComponent<RectTransform>();
		}
		public void SetPosition( Vector2 newPos ) {
			transform.position = newPos;
		}

		public void ShowToolTip( string toolTipText ) {
			Resize();

			HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup);

			isHide = false;
		}

		public void SetItem( Item item ) {
			currentItem = item;
			SetInformation(currentItem);
		}
		private void SetInformation(Item item) {
			ResetInformation();
			itemName.text = item.itemName;
			itemDescription.text = item.itemDescription;
			itemAnotation.text = item.itemAnotation;
			itemRarity.text = item.rarity.ToString();

			if (item is ArmorItem armorItem) {
				itemPrimaryType.text = "Armor";
				itemPower.text = armorItem.armor.Output();
				switch (armorItem) {
					case HeadItem headItem: {
						itemSecondaryType.text = headItem.headType.ToString();
					}
					break;
					case TorsoItem torsoItem: {
						itemSecondaryType.text = torsoItem.torsoType.ToString();
					}
					break;
					case LegsItem legsItem: {
						itemSecondaryType.text = legsItem.legsType.ToString();
					}
					break;
					case FeetItem feetItem: {
						itemSecondaryType.text = feetItem.feetType.ToString();
					}
					break;
					case WaistItem waistItem: {
						itemSecondaryType.text = waistItem.waistType.ToString();
					}
					break;
					case WristItem wristItem: {
						itemSecondaryType.text = wristItem.wristType.ToString();
					}
					break;
					case JewerlyItem jewerlyItem: {
						itemSecondaryType.text = jewerlyItem.jewerlyType.ToString();
					}
					break;

					case OffHandItem offHandItem: {
						itemSecondaryType.text = offHandItem.offHandType.ToString();
					}
					break;
				}

			}
			if (item is WeaponItem weaponItem) {
				itemPrimaryType.text = "Weapon";
				itemPower.text = weaponItem.damage.Output();
				switch (weaponItem) {
					case OneHandedItem oneHandedItem: {
						itemSecondaryType.text = oneHandedItem.handedType.ToString();
					}break;
					case TwoHandedItem twoHandedItem: {
						itemSecondaryType.text = twoHandedItem.twoHandedType.ToString();
					}
					break;
					case RangedItem rangedItem: {
						itemSecondaryType.text = rangedItem.rangedType.ToString();
					}
					break;
				}
			}
			if(item is ConsumableItem consumableItem) {
				itemPrimaryType.text = "Consumable";
				switch (consumableItem) {
					case PotionItem potionItem: {
						itemSecondaryType.text = "Potion";
					}
					break;
					case FoodItem foodItem: {
						itemSecondaryType.text = "Food";
					}
					break;
				}
			}
		}
		private void ResetInformation() {
			itemName.text = "";
			itemPower.text = "";
			itemPrimaryType.text = "";
			itemSecondaryType.text = "";
			itemDescription.text = "";
			itemAnotation.text = "";
			itemRarity.text = "";
			itemDurarity.text = "";
		}
		public void ShowToolTip() {
			HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup);

			isHide = false;
		}

		public void HideToolTip() {
			HelpFunctions.CanvasGroupSeer.DisableGameObject(canvasGroup);

			isHide = true;
		}

		private void Resize() {
			//Vector2 backgroundSize = text.GetPreferredValues() + new Vector2(textPaddingSize * 2f, textPaddingSize * 2f);
			//background.sizeDelta = backgroundSize;
		}
	}
}