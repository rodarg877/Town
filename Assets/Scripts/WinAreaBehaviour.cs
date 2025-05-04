using System.Collections;
using System.Collections.Generic;
using SVS;
using UnityEngine;
using System;

public class WinAreaBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private Visualizer visualizer;
    public static event Action OnPlayerWin;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Has ganado!");
            OnPlayerWin?.Invoke(); // Lanza el evento
        }

    }
}
