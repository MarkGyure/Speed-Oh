using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishScript : MonoBehaviour
{
    private TimerController timerController;
    public TMP_Text finishText;
    string finalTime;
    // Start is called before the first frame update
    private void Awake()
    {
        timerController = FindObjectOfType<TimerController>();
        finishText.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        timerController.StopStopwatch();
        finishText.text = finalTime;
        finishText.gameObject.SetActive(true);

    }
    public void getTime(string temp)
    {
        finalTime = temp;
    }
}
