using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour {
    // This script is used as an instance to keep track in which scnene the player is in.
    public static WorldSaveGameManager instance;
    [SerializeField] int[] worldSceneIndex;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public int[] GetWorldSceneIndex() {
        return worldSceneIndex;
    }
}