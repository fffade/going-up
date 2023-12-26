using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    /* Singleton instance */
    public static UI Instance { get; private set; }
    
    /* Different UI elements */
    public GameObject losePanel,
                        winPanel;


    void Awake()
    {
        // Manage singleton
        if (Instance && Instance != this)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        Instance = this;
        
        Debug.Log("Global UI instance instantiated");
    }
}
