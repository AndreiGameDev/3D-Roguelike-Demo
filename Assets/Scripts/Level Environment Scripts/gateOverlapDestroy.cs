    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateOverlapDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Gate")) {
            Destroy(gameObject);
        }
    }
}
