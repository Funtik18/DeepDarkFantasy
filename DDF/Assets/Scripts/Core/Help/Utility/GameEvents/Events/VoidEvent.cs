using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Events {
    [CreateAssetMenu(fileName = "VoidEvent", menuName = "DDF/Events/VoidEvent")]
    public class VoidEvent : BaseGameEvent<Void> {
        public void Raise() {
            Raise(new Void());
		}
    }
}