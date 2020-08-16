using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Help {

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


			public static GameObject CreateObjectInParent( Transform parent, GameObject obj, string ObjName = "NewGameObject" ) {
				GameObject newobj = Instantiate(obj);
				newobj.name = ObjName;

				Transform temp = newobj.transform;

				temp.SetParent(parent);
				temp.localPosition = Vector3.zero;
				temp.localScale = Vector3.one;
				temp.localRotation = Quaternion.identity;

				return newobj;
			}
			public static T CreateObjectInParent<T>( Transform parent, T obj, string ObjName = "NewGameObject" ) {
				GameObject newobj = Instantiate(obj as GameObject);
				newobj.name = ObjName;

				Transform temp = newobj.transform;

				temp.SetParent(parent);
				temp.localPosition = Vector3.zero;
				temp.localScale = Vector3.one;
				temp.localRotation = Quaternion.identity;

				return newobj.GetComponent<T>();
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
			public static void EnableGameObject( CanvasGroup canvasGroup, bool iteract = false) {
				canvasGroup.alpha = 1;
				canvasGroup.blocksRaycasts = iteract;
				canvasGroup.interactable = iteract;
				
			}
			public static void DisableGameObject( CanvasGroup canvasGroup, bool iteract = false ) {
				canvasGroup.alpha = 0;
				canvasGroup.blocksRaycasts = iteract;
				canvasGroup.interactable = iteract;
			}
		}


		public class Crypto {
			public static string GetNewGuid() {
				return System.Guid.NewGuid().ToString();
			}
		}
	}
}