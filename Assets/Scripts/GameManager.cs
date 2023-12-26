using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* Singleton instance */
    public static GameManager Instance { get; private set; }
    
    /* Game elements */
    private FogMovement _fogMovement;
    
    private Transform _player;

    [SerializeField] private Transform playerStart;
    
    
    void Awake()
    {
        // Singleton instantiation
        Instance = this;
        
        // Game Elements
        _fogMovement = FindObjectOfType<FogMovement>();
        
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    // Resets the game to the beginning
    void ResetGame()
    {
        Log("Game reset");
        
        // Reset all UI elements
        UI.Instance.losePanel.SetActive(false);
        UI.Instance.winPanel.SetActive(false);

        // Reset game elements
        _player.transform.position = playerStart.position;
        
        _fogMovement.ResetPosition();
    }
    
    // Triggered when the player wins
    public void OnWin()
    {
        Log("Game win condition met");
        
        UI.Instance.winPanel.SetActive(true);

        _fogMovement.isMoving = false;
    }
    
    // Triggered when the player loses
    public void OnLose()
    {
        Log("Game loss condition met");
        
        UI.Instance.losePanel.SetActive(true);
        
        _fogMovement.isMoving = false;
    }
    
    // Logs a message related to the game manager
    void Log(string message)
    {
        Debug.Log($"[Game Manager] {message}");
    }
}
