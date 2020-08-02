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

		public CanvasGroup canvasGroup;

		public RectTransform rect { get { return GetComponent<RectTransform>(); } }

		public GameObject optionPrefab;
		public List<MenuOption> options;

		private bool isHide = true;
		public bool IsHide { get { return isHide; } }

		private void Awake() {
			_instance = this;
		}
		private void Start() {
			canvasGroup = GetComponent<CanvasGroup>();

			options = new List<MenuOption>();
			AddNewOption("Open", delegate { Option1(); CloseMenu(); });
			AddNewOption("Read", delegate { Option2(); CloseMenu(); });
			AddNewOption("Use", delegate { Option3(); CloseMenu(); });

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
		private void Option1() {
			print("+");
		}
		private void Option2() {
			print("-");
		}
		private void Option3() {
			print("+-");
		}

		public void SetPosition(Vector3 position ) {
			transform.position = position;
		}

		public void OpenMenu() {

			ReBuildMenu();

			HelpFunctions.CanvasGroupSeer.EnableGameObject(canvasGroup, true);

			isHide = false;
		}
		public void CloseMenu() {
			HelpFunctions.CanvasGroupSeer.DisableGameObject(canvasGroup);

			isHide = true;
		}

		public void ReBuildMenu() {
			Item item = InventoryOverSeer._instance.lastItem;

			
		}
	}
}