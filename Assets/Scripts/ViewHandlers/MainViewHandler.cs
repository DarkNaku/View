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
}