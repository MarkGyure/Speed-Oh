/*****************************************************************************
// File Name : PlayerController.cs
// Author : Nick Moritz
// Creation Date : January 28, 2025
//
// Brief Description : This is the controller for the Player. It controls their movement
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float _jumpValue = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _accelInterval;
    [SerializeField] private float _accelDivision;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _runSpeedIncrease = 3;
    [SerializeField] private CharacterController controller;
    public float appliedSpeed;
    public float appliedGravityValue;
    private bool canDoubleJump = true;
    private bool canJump = true;
    private bool canAccel = true;
    private float currentAccel;
    private Vector3 playerVelocity;
    private Transform cameraTransform;

    private InputAction movement;
    private InputAction look;

    private Rigidbody rb;

    /// <summary>
    /// Turns on the Rigidbody, the Action Map, and the movement listeners
    /// </summary>
    void Start()
    {
        rb = _player.GetComponent<Rigidbody>();
        _playerInput.currentActionMap.Enable();
        look = _playerInput.currentActionMap.FindAction("Look");
        movement = _playerInput.currentActionMap.FindAction("Movement");
        
        cameraTransform = Camera.main.transform;
        
        currentAccel = 0;

        Cursor.visible = false;
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
    void OnJump() //faster way of routing buttons than Event Listeners
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, _jumpValue, rb.velocity.z);
            //This way, the jump happens without overwriting the existing velocity
        }
        else if(canDoubleJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, _jumpValue, rb.velocity.z);
            canDoubleJump = false;
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
    /// Moves and rotates the player when isMoving and isTurning are true
    /// </summary>
    void Update()
    {
        if (IsGrounded() && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        controller.Move(move * Time.deltaTime * appliedSpeed);


        // Double Jump
        /*if (Input.GetButtonDown("Jump") && canDoubleJump && !canJump && !IsGrounded())
        {
            playerVelocity.y += Mathf.Sqrt(_jumpValue * -2.0f * gravityValue);
            canDoubleJump = false;
        }*/

        // Makes the player jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            appliedGravityValue = gravityValue;
            playerVelocity.y += Mathf.Sqrt(_jumpValue * -2.0f * appliedGravityValue);
            canJump = false;
        }



        playerVelocity.y += appliedGravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (move == Vector3.zero)
        {
            currentAccel = 0;
            appliedSpeed = 0;
        }
        else if (move != Vector3.zero)
        {
            if (canAccel && currentAccel < _accelDivision)
            {
                currentAccel++;       
                canAccel = false;
                StartCoroutine(Accelerate());
            }
            appliedSpeed = (_runSpeed * currentAccel) / _accelDivision;
        }

        if (IsGrounded())
        {
            if(!canJump)
            {
                appliedGravityValue = gravityValue * 2;
            }

            canJump = true;
            canDoubleJump = true;
            
        }
    }

    /// <summary>
    /// Checks if the player is on the ground, then returns a bool value
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {

        return Physics.CheckSphere(_groundCheck.transform.position, 0.3f, _layerMask);
    }

    /// <summary>
    /// Gets PlayerMovement Vector3 composite from the Inputs
    /// </summary>
    /// <returns></returns>
    public Vector2 GetPlayerMovement()
    {
        return movement.ReadValue<Vector2>();
    }

    /// <summary>
    /// Gets MouseDelta Vector2 from the Inputs
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMouseDelta()
    {
        return look.ReadValue<Vector2>();
    }

    /// <summary>
    /// Maintains the Acceleration Interval
    /// </summary>
    /// <returns></returns>
    private IEnumerator Accelerate()
    {
        yield return new WaitForSeconds(_accelInterval);
        canAccel = true;
    }
    public void SpeedUp()
    {
        _runSpeed += _runSpeedIncrease;
        Debug.Log(_runSpeed);
    }

}

