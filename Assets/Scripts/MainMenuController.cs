using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance;

    internal UIShowHide ShowingMainMenuUI;

    private PlayerInput _playerInput;

    //Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    private InputAction back;

    //Start is called before the first frame update
    /*void Awake()
    {
        back = _playerInput.currentActionMap.FindAction("Back");
        back.started += CloseCurrentMenu;
    }*/

    private void CloseCurrentMenu(InputAction.CallbackContext context)
    {
        if (ShowingMainMenuUI == null) return;

        ShowingMainMenuUI.ButtonPress();
    }

    /*private void OnDestroy()
    {
        back.started -= Close
    }*/
}
