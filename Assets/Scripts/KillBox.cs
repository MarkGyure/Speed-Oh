/*****************************************************************************
// File Name :         KillBox.cs
// Author :            Jack Fried
// Creation Date :     February 6, 2025
//
// Brief Description : Controls a specified collision area to cause the
                       player to respawn.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillBox : MonoBehaviour
{
    /// <summary>
    /// Causes the restart
    /// </summary>
    /// <param name="collision"> What the collision is with </param>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Gets the name of the currently active scene
        }
    }
}
