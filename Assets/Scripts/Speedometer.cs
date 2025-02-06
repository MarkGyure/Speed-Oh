/*****************************************************************************
// File Name :         Speedometer.cs
// Author :            Jack Fried
// Creation Date :     February 3, 2025
//
// Brief Description : Allows the speedometer UI element to function and
                       track the player's speed.
*****************************************************************************/

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    //Setting variables
    private const float MAX_SPEED_ANGLE = -270f;
    private const float ZERO_SPEED_ANGLE = 0f;
    private const float MAX_RADIAL_FILL = 0.75f;

    private float maxSpeed;
    private float speed;
    private float displaySpeed;
    private float dialAngle;
    private float currentRadialFill = 0f;
    private float speedNormalized;

    [SerializeField] private PlayerController PlayerScript;
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;
    [SerializeField] private TMP_Text SpeedText;
    [SerializeField] private Transform DialTransform;
    [SerializeField] private Image RadialImage;
    [SerializeField] private float fovMin;
    [SerializeField] private float fovMax;

    /// <summary>
    /// Finds the player script and sets max speed detection on start
    /// </summary>
    void Start()
    {
        //Finding required objects
        PlayerScript = FindAnyObjectByType<PlayerController>();
        VirtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();

        //Setting the max speed threshold and initial display speed
        maxSpeed = 30f;
        displaySpeed = 0f;
    }

    /// <summary>
    /// Updates all active speedometer information
    /// </summary>
    void Update()
    {
        speed = PlayerScript.appliedSpeed; //Getting the player's current speed
        displaySpeed = Mathf.Round(speed * 5f); //The flavor speed for the display

        SpeedText.text = displaySpeed.ToString(); //Sets display text to current, player speed

        //Call angle functions
        SetRadialFill();
        SetSpeedRotation();

        if (speed == 0)
        {
            VirtualCamera.m_Lens.FieldOfView = fovMin;
        }
        else
        {
            if (VirtualCamera.m_Lens.FieldOfView < fovMax)
            {
                VirtualCamera.m_Lens.FieldOfView = fovMin + speed;
            }
            else
            {
                VirtualCamera.m_Lens.FieldOfView = fovMax;
            }    
        }
    }

    /// <summary>
    /// Converts current speed into an angle to be used by the dial
    /// </summary>
    private void SetSpeedRotation()
    {
        float angleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE; //Gets the angle range for the dial

        speedNormalized = speed / maxSpeed; //Speed value is read as being between 0 and 1

        //Applies rotation data
        dialAngle = ZERO_SPEED_ANGLE - speedNormalized * angleSize;
        DialTransform.eulerAngles = new Vector3(0, 0, dialAngle);
    }

    /// <summary>
    /// Uses dial rotation data to adjust the radial fill amount accordingly
    /// </summary>
    private void SetRadialFill()
    {
        currentRadialFill = speedNormalized * MAX_RADIAL_FILL;
        RadialImage.fillAmount = currentRadialFill;
    }
}
