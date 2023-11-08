using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public AICharacterManager character;
    [HideInInspector] public AINavigator navigator;
    private void Awake() {
        navigator = GetComponentInParent<AINavigator>();
        character = GetComponentInParent<AICharacterManager>();
    }
    public abstract State RunCurrentState();
}
