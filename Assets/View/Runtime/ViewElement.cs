using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkNaku.View {
    public abstract class ViewElement<T> : MonoBehaviour, IViewElement<T> where T : IViewHandler {

        protected T Handler { get; private set; }

        public void Initialize(T handler) {
            Handler = handler;

            OnInitialize();
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

        protected virtual void OnEnterBefore() {
        }

        protected virtual void OnEnterAfter() {
        }

        protected virtual void OnExitBefore() {
        }

        protected virtual void OnExitAfter() {
        }
    }
}