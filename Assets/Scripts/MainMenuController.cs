using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance;

    internal UIShowHide ShowingMainMenuUI;

    void Start()
    {
        Instance = this;
    }

    private void CloseCurrentMenu()
    {
        if (ShowingMainMenuUI == null) return;

        ShowingMainMenuUI.ButtonPress();
    }
}
