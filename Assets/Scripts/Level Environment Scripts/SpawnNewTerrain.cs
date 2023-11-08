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
    public GameObject[] arena; // Holds every arena instantiated
    public List<GameObject> spawnPoints = new List<GameObject>();
    public List<GameObject> surplusArenas = new List<GameObject>(); // Holds surplus arenas - left overs after the max arenas got spawned so they get a dead end and aren't left in the open
    // Variables for the forloop
    bool activatePreventLoop; // Prevents arenas from going to a deadend
    bool preventLoop; // Bool for prevent loop
    int preventLoopCount; // Integer for prevent loop count
    int currentWaveTier;
    private void Awake() {
        arena = new GameObject[maxArenasToGenerate];
        int lastArenaPlacedIndex;
        WaveManager waveManager = null;

        for (int arenasPlaced = 0; arenasPlaced < maxArenasToGenerate;) {
            // If it's the first arena place then do the ArenaPresetBeggining array
            if (arenasPlaced == 0) {
                arena[arenasPlaced] = Instantiate(arena1PresetBeggining, Vector3.zero, Quaternion.identity, terrainHolder.transform);
                // Sets wave tier
                waveManager = arena[arenasPlaced].GetComponentInChildren<WaveManager>();
                waveManager.waveTier = (waveTiers)currentWaveTier;
                // Grabs and adds spawnpoints
                CheckSlotValidity(arena[arenasPlaced].transform);
                // Sets last arena placed index for incoming for loop
                lastArenaPlacedIndex = arenasPlaced;
                currentWaveTier++;
                foreach (GameObject spawnpoint in spawnPoints) {
                    arenasPlaced++;
                    arena[arenasPlaced] = Instantiate(arena2PresetBeggining, spawnpoint.transform.position, spawnpoint.transform.rotation, terrainHolder.transform);
                    //Sets wave tier
                    waveManager = arena[arenasPlaced].GetComponentInChildren<WaveManager>();
                    waveManager.waveTier = (waveTiers)currentWaveTier;
                }
                currentWaveTier++;
                spawnPoints.Clear();
                // Checks for spawnpoints
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
                    //When the prevent loop mechanism is active decrease count representing how many times to place the set buildings.
                    if (preventLoopCount <= 0) {
                        preventLoop = false;
                    } else if (preventLoop == true) {
                        preventLoopCount--;
                    }
                    // Sets last arena placed index
                    lastArenaPlacedIndex = arenasPlaced;
                    if (arenasPlaced + spawnPoints.Count < maxArenasToGenerate) {
                        foreach (GameObject spawnpoint in spawnPoints) {
                            arenasPlaced++;
                            // Here is where it gets randomly selected or hard selected to make sure it doesn't run into a loop
                            GameObject arenaIterationSpawn = arenaPrefabIterations[Random.Range(0, arenaPrefabIterations.Length)];
                            // If preventing loop, hardset what arenas spawn
                            if (preventLoop == true) {
                                if (preventLoopCount == 2) {
                                    arenaIterationSpawn = arenaPresetPreventLoop[0];
                                } else if (preventLoopCount == 1) {
                                    arenaIterationSpawn = arenaPresetPreventLoop[1];
                                }
                            }
                            // Instantaites object at spawnpoint]
                            arena[arenasPlaced] = Instantiate(arenaIterationSpawn, spawnpoint.transform.position, spawnpoint.transform.rotation, terrainHolder.transform);

                            //Sets wave tier for each map
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
                        // Loops through the arena we placed in order to check for new spawnpoints
                        if (arenasPlaced <= maxArenasToGenerate) {
                            for (int i = arenasPlaced; i > lastArenaPlacedIndex; i--) {
                                if (arena[i] != null) {
                                    CheckSlotValidity(arena[i].transform);
                                } else if (arena[i] == null) {
                                    break;
                                }
                            }
                        }

                        //Increments wave tier each loop
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
                    // Sets arena placed to maximum because otherwise it infinite loops and freezes
                    arenasPlaced = maxArenasToGenerate;
                    break;
                }
            }
        }

        //  Checks for any other spawnpoints just in case and grabs them
        for (int i = maxArenasToGenerate - 1; i > 0; i--) {
            if (arena[i] != null) {
                CheckSlotValidity(arena[i].transform);
            } else if (arena[i] == null) {
                break;
            }
        }
        // Adds the last dead end arenas if there is any spawnpoint found
        if (spawnPoints.Count > 0) {
            foreach (GameObject spawnpoint in spawnPoints) {
                surplusArenas.Add(Instantiate(deadEndArena, spawnpoint.transform.position, spawnpoint.transform.rotation, terrainHolder.transform));
            }
        }
        // Sets the wave tier to every surplus arena
        foreach (GameObject surplusArena in surplusArenas) {
            waveManager = surplusArena.GetComponentInChildren<WaveManager>();
            waveManager.waveTier = (waveTiers)currentWaveTier;
        }
    }

    private void Start() {
        //Builds nav mesh
        navMeshSurface.BuildNavMesh();
    }


    //Check if the slot the arena should spawn at is available or not
    void CheckSlotValidity(Transform spawnpointSlot) {
        // Looks for a the gameobject Spawnpoints
        Transform temp = spawnpointSlot.Find("Spawnpoints");
        // if temp is null that means it's a dead end so do nothing otherwise loop through each child and check if there is an arena placed if there isn't then add to spawnpoints.
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


