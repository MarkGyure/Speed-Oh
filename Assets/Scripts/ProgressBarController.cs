using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBarController : MonoBehaviour
{
    private float maximumDistance;
    private float currentDistance;
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject finish;
    [SerializeField] private GameObject player;
    [SerializeField] private Image mask;
    // Start is called before the first frame update
    void Start()
    {
        maximumDistance = finish.transform.position.z - start.transform.position.z;
        
    }

    // Update is called once per frame
    void Update()
    {
        currentDistance = player.transform.position.z - start.transform.position.z;
        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)currentDistance / (float)maximumDistance;
        mask.fillAmount = fillAmount;
    }
}
