using System.Collections;
using System;

using UnityEngine;

namespace DDF.Help {

    public sealed class CoroutineObject : CoroutineObjectBase {

        /// <summary>
        /// Делегат, с которым будет сообщен метод, выполняющий роль корутины.
        /// </summary>
        public Func<IEnumerator> Routine { get; private set; }
        
        public override event Action onFinished;

        public CoroutineObject( MonoBehaviour owner, Func<IEnumerator> routine ) {
            Owner = owner;
            Routine = routine;
        }


        public void Start() {
            Stop();

            Coroutine = Owner.StartCoroutine(Process());
        }
        /// <summary>
        /// Нужен для того, чтобы иметь возможность проследить, когда же завершится выполнение корутины, 
        /// сбросить ссылку на неё и выполнить в этот момент другой код (если понадобится).
        /// </summary>
        /// <returns></returns>
        private IEnumerator Process() {
            yield return Routine.Invoke();
            Coroutine = null;

            onFinished?.Invoke();
        }
        public void Stop() {
            if (IsProcessing) {
                Owner.StopCoroutine(Coroutine);
                Coroutine = null;
            }
        }
    }
}