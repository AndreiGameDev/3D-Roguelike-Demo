using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public bool isInWave;
    public int enemiesInWaveLeft;
    private static GameManager _instance;
    public float difficultyAmplify = 2f; // 2f is easy, 1f is hard
    public float Score;
    public static GameManager Instance {
        get {
            return _instance;
        }
    }
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}
