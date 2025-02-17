/*****************************************************************************
// File Name :         PauseMenu.cs
// Author :            Jack Fried
// Creation Date :     February 3, 2025
//
// Brief Description : Controls the pause screen during gameplay.
*****************************************************************************/

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    //Setting variables
    [SerializeField] private PlayerInput PlayerInputInstance;
    private bool isPause = false;

    [SerializeField] private Image PauseBG;
    [SerializeField] private GameObject ButtonGroup;


    /// <summary>
    /// Automatically disable the pause menu on start
    /// </summary>
    void Start()
    {
        PauseBG.enabled = false;
        ButtonGroup.SetActive(false);
    }


    /// <summary>
    /// Reads the "Pause" input and stops the game's processing until unpaused
    /// </summary>
    void OnPause()
    {
        PauseActivate();
    }

    /// <summary>
    /// Resume the game when the pause menu "Resume" button is pressed
    /// </summary>
    public void Resume()
    {
        PauseActivate();
    }

    /// <summary>
    /// Reloads the current level when the pause menu "Restart" button is pressed
    /// </summary>
    public void Restart()
    {
        PauseActivate(); //Unpauses the game for the reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Gets the name of the currently active scene
    }

    /// <summary>
    /// Return to the main menu when the pause menu "Main Menu" button is pressed
    /// </summary>
    public void MainMenu()
    {
        PauseActivate(); //Unpauses the game before returning
        SceneManager.LoadScene("SamUI");
    }

    /// <summary>
    /// Pause function to be easily reused in multiple places
    /// </summary>
    private void PauseActivate()
    {
        if (isPause == false)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            PauseBG.enabled = true;
            ButtonGroup.SetActive(true);
            isPause = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            PauseBG.enabled = false;
            ButtonGroup.SetActive(false);
            isPause = false;
        }
    }
}
