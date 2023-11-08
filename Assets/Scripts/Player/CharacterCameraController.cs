using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass {

}
public class CharacterCameraController : MonoBehaviour {
    PlayerInputManager inputManager;
    [SerializeField] CharacterManager characterManager;

    [SerializeField] Camera headCamera;
    public float xSensitivity;
    public float ySensitivity;
    float xRotation = 0f;

    private void Start() {
        inputManager = PlayerInputManager.Instance;
        characterManager = GetComponent<CharacterManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void HandleCameraMovement() {
        if (characterManager.canLook) {
            float mouseX = inputManager.GetMouseDelta().x;
            float mouseY = inputManager.GetMouseDelta().y;
            xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            headCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.root.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }
    }
    private void LateUpdate() {
        HandleCameraMovement();
    }

}