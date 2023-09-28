using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    private InputManager inputManager;
    private Rigidbody rb;
    private Transform groundCheckTransform;
    private Transform characterOrientation;
    private float groundCheckDistance = 0.25f;    
    private float originalSpeed;
    private bool isGrounded;
#region  Movement Settings
    [Header("Movement Settings")]
    [Tooltip("The base speed of your character")]
    [SerializeField] private float moveSpeed;
    [Tooltip("A modifier that multiplies your base Move Speed")]
    [SerializeField] private float walkSpeedModifier;
    [Tooltip("A modifier that will multiply your move speed when you are sprinting")]
    [SerializeField] private float sprintSpeedModifier;
    [Tooltip("A modifier that will multiply your move speed when you are crouching.")]
    [SerializeField] private float crouchSpeedModifier;
#endregion

#region  Jump Settings
    [Header("Jump Settings")]
    [Tooltip("This is the Power for your jump")]
    [SerializeField] private float jumpForce;
    [Tooltip("Include all the layers that you want your character to jump off of")]
    [SerializeField] private LayerMask groundLayer;
#endregion

#region Crouch Settings
    [Header("Crouch Settings")]
    [Tooltip("This variable will set your players scale to be what you set it to when you crouch")]
    [SerializeField] private Vector3 crouchHeight;
    [Tooltip("This variable will set your players scale to be what you set it to when your standing")]
    [SerializeField] private Vector3 standHeight;
#endregion
    
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
        groundCheckTransform = transform.GetChild(0);
        characterOrientation = transform.GetChild(1);
        originalSpeed = moveSpeed;
    }

    private void Update() {
        // Perform the ground check using a raycast.
        isGrounded = Physics.OverlapSphere(groundCheckTransform.position, groundCheckDistance, groundLayer).Length > 0;
    }


    private void HorizontalMovement(Vector2 horizontalInput) {

        Vector3 move = characterOrientation.forward * horizontalInput.y + characterOrientation.right * horizontalInput.x;
        
        move *= moveSpeed;

        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }
    
    private void Sprint(float sprintFloat) {
        //move *= inputManager.characterInput.Movement.Sprint.ReadValue<float>() == 0 ? speed : speed * runSpeedModifier;
        if(sprintFloat != 0 && isGrounded) { // If the sprint button is not being pressed
            moveSpeed = originalSpeed * sprintSpeedModifier;
        } else {
            moveSpeed = originalSpeed * walkSpeedModifier;
        }
    }

    private void Crouch(float crouchFloat) {
        if(crouchFloat != 0 && isGrounded) {
            transform.localScale = crouchHeight;
            moveSpeed = originalSpeed * crouchSpeedModifier;
        } else {
            transform.localScale = standHeight;
            moveSpeed = originalSpeed * walkSpeedModifier;
        }
    }

    private void Jump() {
        if(isGrounded) {
            rb.AddForce(Vector3.up * jumpForce);
        }
    } 

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckDistance);
    // }

}
