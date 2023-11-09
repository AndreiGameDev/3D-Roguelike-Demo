using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnNewTerrain : MonoBehaviour {
    [SerializeField] NavMeshSurface navMeshSurface;
    public GameObject terrainHolder;
    // Arena prefab iterations and presets for scenarios such as the beggining generation
    [SerializeField] GameObject[] arenaPrefabIterations;
    [SerializeField] GameObject arena1PresetBeggining, arena2PresetBeggining;
    [SerializeField] GameObject[] arenaPresetPreventLoop;
    [SerializeField] GameObject deadEndArena; // This arena won't provide any spawnpoint on generation
    [SerializeField] LayerMask terrainLayers; // Used to check if there is a new layer
    [SerializeField] int maxArenasToGenerate = 5;
    // Gameobject holders to view in the inspector
    [SerializeField] GameObject[] arena; // Holds every arena instantiated
    [SerializeField] List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] List<GameObject> surplusArenas = new List<GameObject>();
    // Variables for the forloop
    bool activatePreventLoop; 
    bool preventLoop;
    int preventLoopCount;
    int currentWaveTier;

    private void Awake() {
        arena = new GameObject[maxArenasToGenerate];
        int lastArenaPlacedIndex;
        WaveManager waveManager = null;

        // This loops until it generated as many arenas as the variable maxArenasToGenerate is set to
        for (int arenasPlaced = 0; arenasPlaced < maxArenasToGenerate;) {

            // If it's the first arena place then do the ArenaPresetBeggining array
            if (arenasPlaced == 0) {
                arena[arenasPlaced] = Instantiate(arena1PresetBeggining, Vector3.zero, Quaternion.identity, terrainHolder.transform);
                // Sets wave tier
                waveManager = arena[arenasPlaced].GetComponentInChildren<WaveManager>();
                waveManager.waveTier = (waveTiers)currentWaveTier;

                // Grabs and adds spawnpoints
                CheckSlotValidity(arena[arenasPlaced].transform);
                lastArenaPlacedIndex = arenasPlaced;
                currentWaveTier++;

                // Instantiates arenas at spawnpoints and sets wave tier
                foreach (GameObject spawnpoint in spawnPoints) {
                    arenasPlaced++;
                    arena[arenasPlaced] = Instantiate(arena2PresetBeggining, spawnpoint.transform.position, spawnpoint.transform.rotation, terrainHolder.transform);
                    waveManager = arena[arenasPlaced].GetComponentInChildren<WaveManager>();
                    waveManager.waveTier = (waveTiers)currentWaveTier;
                }

                currentWaveTier++;
                spawnPoints.Clear();

                // Checks for new spawnpoints since the last arena checked 
                for (int i = arenasPlaced; i > lastArenaPlacedIndex; i--) {
                    if (arena[i] != null) {
                        CheckSlotValidity(arena[i].transform);
                    } else if (arena[i] == null) {
                        break;
                    }
                } 

            } else {
                //Checks for spawnpoints to be above 0 so it doesn't become an infinite loop
                if (spawnPoints.Count > 0) {
                    //If the count is lower than 0 then the algorithm isn't active
                    if (preventLoopCount <= 0) {
                        preventLoop = false;
                    }
                    if (preventLoop == true) {
                        preventLoopCount--;
                    }
                    lastArenaPlacedIndex = arenasPlaced;


                    if (arenasPlaced + spawnPoints.Count < maxArenasToGenerate) {
                        foreach (GameObject spawnpoint in spawnPoints) {
                            arenasPlaced++;
                            GameObject arenaIterationSpawn = arenaPrefabIterations[Random.Range(0, arenaPrefabIterations.Length)];

                            // If preventing loop, hardset what arenas spawn
                            if (preventLoop == true) {
                                if (preventLoopCount == 2) {
                                    arenaIterationSpawn = arenaPresetPreventLoop[0];
                                } else if (preventLoopCount == 1) {
                                    arenaIterationSpawn = arenaPresetPreventLoop[1];
                                }
                            }

                            arena[arenasPlaced] = Instantiate(arenaIterationSpawn, spawnpoint.transform.position, spawnpoint.transform.rotation, terrainHolder.transform);

                            waveManager = arena[arenasPlaced].GetComponentInChildren<WaveManager>();
                            waveManager.waveTier = (waveTiers)currentWaveTier;

                            // Check for infinite dead end loop
                            if (arenaIterationSpawn == arenaPrefabIterations[0] || arenaIterationSpawn == arenaPrefabIterations[1]) {
                                activatePreventLoop = true;
                            }
                        }

                        if (activatePreventLoop) {
                            preventLoop = true;
                            preventLoopCount = 3;
                            activatePreventLoop = false;
                        }
                        spawnPoints.Clear();

                        // Checks for new spawnpoints since the last arena checked 
                        if (arenasPlaced <= maxArenasToGenerate) {
                            for (int i = arenasPlaced; i > lastArenaPlacedIndex; i--) {
                                if (arena[i] != null) {
                                    CheckSlotValidity(arena[i].transform);
                                } else if (arena[i] == null) {
                                    break;
                                }
                            }
                        }

                        //Increments wave tier each loop, the max tier is 10
                        currentWaveTier++;
                        if (currentWaveTier >= 10) {
                            currentWaveTier = 10;
                        }

                    } else {
                        // When there is more spawnpoints than arenas you can place, this whill get called to stop the loop and finish the layout with Dead End arena prefabs.
                        arenasPlaced = maxArenasToGenerate;
                        foreach (GameObject spawnpoint in spawnPoints) {
                            surplusArenas.Add(Instantiate(deadEndArena, spawnpoint.transform.position, spawnpoint.transform.rotation, terrainHolder.transform));
                        }
                        spawnPoints.Clear();
                    }
                } else {
                    // There is no more spawnpoints meaning it hit a dead end so force that 
                    arenasPlaced = maxArenasToGenerate;
                    break;
                }
            }
        }

        // From this till the end, it will check for any spawnpoints that are empty in order to close off the map with dead end arenas, a safety net to finish the generation smoothly.
        for (int i = maxArenasToGenerate - 1; i > 0; i--) {
            if (arena[i] != null) {
                CheckSlotValidity(arena[i].transform);
            } else if (arena[i] == null) {
                break;
            }
        }

        if (spawnPoints.Count > 0) {
            foreach (GameObject spawnpoint in spawnPoints) {
                surplusArenas.Add(Instantiate(deadEndArena, spawnpoint.transform.position, spawnpoint.transform.rotation, terrainHolder.transform));
            }
        }

        foreach (GameObject surplusArena in surplusArenas) {
            waveManager = surplusArena.GetComponentInChildren<WaveManager>();
            waveManager.waveTier = (waveTiers)currentWaveTier;
        }
    }

    private void Start() {
        //Builds ai navigation mesh
        navMeshSurface.BuildNavMesh();
    }


    //Check if the slot the arena would spawn at is already taken or not.
    void CheckSlotValidity(Transform spawnpointSlot) {
        Transform temp = spawnpointSlot.Find("Spawnpoints");

        if (temp != null) {
            for (int i = 0; i < temp.childCount; i++) {
                //Position the check will happen at
                Vector3 localPos = new Vector3(25, 0, 25);
                Vector3 worldPos = temp.GetChild(i).transform.TransformPoint(localPos);
                Collider[] overlappedObject = Physics.OverlapSphere(worldPos, 3f, terrainLayers);
                if (overlappedObject.Length == 0) {
                    spawnPoints.Add(temp.GetChild(i).gameObject);
                } else {
                    Debug.Log("Spawnpoint: " + temp.GetChild(i) + " from: " + spawnpointSlot + " Taken.");
                    // Spot Taken
                }
            }
        }
    }
}


