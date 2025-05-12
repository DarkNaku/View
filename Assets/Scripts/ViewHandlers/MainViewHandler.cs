using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DarkNaku.View;

public class MainViewHandler : ViewHandler<MainViewHandler> {
    public void OnClickGame() {
        View.Change("GameView");
    }

    public void OnClickSceneA() {
        SceneManager.LoadScene("SceneA");
    }

    public void OnClickSceneB() {
        SceneManager.LoadScene("SceneB");
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