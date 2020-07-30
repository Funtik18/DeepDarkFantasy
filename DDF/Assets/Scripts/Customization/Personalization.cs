using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Personalization : MonoBehaviour {
    
    [Tooltip("Object to edit")]public SkinnedMeshRenderer meshRenderer;

	public Slider sliderCheek;
	public Slider sliderChin;

	private Mesh mesh;

	private void Awake() {
		mesh = meshRenderer.sharedMesh;

		List<string> blends = ParseBlendShapeToDictionary();

		mesh.GetBlendShapeIndex(blends[0]);

		sliderCheek.value = 0;
		sliderCheek.minValue = -100;
		sliderCheek.maxValue = 100;

		sliderChin.value = 0;
		sliderChin.minValue = -100;
		sliderChin.maxValue = 100;

		sliderCheek.onValueChanged.AddListener(delegate { Cheeks(sliderCheek.value); });
		sliderChin.onValueChanged.AddListener(delegate { Chin(sliderChin.value); });
	}

	private void Cheeks(float value) {
		if (value < 0) {
			meshRenderer.SetBlendShapeWeight(0, value*-1);
		}
		if (value >= 0) {
			meshRenderer.SetBlendShapeWeight(1, value);
		}
	}
	private void Chin( float value ) {
		if (value < 0) {
			meshRenderer.SetBlendShapeWeight(2, value * -1);
		}
		if (value >= 0) {
			meshRenderer.SetBlendShapeWeight(3, value);
		}
	}


	private List<string> ParseBlendShapeToDictionary() {
		List<string> blendShapesNames = Enumerable.Range(0, mesh.blendShapeCount).Select(x=>mesh.GetBlendShapeName(x)).ToList();
		return blendShapesNames;
	}
}
