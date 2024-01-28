using Game.Player;
using Game.Score;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private ReceiveManager receiveManager;
    [SerializeField] private ItemsSpawner spawner;
    [SerializeField] private PlayersController playersController;

    private bool isRunning;

    public void Awake()
    {
        scoreManager.Initialize(this);
        receiveManager.Initialize(scoreManager, mapGenerator);
        isRunning = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isRunning) 
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        scoreManager.CreateScores();
        receiveManager.SpawnReceivers();
        spawner.StartSpawning();
        playersController.EnableInput();
        isRunning = true;
    }

    public void EndGame()
    {
        spawner.StopSpawning();
        isRunning = false;
        ResetGame();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
