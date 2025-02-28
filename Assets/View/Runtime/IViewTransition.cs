using System.Collections;
using UnityEngine;

namespace DarkNaku.View {
    public interface IViewTransition {
        IEnumerator CoTransitionIn();
        IEnumerator CoTransitionOut();
    }
}