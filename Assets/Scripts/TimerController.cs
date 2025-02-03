using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    bool stopwatchActive = false;
    float currentTime=0;
    [SerializeField] TMP_Text timetext;
    private FinishScript finishScript;

    private void Start()
    {
        finishScript = FindObjectOfType<FinishScript>();
        StartStopwatch();
    }

    // Update is called once per frame
    void Update()
    {
        if(stopwatchActive==true)
        {
            currentTime = currentTime + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timetext.text = time.ToString(@"mm\:ss\:fff");
    }
    public void StartStopwatch()
    {
        stopwatchActive = true;
        
    }
    public void StopStopwatch()
    {
        stopwatchActive = false;
        finishScript.getTime(timetext.text);
    }
   
}
