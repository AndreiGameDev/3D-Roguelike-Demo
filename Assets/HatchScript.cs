using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HatchScript : MonoBehaviour, IInteract
{
    public void Interact(PlayerCombatManager player = null) {
        SceneManager.LoadScene("FinishedLevelScene");
    }
}
