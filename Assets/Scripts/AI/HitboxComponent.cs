using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxComponent : MonoBehaviour
{
    public IDamagable parentIDamagable;
    public bodyParts bodyPart;
    [HideInInspector] public string bodyPartString;

    private void Awake() {
        parentIDamagable = GetComponentInParent<IDamagable>();
        AssignStringName(bodyPart);
    }

    void AssignStringName(bodyParts BodyPart) {
        switch (BodyPart) {
            case bodyParts.Head:
                bodyPartString = "Head";
                break;
            case bodyParts.Body:
                bodyPartString = "Body";
                break;
            default:
                bodyPartString = "Body";
                break;
        }
    }

}
public enum bodyParts {
    Head,
    Body
}
