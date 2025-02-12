using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using UnityEngine.InputSystem.UI;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance;

    internal UIShowHide ShowingMainMenuUI;

    public InputSystemUIInputModule Input;

    //Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    private InputAction back;

    //Start is called before the first frame update
    void Awake()
    {
        back = Input.actionsAsset.FindAction("Cancel");
        back.started += CloseCurrentMenu;
    }

    private void CloseCurrentMenu(InputAction.CallbackContext context)
    {
        if (ShowingMainMenuUI == null) return;

        ShowingMainMenuUI.ButtonPress();
    }

    private void OnDestroy()
    {
        back.started -= CloseCurrentMenu;
    }
}
