using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum waveTiers {
    Tier1,
    Tier2,
    Tier3,
    Tier4,
    Tier5,
    Tier6,
    Tier7,
    Tier8,
    Tier9,
    Tier10
}
public class WaveManager : MonoBehaviour {
    bool isWaveCleared;
    public waveTiers waveTier;
    [HideInInspector]
    public int enemiesAmount {
        get {
            switch (waveTier) {
                case waveTiers.Tier1:
                    return 10;
                case waveTiers.Tier2:
                    return 10;
                case waveTiers.Tier3:
                    return 15;
                case waveTiers.Tier4:
                    return 15;
                case waveTiers.Tier5:
                    return 20;
                case waveTiers.Tier6:
                    return 20;
                case waveTiers.Tier7:
                    return 20;
                case waveTiers.Tier8:
                    return 25;
                case waveTiers.Tier9:
                    return 35;
                case waveTiers.Tier10:
                    return 45;
                default:
                    return 0;
            }
        }
    }
    [HideInInspector]
    public float enemyStrengthAmplifier {
        get {
            switch (waveTier) {
                case waveTiers.Tier1:
                    return 1;
                case waveTiers.Tier2:
                    return 1;
                case waveTiers.Tier3:
                    return 1;
                case waveTiers.Tier4:
                    return 1.2f;
                case waveTiers.Tier5:
                    return 1.2f;
                case waveTiers.Tier6:
                    return 1.5f;
                case waveTiers.Tier7:
                    return 1.5f;
                case waveTiers.Tier8:
                    return 2f;
                case waveTiers.Tier9:
                    return 2f;
                case waveTiers.Tier10:
                    return 2f;
                default:
                    return 0;
            }
        }
    }
    GameManager gameManager;
    [SerializeField] List<Transform> spawnpoints;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int enemiesToKill;
    [SerializeField] Transform enemyHolder;
    InteractChest chest;
    private void Start() {
        gameManager = GameManager.Instance;
        enemiesToKill = enemiesAmount;
        chest = GameObject.FindWithTag("Chest").GetComponent<InteractChest>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Player")) {
            if (!isWaveCleared) {
                gameManager.isInWave = true;
                StartCoroutine(SpawnEnemy());
                StartCoroutine(CheckIfWaveCleared());
            }
        }
    }

    IEnumerator CheckIfWaveCleared() {
        yield return new WaitForSeconds(1f);
        if (enemyHolder.childCount == 0 && enemiesToKill <= 0) {
            isWaveCleared = true;
            gameManager.isInWave = false;
            chest.canUse = true;
        }
        if (!isWaveCleared) {
            StartCoroutine(CheckIfWaveCleared());
        }
    }

    IEnumerator SpawnEnemy() {
        float maxSpawnRateTime;
        if (enemyHolder.childCount >= 3) {
            maxSpawnRateTime = 1.5f;
        } else {
            maxSpawnRateTime = 3f;
        }
        yield return new WaitForSecondsRealtime(Random.Range(0, maxSpawnRateTime));
        Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Count)];
        Instantiate(enemies[Random.Range(0, enemies.Length)], spawnpoint.position, spawnpoint.rotation);
        enemiesToKill--;
        if (enemiesToKill >= 0) {
            StartCoroutine(SpawnEnemy());
        }
    }
}
