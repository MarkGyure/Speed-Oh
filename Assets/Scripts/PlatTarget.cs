using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatTarget : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private float Pspeed;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Pspeed++;
        }
    }
}
