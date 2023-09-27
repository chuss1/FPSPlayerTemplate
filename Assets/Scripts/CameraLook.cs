using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour {
    private InputManager inputManager;
    private Transform characterBody;
    [SerializeField] private float mouseSensitivity;

    private float xRotation = 0;

    private void Awake() {
        inputManager = GetComponentInParent<InputManager>();
        characterBody = transform.parent;
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        Vector2 mouseXYDelta = inputManager.characterInput.CameraLook.Look.ReadValue<Vector2>()
         * mouseSensitivity * Time.deltaTime;

         xRotation -= mouseXYDelta.y;
         xRotation = Mathf.Clamp(xRotation, -90f, 90f);

         transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
         characterBody.Rotate(Vector3.up * mouseXYDelta.x);

         //Debug.Log(mouseX + " " + mouseY);
         //Debug.Log(mouseXYDelta);
    }
}
