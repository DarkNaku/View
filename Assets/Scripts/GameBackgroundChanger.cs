using System.Collections;
using System.Collections.Generic;
using DarkNaku.View;
using UnityEngine;
using UnityEngine.UI;

public class GameBackgroundChanger : ViewElement<GameViewHandler> {
    [SerializeField] private Image _background;

    protected override void OnInitialize() {
        Debug.Log($"[GameBackgroundChanger] OnInitialize - {name}");
    }

    protected override void OnRelease() {
        Debug.Log($"[GameBackgroundChanger] OnRelease - {name}");
    }

    protected override void OnEnterBefore() {
        Debug.Log($"[GameBackgroundChanger] OnEnterBefore - {name}");

        _background.color = Random.ColorHSV();
    }

    protected override void OnEnterAfter() {
        Debug.Log($"[GameBackgroundChanger] OnEnterAfter - {name}");
    }

    protected override void OnExitBefore() {
        Debug.Log($"[GameBackgroundChanger] OnExitBefore - {name}");
    }

    protected override void OnExitAfter() {
        Debug.Log($"[GameBackgroundChanger] OnExitAfter - {name}");
    }
}