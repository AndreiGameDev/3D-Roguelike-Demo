using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    Vector3 currentRotation, targetRotation;
    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float recoilZ;
    public float snapiness, returnAmount;
    [SerializeField] Transform cam;
    
    // Launches camera up when the recoil function is being called, then slowly goes back down
    private void Update() {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * snapiness);
        cam.transform.localRotation = Quaternion.Euler(currentRotation);
    }

    // Launches the rotation upwards
    public void RecoilLogic() {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ ));
    }
}
