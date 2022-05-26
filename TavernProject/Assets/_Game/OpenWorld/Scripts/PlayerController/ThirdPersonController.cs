using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviourPunCallbacks
{
    //input fields
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;

    //movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private Animator animator;
    private enum AnimationStates { IDLE, WALKING }
    private AnimationStates playerAnimationState = AnimationStates.IDLE;

    private bool _initialized = false;

    private void Start()
    {
        if(NetworkManager.use_network && !photonView.IsMine)
            return;
            
        rb = this.GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        Subscribe();
    }

    private void OnEnable()
    {
        if(NetworkManager.use_network && !photonView.IsMine)
            return;
        
        if (_initialized) Subscribe();
    }
    private void Subscribe()
    {
        print(GameManager.Instance);
        print(GameManager.Instance.playerInputManager.GetPlayerActionsAsset());
        playerActionsAsset = GameManager.Instance.playerInputManager.GetPlayerActionsAsset();

        playerActionsAsset.Player.Enable();
        playerActionsAsset.Player.Jump.started += DoJump;
        move = playerActionsAsset.Player.Move;

        _initialized = true;
    }
    private void OnDisable()
    {
        playerActionsAsset.Player.Jump.started -= DoJump;
        playerActionsAsset.Player.Disable();
    }

    private void FixedUpdate()
    {
        if (NetworkManager.use_network && !photonView.IsMine)
            return;

        LookAt();

        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        Vector2 actionInput = move.ReadValue<Vector2>();
        if (actionInput != Vector2.zero && playerAnimationState != AnimationStates.WALKING)
        {
            playerAnimationState = AnimationStates.WALKING;
            animator.SetBool("Is_Walking", true);
        }
        if (actionInput == Vector2.zero && playerAnimationState != AnimationStates.IDLE)
        {
            playerAnimationState = AnimationStates.IDLE;
            animator.SetBool("Is_Walking", false);
        }

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if (rb.velocity.y < 0f)
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }

    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
            return true;
        else
            return false;
    }
}
