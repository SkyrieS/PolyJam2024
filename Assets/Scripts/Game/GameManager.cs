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
    [SerializeField] private ScreenTransition transition;
    [SerializeField] private GameObject startPanel;

    private bool isRunning;
    private bool isEnded;

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
        if (Input.GetKeyDown(KeyCode.Space) && isRunning && isEnded)
        {
            ResetGame();
        }
    }

    [ContextMenu("Start")]
    public void StartGame()
    {
        isEnded = false;
        startPanel.SetActive(false);
        transition.SetTarget(0f, null);
        scoreManager.CreateScores();
        receiveManager.SpawnReceivers();
        spawner.StartSpawning();
        playersController.EnableInput();
        isRunning = true;
    }

    [ContextMenu("EndGame")]
    public void EndGame()
    {
        transition.SetTarget(1f, () =>
        {
            spawner.StopSpawning();
            isRunning = false;
            ResetGame();
        });
        spawner.StopSpawning();
        isRunning = false;
        isEnded = true;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
