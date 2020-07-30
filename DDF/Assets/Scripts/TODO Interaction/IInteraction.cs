using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteraction {
	void OnTriggerEnter( Collider other );
	void OnTriggerStay( Collider other );
	void OnTriggerExit( Collider other );
}
