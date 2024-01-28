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
    [SerializeField] private GameObject firstPlayerPanel;
    [SerializeField] private GameObject secondPlayerPanel;
    [SerializeField] private GameObject drawPlayerPanel;

    private bool isRunning;
    public bool isEnded;

    public void Awake()
    {
        scoreManager.Initialize(this);
        receiveManager.Initialize(scoreManager, mapGenerator);
        isRunning = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isEnded)
        {
            ResetGame();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isRunning) 
        {
            StartGame();
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
    public void EndGame(bool iWonImFirstPlayer, bool iWonImSecondPlayer)
    {
        transition.SetTarget(1f, () =>
        {
            spawner.StopSpawning();
            spawner.StopSpawning();
            isRunning = false;
            isEnded = true;
            if (iWonImFirstPlayer)
            {
                firstPlayerPanel.SetActive(true);
            }
            else if (iWonImSecondPlayer)
            {
                secondPlayerPanel.SetActive(true);
            }
            else
            {
                drawPlayerPanel.SetActive(true);
            }
        });
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
