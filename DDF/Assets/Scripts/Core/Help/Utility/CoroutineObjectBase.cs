using System;
using UnityEngine;

namespace DDF.Help {

    public abstract class CoroutineObjectBase {
        /// <summary>
        ///  Владелец корутины.
        /// </summary>
        /// 
        public MonoBehaviour Owner { get; protected set; }
        /// <summary>
        /// Содержит ссылку на текущую корутину.
        /// </summary>
        public Coroutine Coroutine { get; protected set; }
        /// <summary>
        /// Позволяет узнать, выполняется ли корутина в текущий момент.
        /// </summary>
        public bool IsProcessing => Coroutine != null;

        public abstract event Action onFinished;

    }
}