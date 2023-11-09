using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceivers : MonoBehaviour
{
    // This script is only used to reference the weaponfire function from the gunner script so it can be used in a keyevent
    [SerializeField] Gunner ownerScripts;

    void Start()
    {
        ownerScripts = GetComponentInParent<Gunner>();
    }

    void Fire() {
        ownerScripts.WeaponFire();
    }
}
