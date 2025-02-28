using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkNaku.View;

public class EntryPoint : MonoBehaviour {
    private void Start() {
        View.Change("MainView");
    }
}