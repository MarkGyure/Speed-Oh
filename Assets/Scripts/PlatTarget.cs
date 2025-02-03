using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatTarget : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private PlayerController playerController;
   // [SerializeField] private float Pspeed;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Target");
            playerController.SpeedUp();
            Destroy(gameObject);
           // Pspeed++;
        }
    }
}
