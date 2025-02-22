﻿/*****************************************************************************
// File Name : PlayerController.cs
// Author : Nick Moritz
// Creation Date : January 28, 2025
//
// Brief Description : This is the controller for the Player. It controls their movement
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float _jumpValue = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float _playerSpeed = 5f;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _accelInterval;
    [SerializeField] private float _accelDivision;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _runSpeedIncrease = 3;
    [SerializeField] private CharacterController controller;
    public float appliedSpeed;
    //public float appliedGravityValue;
    private bool canDoubleJump = true;
    private bool canJump = true;
    private bool canAccel = true;
    private float currentAccel;
    private Vector3 playerVelocity;
    private Transform cameraTransform;

    private InputAction movement;
    private InputAction look;
    private InputAction brake;

    public Rigidbody rb;
    public TrajectorRenderer tr;
    private Vector3 lastPosition;
    private Vector3 actualVelocity;
    [SerializeField] private Vector3 playerMovment;
    [SerializeField] private Vector3 prevPlayerMovement;

    private Vector2 prevInputMovement;
    [SerializeField] private float currentPlayerSpeed;
    public float MaxPlayerSpeed;
    [SerializeField] private float speedIncrease;

    [SerializeField] private Image pauseScreen;
    private PauseMenu pauseScript;
    /// <summary>
    /// Turns on the Rigidbody, the Action Map, and the movement listeners
    /// </summary>
    void Start()
    {
        //Getting the PauseMenu script from the current screen's UI (PauseScreen element)
        pauseScript = pauseScreen.GetComponent<PauseMenu>();

        rb = _player.GetComponent<Rigidbody>();
        _playerInput.currentActionMap.Enable();
        look = _playerInput.currentActionMap.FindAction("Look");
        movement = _playerInput.currentActionMap.FindAction("Movement");
        brake = _playerInput.currentActionMap.FindAction("Brake");
        
        cameraTransform = Camera.main.transform;
        
        currentAccel = 0;

        Cursor.visible = false;

        lastPosition = transform.position;

    }


    /// <summary>
    /// Turns off the event listeners when the player is dead
    /// </summary>
    public void OnDestroy()
    {
        
    }
    /// <summary>
    /// Makes the player jump when the player hits spacebar and is on the ground
    /// </summary>
    void OnJump()
    {
        if (IsGrounded())
        {
           
            rb.velocity = new Vector3(rb.velocity.x, _jumpValue, rb.velocity.z);
            actualVelocity = rb.velocity; // Store velocity at takeoff
            canDoubleJump = true;

            tr.DrawTrajectory(actualVelocity); // Draw trajectory
        }
        else if (canDoubleJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, _jumpValue, rb.velocity.z);
            actualVelocity = rb.velocity;
            canDoubleJump = false;
           
            tr.DrawTrajectory(actualVelocity);
        }
    }



    /// <summary>
    /// Restarts the scene whene the player hits the R button
    /// </summary>
    private void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Quits the game when the player hits the Q button
    /// </summary>
    private void OnQuit()
    {
        print("Quit Performed");
        UnityEngine.Application.Quit();
    }

    /// <summary>
    /// player movments
    /// </summary>
    private void OnMove()
    {
        Vector2 inputMovement = movement.ReadValue<Vector2>();
        playerMovment.x = inputMovement.x * currentPlayerSpeed;
        playerMovment.z = MathF.Abs(inputMovement.y * currentPlayerSpeed);

        //Previous inputs used before they were released
        prevPlayerMovement.x = prevInputMovement.x * currentPlayerSpeed;
        prevPlayerMovement.z = MathF.Abs(prevInputMovement.y * currentPlayerSpeed);
        //assinging y input to the z axis 

        //Controlling gradual speed increase and decrease
        if (pauseScript.isPause == false)
        {
            if (inputMovement == Vector2.zero) //Slow down over time
            {
                if (currentPlayerSpeed > 0)
                {
                    currentPlayerSpeed -= speedIncrease;
                }
                else
                {
                    currentPlayerSpeed = 0;
                }

                //Player movement only takes the facing camera direction and current speed for motion
                playerMovment = cameraTransform.forward * prevPlayerMovement.z + cameraTransform.right * prevPlayerMovement.x;
            }
            else
            {
                if (prevPlayerMovement.z < 0) //Braking functionality
                {
                    currentPlayerSpeed -= speedIncrease * 2;
                }
                else //Move normally, picking up speed as you go
                {
                    if (currentPlayerSpeed < MaxPlayerSpeed)
                    {
                        currentPlayerSpeed += speedIncrease;
                    }
                    else
                    {
                        currentPlayerSpeed = MaxPlayerSpeed;
                    }
                }

                if (playerMovment.z < 0)
                {
                    currentPlayerSpeed -= speedIncrease * 6;
                    playerMovment = cameraTransform.forward * -playerMovment.z + cameraTransform.right * playerMovment.x;
                }
                else
                {
                    //Player movement uses both camera direction and player input for motion
                    playerMovment = cameraTransform.forward * playerMovment.z + cameraTransform.right * playerMovment.x;
                }

                prevInputMovement = movement.ReadValue<Vector2>();
            }
        }
    }

    private void OnBrake()
    {
        float isBraking = brake.ReadValue<float>();

        if (isBraking > 0)
        {
            if (currentPlayerSpeed > 0)
            {
                currentPlayerSpeed -= speedIncrease * 6;
            }
        }
    }

    /// <summary>
    /// Moves and rotates the player when isMoving and isTurning are true
    /// </summary>
    void Update()
    {
        OnMove();
        OnBrake();
        //playerMovment = cameraTransform.forward * playerMovment.z + cameraTransform.right * playerMovment.x;
        //player turns the direction of the camera 
        playerMovment.y = 0f;
        


        // Apply Gravity
        rb.velocity += Vector3.up * gravityValue * Time.deltaTime;

        actualVelocity = rb.velocity; // Track movement velocity for trajectory

        if (!IsGrounded())
        {
            tr.DrawTrajectory(actualVelocity); // Continuously update trajectory
            
        }
        else
        {
            tr.ClearTrajectory(); // Clear the trajectory when grounded
        }

        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Apply movement to Rigidbody
        rb.velocity = new Vector3(playerMovment.x, rb.velocity.y, playerMovment.z);
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f; // Move ray start above the feet

        bool grounded = Physics.Raycast(rayStart, Vector3.down, out hit, 1.35f, LayerMask.GetMask("Ground"));

        return grounded;
    }


    public Vector2 GetMouseDelta()
    {
       return look.ReadValue<Vector2>();
    }
}

