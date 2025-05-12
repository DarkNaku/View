using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkNaku.View {
    public interface IViewElement<T> where T : IViewHandler {
        void Initialize(T handler);
        void Release();
        void EnterBefore();
        void EnterAfter();
        void ExitBefore();
        void ExitAfter();
    }
}