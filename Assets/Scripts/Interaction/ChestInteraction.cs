using UnityEngine;

public class ChestInteraction : MonoBehaviour, IInteraction {

	//UICanvas instance;

	private void Start() {
		//instance = UICanvas._instance;
	}


	public void OnTriggerEnter( Collider other ) {
		//instance.ShowTip(Interaction.E);
	}
	
	public void OnTriggerStay( Collider other ) {
		
	}

	public void OnTriggerExit( Collider other ) {
		//instance.CloseTip(Interaction.E);
	}
}
