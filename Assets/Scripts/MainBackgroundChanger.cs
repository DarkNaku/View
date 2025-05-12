using System.Collections;
using System.Collections.Generic;
using DarkNaku.View;
using UnityEngine;
using UnityEngine.UI;

public class MainBackgroundChanger : ViewElement<MainViewHandler> {
    [SerializeField] private Image _background;

    protected override void OnInitialize() {
        Debug.Log($"[MainBackgroundChanger] OnInitialize - {name}");
    }

    protected override void OnRelease() {
        Debug.Log($"[MainBackgroundChanger] OnRelease - {name}");
    }

    protected override void OnEnterBefore() {
        Debug.Log($"[MainBackgroundChanger] OnEnterBefore - {name}");

        _background.color = Random.ColorHSV();
    }

    protected override void OnEnterAfter() {
        Debug.Log($"[MainBackgroundChanger] OnEnterAfter - {name}");
    }

    protected override void OnExitBefore() {
        Debug.Log($"[MainBackgroundChanger] OnExitBefore - {name}");
    }

    protected override void OnExitAfter() {
        Debug.Log($"[MainBackgroundChanger] OnExitAfter - {name}");
    }
}