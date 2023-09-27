using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    public Character_Input characterInput;
    private Character_Input.MovementActions movement;
    private Character_Input.CameraLookActions cameraInput;

    public Action<Vector2> horizontalMoveAction;
    public Action<float> sprintAction;
    public Action<float> crouchAction;
    public Action jumpAction;

    private void Awake() {
        characterInput = new Character_Input();
        movement = characterInput.Movement;
        cameraInput = characterInput.CameraLook;
    }

    private void OnEnable() {
        movement.Enable();
        cameraInput.Enable();
    }

    private void OnDisable() {
        movement.Disable();
        cameraInput.Disable();
    }

    private void Start() {
        movement.Jump.started += ctx => jumpAction?.Invoke();
    }

    private void Update() {
        Vector2 horizontal = movement.HorizontalMovement.ReadValue<Vector2>();
        horizontalMoveAction?.Invoke(horizontal);

        float sprinting = movement.Sprint.ReadValue<float>();
        sprintAction?.Invoke(sprinting);

        float crouching = movement.Crouch.ReadValue<float>();
        crouchAction?.Invoke(crouching);
    }

}
