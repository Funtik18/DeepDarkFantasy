using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Events {
    /// <summary>
    /// Интерфейс слушателя событий.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGameEventListener<T> {
        void OnEventRaised( T item );
    }
}