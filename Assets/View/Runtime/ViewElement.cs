using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkNaku.View {
    public abstract class ViewElement<T> : MonoBehaviour, IViewElement<T> where T : IViewHandler {

        protected T Handler { get; private set; }

        private bool _isInitialized;
        private bool _isReleased;

        public void Initialize(T handler) {
            if (_isInitialized) return;

            Handler = handler;

            OnInitialize();
        }

        public void Release() {
            if (_isReleased) return;

            OnRelease();

            _isReleased = true;
        }

        public void EnterBefore() {
            OnEnterBefore();
        }

        public void EnterAfter() {
            OnEnterAfter();
        }

        public void ExitBefore() {
            OnExitBefore();
        }

        public void ExitAfter() {
            OnExitAfter();
        }

        protected virtual void OnInitialize() {
        }

        protected virtual void OnRelease() {
        }

        protected virtual void OnEnterBefore() {
        }

        protected virtual void OnEnterAfter() {
        }

        protected virtual void OnExitBefore() {
        }

        protected virtual void OnExitAfter() {
        }

        private void OnDestroy() {
            Release();
        }
    }
}