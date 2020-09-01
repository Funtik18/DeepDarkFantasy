using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace DDF.UI.Inventory.Items {

    public class ItemsTags : MonoBehaviour {

		public static ItemsTags _instance { get; private set; }

		public TagTake tagTake;
		public TagThrow tagThrow;

		public TagEquip tagEquip;
		public TagTakeOff takeOff;

		public TagEat tagEat;
		public TagDrink tagDrink;


		[HideInInspector]public List<ItemTag> mainTags;

		public static void Init() {
			if (_instance == null)
				_instance = FindObjectOfType<ItemsTags>();
		}

		private void Awake() {
			mainTags = new List<ItemTag>();

			mainTags.Add(tagTake?.GetCopy());
			mainTags.Add(tagThrow?.GetCopy());

			mainTags.Add(tagEquip?.GetCopy());
			mainTags.Add(takeOff?.GetCopy());

			mainTags.Add(tagEat?.GetCopy());
			mainTags.Add(tagDrink?.GetCopy());
		}

		public ItemTag GetTag<T>() {
			ItemTag tag;
			for (int i = 0; i < mainTags.Count; i++) {
				tag = mainTags[i];
				if(tag is T) {
					return tag;
				}
			}
			return null;
		}
	}
}