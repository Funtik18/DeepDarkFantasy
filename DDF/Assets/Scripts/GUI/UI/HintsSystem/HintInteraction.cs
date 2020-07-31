using UnityEngine;

namespace DDF.UI {
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class HintInteraction : MonoBehaviour {

        public bool isBillBoard = true;

        private Canvas canvas;
        private CanvasGroup canvasGroup;

        private Quaternion originalRotation;

        private void Awake() {
            canvas = GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();

            originalRotation = Quaternion.Euler(Vector3.zero);
        }

        public void OpenHint() {
            canvasGroup.alpha = 1;
        }
        public void CloseHint() {
            canvasGroup.alpha = 0;
        }
        public void OpenCloseHint() {

        }

        public void LookAtCamera(Camera camera) {
            if(isBillBoard)
                transform.rotation = camera.transform.rotation * originalRotation;

            //transform.LookAt(camera.transform);
        }
    }
}