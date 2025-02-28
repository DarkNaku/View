using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkNaku.View;

public class ViewTransition : MonoBehaviour, IViewTransition {
    [SerializeField] private CanvasGroup _canvasGroup;

    public IEnumerator CoTransitionIn() {
        _canvasGroup.alpha = 0;

        while (_canvasGroup.alpha < 1) {
            _canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator CoTransitionOut() {
        _canvasGroup.alpha = 1;

        while (_canvasGroup.alpha > 0) {
            _canvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }
    }
}