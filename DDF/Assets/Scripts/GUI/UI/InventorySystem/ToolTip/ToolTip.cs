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
		[SerializeField] private Color rarityCommon = new Color32(255, 255, 255, 255);
		[SerializeField] private Color rarityRare = new Color32(66, 135, 245, 255);
		[SerializeField] private Color rarityEpic = new Color32(218, 66, 245, 255);
		[SerializeField] private Color raritySet = new Color32(108, 245, 66, 255);
		[SerializeField] private Color rarityLegendary = new Color32(245, 245, 66, 255);

		[SerializeField] private TMPro.TextMeshProUGUI itemName;
		[SerializeField] private TMPro.TextMeshProUGUI itemPower;
		[SerializeField] private TMPro.TextMeshProUGUI itemPrimaryType;
		[SerializeField] private TMPro.TextMeshProUGUI itemSecondaryType;
		[SerializeField] private TMPro.TextMeshProUGUI itemDescription;
		[SerializeField] private TMPro.TextMeshProUGUI itemAnotation;
		[SerializeField] private TMPro.TextMeshProUGUI itemRarity;
		[SerializeField] private TMPro.TextMeshProUGUI itemDurarity;



		private float maxHeight = 0;
		private float maxWidth = 0;


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

			maxHeight = Screen.height;
			maxWidth = Screen.width/4;
		}
		public void SetPosition( Vector2 newPos ) {
			transform.position = newPos;
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

			switch (item.rarity) {
				case ItemRarity.Common: {
					itemRarity.color = rarityCommon;
					itemName.color = rarityCommon;
				}
				break;
				case ItemRarity.Rare: {
					itemRarity.color = rarityRare;
					itemName.color = rarityRare;
				}
				break;
				case ItemRarity.Epic: {
					itemRarity.color = rarityEpic;
					itemName.color = rarityEpic;
				}
				break;
				case ItemRarity.Set: {
					itemRarity.color = raritySet;
					itemName.color = raritySet;
				}
				break;
				case ItemRarity.Legendary: {
					itemRarity.color = rarityLegendary;
					itemName.color = rarityLegendary;
				}
				break;
			}
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
			if (item is ConsumableItem consumableItem) {
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
			
			ReSizeToolTip();
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

		private void ReSizeToolTip() {
			Vector2 currentSize = new Vector2(250f, 150f);

			Vector2 realSize = Vector2.zero;
			realSize += itemName.GetPreferredValues();
			realSize += itemPower.GetPreferredValues();
			realSize += itemDescription.GetPreferredValues();
			realSize += itemAnotation.GetPreferredValues();
			realSize += itemRarity.GetPreferredValues();

			if(currentSize.y < realSize.y) {
				currentSize.y = realSize.y+10;
			}

			rect.sizeDelta = currentSize;
		}

		public void ShowToolTip() {
			HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup);

			isHide = false;
		}
		public void HideToolTip() {
			HelpFunctions.CanvasGroupSeer.DisableGameObject(canvasGroup);
			currentItem = null;
			isHide = true;
		}
	}
}