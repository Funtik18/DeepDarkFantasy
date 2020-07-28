using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpFunctions : MonoBehaviour {

	public class TransformSeer {
		public static Transform[] TakeChildrenInParent( Transform parent ) {
			Transform[] childs = new Transform[parent.childCount];
			for (int i = 0; i < parent.childCount; i++) {
				childs[i] = parent.GetChild(i);
			}
			return childs;
		}
		public static void DestroyChildrenInParent( Transform parent ) {
			GameObject[] trashArray = new GameObject[parent.childCount];

			for (int i = 0; i < trashArray.Length; i++) {
				trashArray[i] = parent.GetChild(i).gameObject;
			}

			for (int i = 0; i < trashArray.Length; i++) {
				DestroyImmediate(trashArray[i]);
			}
		}


		public static GameObject CreateObjectInParen( Transform parent, GameObject obj ) {
			GameObject newobj = Instantiate(obj);
			Transform temp = newobj.transform;

			temp.SetParent(parent);
			temp.localPosition = Vector3.zero;
			temp.localScale = Vector3.one;
			temp.localRotation = Quaternion.identity;

			return newobj;
		}
		public static GameObject CreateObject( GameObject obj ) {
			GameObject newobj = Instantiate(obj);
			Transform temp = newobj.transform;

			temp.localPosition = Vector3.zero;
			temp.localScale = Vector3.one;
			temp.localRotation = Quaternion.identity;

			return newobj;
		}

		public static void DestroyObject( GameObject _obj ) {
			DestroyImmediate(_obj);
		}
	}

	public class CanvasGroupSeer {
		public static void EnableGameObject(CanvasGroup canvasGroup) {
			canvasGroup.alpha = 1;
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;
		}
		public static void DisableGameObject( CanvasGroup canvasGroup ) {
			canvasGroup.alpha = 0;
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;
		}
	}


	public class Crypto {
		public static string GetNewGuid() {
			return System.Guid.NewGuid().ToString();
		}
	}
	

	
}
