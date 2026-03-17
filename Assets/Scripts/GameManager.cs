// Game Manager - beheert game state en Game Over

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Subscribe aan game over event
        EnemyAI.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        // Unsubscribe
        EnemyAI.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;
        Debug.Log("GAME OVER - Player captured!");
        
        // Hier kun je Game Over UI tonen, sounds afspelen, etc.
        // Voor nu pauzeren we het spel en tonen we een bericht
        
        // Reload na 3 seconden
        Invoke(nameof(ReloadScene), 3f);
    }

    private void ReloadScene()
    {
        Time.timeScale = 1; // Resume het spel
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsGameOver => isGameOver;
}
