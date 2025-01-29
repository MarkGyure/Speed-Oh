/*****************************************************************************
// File Name : PlayerController.cs
// Author : Nick Moritz
// Creation Date : March 2, 2024
//
// Brief Description : This is the controller for the Player. It controls their movement, attacks, and jumps 
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float _jumpValue = 5f;
    [SerializeField] private GameObject _groundCheck;
    [SerializeField] private GameObject _frontMove;
    [SerializeField] private GameObject _backMove;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _turnSpeed;
    private float rotation = 0;
    private bool isTurning;
    private bool isMoving;
    private float direction;

    private InputAction turn;
    private InputAction forwardBackward;

    private Rigidbody rb;

    /// <summary>
    /// Turns on the Rigidbody, the Action Map, and the movement listeners
    /// </summary>
    void Start()
    {
        rb = _player.GetComponent<Rigidbody>();
        _playerInput.currentActionMap.Enable();
        turn = _playerInput.currentActionMap.FindAction("Turn");
        forwardBackward = _playerInput.currentActionMap.FindAction("ForwardBackward");

        turn.started += Turn_started;
        turn.canceled += Turn_canceled;
        forwardBackward.started += ForwardBackward_started;
        forwardBackward.canceled += ForwardBackward_canceled;
    }

    /// <summary>
    /// Turns off the event listeners when the player is dead
    /// </summary>
    public void OnDestroy()
    {
        turn.started -= Turn_started;
        turn.canceled -= Turn_canceled;
        forwardBackward.started -= ForwardBackward_started;
        forwardBackward.canceled -= ForwardBackward_canceled;
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
    }

    /// <summary>
    /// Sets the isTurning parameter to false when the player releases the button. I know it's weird to 
    /// have a mix of event listeners and the other input routing method, but I don't know how to do this with the 
    /// newer method, so I had to use event listeners
    /// </summary>
    /// <param name="obj"></param>
    private void Turn_canceled(InputAction.CallbackContext obj)
    {
        isTurning = false;
    }

    /// <summary>
    /// Sets the isTurning paramenter to true when the player presses the button
    /// </summary>
    /// <param name="obj"></param>
    private void Turn_started(InputAction.CallbackContext obj)
    {
        isTurning = true;
    }

    /// <summary>
    /// Sets the isMoving parameter to false when the button is released
    /// </summary>
    /// <param name="obj"></param>
    private void ForwardBackward_canceled(InputAction.CallbackContext obj)
    {
        isMoving = false;
    }

    /// <summary>
    /// Sets the isMoving parameter to true when the button is pressed
    /// </summary>
    /// <param name="obj"></param>
    private void ForwardBackward_started(InputAction.CallbackContext obj)
    {
        isMoving = true;
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
        if (isMoving)
        {
            direction = forwardBackward.ReadValue<float>();
            if (direction < 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, _backMove.transform.position, 1.5f * Time.deltaTime);
                //There are Empty Objects in front of and behind the Player, which rotate when it rotates. The way it moves
                //forward and backward is by moving towwards those Empty Objects. It's a strange way to move it,
                //but the only other way I could think of to change it's move direction with it's rotation was 
                //EulerAngles, which give me a headache at the best of times
            }
            if (direction > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, _frontMove.transform.position, 1.5f * Time.deltaTime);
            }
        }
        rotation = _turnSpeed * turn.ReadValue<float>();
        if (isTurning)
        {
            transform.Rotate(0, 360 * rotation * Time.deltaTime, 0);
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

}

