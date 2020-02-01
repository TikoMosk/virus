using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public float gameSpeed;
    public Tube tube;
    public static GameController Instance {
        get {
            return _instance;
        }
    }
    private void Awake() {
        if (_instance != null) {
            Destroy(this);
        }
        else {
            _instance = this;
        }
    }
}
