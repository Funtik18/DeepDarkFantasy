using UnityEngine;

namespace DDF.Events {
	public interface IInteraction {
		void OnTriggerEnter( Collider other );
		void OnTriggerStay( Collider other );
		void OnTriggerExit( Collider other );
	}
}
