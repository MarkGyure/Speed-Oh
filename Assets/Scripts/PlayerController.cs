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
    [SerializeField] private float _runSpeed;
    [SerializeField] private CharacterController controller;
    public float appliedSpeed;
    private bool canDoubleJump = true;
    private bool canJump = true;
    private bool isMoving;
    private float direction;
    private bool canAccel = true;
    private float currentAccel;
    private bool goingForward;
    private bool goingBackward;
    private bool braking;
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
        /*if (isMoving)
        {
            direction = forwardBackward.ReadValue<float>();
            if (direction < 0)
            {
                if (canAccel && currentAccel < _accelDivision) 
                {
                    if (braking)
                    {
                        currentAccel--;
                        if (currentAccel == 0)
                        {
                            braking = false;
                        }
                        appliedSpeed = -(_runSpeed * currentAccel) / _accelDivision;
                        canAccel = false;
                        StartCoroutine(Accelerate());
                    }
                    else if (goingForward)
                    {
                        currentAccel--;
                        goingForward = false;
                        goingBackward = true;
                        braking = true;
                        appliedSpeed = -(_runSpeed * currentAccel) / _accelDivision;
                        canAccel = false;
                        StartCoroutine(Accelerate());
                    }
                    else
                    {
                        currentAccel++;
                        goingForward = false;
                        goingBackward = true;
                        appliedSpeed = (_runSpeed * currentAccel) / _accelDivision;
                        canAccel = false;
                        StartCoroutine(Accelerate());
                    }
                }
                
                transform.position = Vector3.MoveTowards(transform.position, _backMove.transform.position, appliedSpeed * Time.deltaTime);

                //There are Empty Objects in front of and behind the Player, which rotate when it rotates. The way it moves
                //forward and backward is by moving towwards those Empty Objects. It's a strange way to move it,
                //but the only other way I could think of to change it's move direction with it's rotation was 
                //EulerAngles, which give me a headache at the best of times
            }
            if (direction > 0)
            {
                if (canAccel && currentAccel < _accelDivision) 
                {

                    if (braking)
                    {
                        currentAccel--;
                        if (currentAccel == 0)
                        {
                            braking = false;
                        }
                        appliedSpeed = -(_runSpeed * currentAccel) / _accelDivision;
                        canAccel = false;
                        StartCoroutine(Accelerate());
                    }
                    else if(goingBackward)
                    {
                        currentAccel--;
                        goingForward = true;
                        goingBackward = true;
                        braking = true;  
                        appliedSpeed = -(_runSpeed * currentAccel) / _accelDivision;
                        canAccel = false;
                        StartCoroutine(Accelerate());         
                    }
                    else
                    {
                        currentAccel++;
                        goingForward = true;
                        goingBackward = false;
                        appliedSpeed = (_runSpeed * currentAccel) / _accelDivision;
                        canAccel = false;
                        StartCoroutine(Accelerate());
                    }
                }
                
                transform.position = Vector3.MoveTowards(transform.position, _frontMove.transform.position, appliedSpeed * Time.deltaTime);
            }
        }*/

        if (IsGrounded() && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        controller.Move(move * Time.deltaTime * appliedSpeed);

        /*if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }*/

        // Double Jump
        if (Input.GetButtonDown("Jump") && canDoubleJump && !canJump && !IsGrounded())
        {
            playerVelocity.y += Mathf.Sqrt(_jumpValue * -2.0f * gravityValue);
            canDoubleJump = false;
        }

        // Makes the player jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            playerVelocity.y += Mathf.Sqrt(_jumpValue * -2.0f * gravityValue);
            canJump = false;
        }
        

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


        if (IsGrounded())
        {
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

        return Physics.CheckSphere(_groundCheck.transform.position, 0.1f, _layerMask);
    }

    public Vector2 GetPlayerMovement()
    {
        return movement.ReadValue<Vector2>();
    }

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
        _runSpeed += 3;
        Debug.Log(_runSpeed);
    }

}

