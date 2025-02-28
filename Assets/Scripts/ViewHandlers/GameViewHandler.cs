using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkNaku.View;

public class GameViewHandler : ViewHandler<GameViewHandler> {
    public void OnClickMain() {
        View.Change("MainView");
    }
}
