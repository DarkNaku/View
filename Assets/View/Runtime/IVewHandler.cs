using System.Collections;
using UnityEngine;

namespace DarkNaku.View
{
    public interface IViewHandler {
        string Name { get; }
        Canvas ViewCanvas { get; }
        bool Interactable { get; set; }
        bool IsInTransition { get; }

        void Initialize();
        IEnumerator Show();
        IEnumerator Hide();
    }
}