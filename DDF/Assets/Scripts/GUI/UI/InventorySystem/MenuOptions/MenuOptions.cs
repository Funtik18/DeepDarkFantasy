using DDF.Help;
using DDF.UI.Inventory.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.UI.Inventory {
	[RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
	public class MenuOptions : MonoBehaviour {

        public static MenuOptions _instance;

		private CanvasGroup canvasGroup;

		public RectTransform rect { get { return GetComponent<RectTransform>(); } }

		public GameObject optionPrefab;
		public List<MenuOption> options;

		private bool isHide = true;
		public bool IsHide { get { return isHide; } }

		private Item currentItem;
		


		private void Awake() {
			_instance = this;
		}
		private void Start() {
			canvasGroup = GetComponent<CanvasGroup>();

			options = new List<MenuOption>();
		}

		public void AddNewOption(string optionName, UnityAction call) {
			GameObject obj = HelpFunctions.TransformSeer.CreateObjectInParent(transform, optionPrefab);
			obj.name = optionName + "-option-" + options.Count;

			Transform objTrans = obj.transform;
			objTrans.localPosition = new Vector3(objTrans.localPosition.x, -((objTrans as RectTransform).sizeDelta.y * options.Count));

			MenuOption option = obj.GetComponent<MenuOption>();
			option.Option = optionName;
			option.SetAction(call);

			options.Add(option);
		}
		private void OptionUse() {
			print("Use with " + currentItem.name);
		}
		private void OptionOpen() {
			ItemType type = currentItem.GetItemType();
			if (type is PouchType) {
				string findId = ( type as PouchType ).inventoryReference;
				print(findId);
				List<Inventory> inventories = InventoryOverSeer._instance.containers;
				Inventory finder = inventories.Find(x => x.inventoryID == findId);

				if(finder == null) {
					Debug.LogError("ERROR i cant find this id - " + findId);
					return;
				}
				CanvasGroup obj = finder.GetComponent<CanvasGroup>();

				HelpFunctions.CanvasGroupSeer.EnableGameObject(obj, true);
			} else {
				Debug.LogError("ERROR");
			}
		}
		private void Option2() {
			print("-");
		}
		private void Option3() {
			print("+-");
		}
		private void DefaultOption() {
			print("default");
		}


		public void SetPosition(Vector3 position ) {
			transform.position = position;
		}

		public void SetCurrentItem( Item item ) {
			currentItem = item;
		}

		public void OpenMenu() {

			HelpFunctions.TransformSeer.DestroyChildrenInParent(transform);
			options.Clear();

			List<ItemTag> tags = currentItem.GetItemType().tags;
			for(int i = 0; i < tags.Count; i++) {

				UnityAction call = DetermineAction(tags[i].name);

				AddNewOption(tags[i].tag, delegate { call?.Invoke(); CloseMenu(); });
			}

			HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup, true);

			isHide = false;
		}

		public UnityAction DetermineAction(string tag) {

			UnityAction call;

			switch (tag) {
				case "Use": {
					call = OptionUse;
					return call;
				}
				break;
				case "Open": {
					call = OptionOpen;
					return call;
				}
				break;

				/*case "Read": {
					call = Option1;
					return call;
				}break;
				case "Drink": {
					call = Option1;
					return call;
				}
				break;
				case "Eat": {
					call = Option1;
					return call;
				}
				break;
				case "Equip": {
					call = Option1;
					return call;
				}
				case "TakeOff": {
					call = Option1;
					return call;
				}
				break;
				break;
				case "Identify": {
					call = Option1;
					return call;
				}
				break;
				case "Open": {
					call = Option1;
					return call;
				}
				break;*/



				default: {
					call = DefaultOption;
					return call;
				}
				break;
			}
		}


		public void CloseMenu() {
			HelpFunctions.CanvasGroupSeer.DisableGameObject(canvasGroup);

			isHide = true;
		}
	}
}