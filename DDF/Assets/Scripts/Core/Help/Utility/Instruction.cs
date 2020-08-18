using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.Help {

	public abstract class Instruction : IEnumerator, IInstruction {

        private Instruction current;
        object IEnumerator.Current => current;

        private object routine;
        public MonoBehaviour Parent { get; private set; }

        public bool isExecuting { get; private set; }
        public bool isPaused { get; private set; }
        public bool isStoped { get; private set; }

        public event Action<Instruction> onStarted;
        public event Action<Instruction> onPaused;
        public event Action<Instruction> onTerminated;
        public event Action<Instruction> onDone;
        
        protected Instruction( MonoBehaviour parent ) => Parent = parent;

        void IEnumerator.Reset() {
            isPaused = false;
            isStoped = false;

            routine = null;
        }

        bool IEnumerator.MoveNext() {
            if (isStoped) {
                ( this as IEnumerator ).Reset();
                return false;
            }

            if (!isExecuting) {
                isExecuting = true;
                routine = new object();

                OnStarted();
                onStarted?.Invoke(this);
            }

            if (current != null)
                return true;

            if (isPaused)
                return true;

            if (!Update()) {
                OnDone();
                onDone?.Invoke(this);

                isStoped = true;
                return false;
            }

            return true;
        }

        public void Pause() {
            if (isExecuting && !isPaused) {
                isPaused = true;

                OnPaused();
                onPaused?.Invoke(this);
            }
        }
        public void Resume() {
            isPaused = false;
            OnResumed();
        }
        public void Terminate() {
            if (Stop()) {
                OnTerminated();
                onTerminated?.Invoke(this);
            }
        }
        private bool Stop() {
            if (isExecuting) {
                if (routine is Coroutine)
                    Parent.StopCoroutine(routine as Coroutine);

                ( this as IEnumerator ).Reset();

                return isStoped = true;
            }

            return false;
        }
        public Instruction Execute() {
            if (current != null) {
                Debug.LogWarning($"Instruction { GetType().Name} is currently waiting for another one and can't be stared right now.");
                return this;
            }

            if (!isExecuting) {
                isExecuting = true;
                routine = Parent.StartCoroutine(this);

                return this;
            }

            Debug.LogWarning($"Instruction { GetType().Name} is already executing.");
            return this;
        }
        public Instruction Execute( MonoBehaviour parent ) {
            if (current != null) {
                Debug.LogWarning($"Instruction { GetType().Name} is currently waiting for another one and can't be stared right now.");
                return this;
            }

            if (!isExecuting) {
                isExecuting = true;
                routine = ( Parent = parent ).StartCoroutine(this);

                return this;
            }

            Debug.LogWarning($"Instruction { GetType().Name} is already executing.");
            return this;
        }

        public void Reset() {
            Terminate();

            onStarted = null;
            onPaused = null;
            onTerminated = null;
            onDone = null;
        }

        public virtual void OnStarted() { }
        public virtual void OnPaused() { }
        public virtual void OnResumed() { }
        public virtual void OnTerminated() { }
        public virtual void OnDone() { }

        protected abstract bool Update();
    }
}