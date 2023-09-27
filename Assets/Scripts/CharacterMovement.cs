using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    private InputManager inputManager;
    private Rigidbody rb;
    private float originalSpeed;
    private bool isGrounded;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeedModifier;
    [SerializeField] private float runSpeedModifier;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;    
    
    private void OnEnable() {
        inputManager.horizontalMoveAction += HorizontalMovement;
        inputManager.sprintAction += Sprint;
        inputManager.crouchAction += Crouch;
        inputManager.jumpAction += Jump;
    }

    private void OnDisable() {
        inputManager.horizontalMoveAction -= HorizontalMovement;
        inputManager.sprintAction -= Sprint;
        inputManager.crouchAction -= Crouch;
        inputManager.jumpAction -= Jump;
    }

    private void Awake() {
        TryGetComponent<InputManager>(out inputManager);
        TryGetComponent<Rigidbody>(out rb);
        originalSpeed = moveSpeed;
    }

    void Update() {
        // Perform the ground check using a raycast.
        isGrounded = Physics.Raycast(groundCheckTransform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    private void HorizontalMovement(Vector2 horizontalInput) {

        Vector3 move = transform.forward * horizontalInput.y + transform.right * horizontalInput.x;
        
        move *= moveSpeed;

        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }
    
    private void Sprint(float sprintFloat) {
        //move *= inputManager.characterInput.Movement.Sprint.ReadValue<float>() == 0 ? speed : speed * runSpeedModifier;
        if(sprintFloat == 0) { // If the sprint button is not being pressed
            moveSpeed = originalSpeed * walkSpeedModifier;
        } else {
            moveSpeed = originalSpeed * runSpeedModifier;
        }
    }

    private void Crouch(float crouchFloat) {
        if(crouchFloat != 0) {
            transform.localScale = new Vector3(1f, 0.5f, 1f);
        } else {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void Jump() {
        if(isGrounded) {
            rb.AddForce(Vector3.up * jumpForce);
        }
    } 
}
