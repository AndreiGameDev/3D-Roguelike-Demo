using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceivers : MonoBehaviour
{
    [SerializeField] Gunner ownerScripts;

    void Start()
    {
        ownerScripts = GetComponentInParent<Gunner>();
    }

    void Fire() {
        ownerScripts.WeaponFire();
    }
}
