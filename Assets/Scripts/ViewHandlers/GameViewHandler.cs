using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkNaku.View;

public class GameViewHandler : ViewHandler<GameViewHandler> {
    public void OnClickMain() {
        View.Change("MainView");
    }

    protected override void OnInitialize() {
        Debug.Log($"[ViewHandler] OnInitialize - {name}");
    }

    protected override void OnRelease() {
        Debug.Log($"[ViewHandler] OnRelease - {name}");
    }

    protected override void OnEnterBefore() {
        Debug.Log($"[ViewHandler] OnEnterBefore - {name}");
    }

    protected override void OnEnterAfter() {
        Debug.Log($"[ViewHandler] OnEnterAfter - {name}");
    }

    protected override void OnExitBefore() {
        Debug.Log($"[ViewHandler] OnExitBefore - {name}");
    }

    protected override void OnExitAfter() {
        Debug.Log($"[ViewHandler] OnExitAfter - {name}");
    }
}
