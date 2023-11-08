using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUpAnimation : MonoBehaviour
{
    public Camera cam;
    public AnimationCurve opacityCurve;
    public AnimationCurve normalScaleCurve;
    public AnimationCurve critScaleCurve;
    public AnimationCurve heightCurve;
    private TextMeshProUGUI tmp;
    private float time = 0 ;
    public bool hasCrit;
    public Vector3 origin;

    private void Awake() {
        cam = Camera.main;
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
    }

    private void Update() {
        transform.forward = cam.transform.forward;
        tmp.color = new Color(1,1,1, opacityCurve.Evaluate(time));
        scaleText();
        transform.position = origin + new Vector3(0, 1 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }

     void scaleText() {
        if(hasCrit) {
            transform.localScale = Vector3.one * critScaleCurve.Evaluate(time);
        }
        else if(!hasCrit) {
            transform.localScale = Vector3.one * normalScaleCurve.Evaluate(time);
        }
    }
}
