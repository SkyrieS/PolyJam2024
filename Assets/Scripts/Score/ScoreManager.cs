using Game.Player;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

namespace Game.Score
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int scoreToWin;
        [SerializeField] private TextMeshProUGUI player1Score;
        [SerializeField] private TextMeshProUGUI player2Score;
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private float gameTime = 180;

        private Dictionary<PlayerType, int> playersScores;
        private GameManager gameManager;

        private float currentTime;
        private float startTime;
        private bool isPlaying;

        private void Update()
        {
            if (isPlaying)
            {
                if (currentTime >= startTime + gameTime)
                {
                    GameWon();
                    UpdateTimer(0);
                }

                currentTime += Time.deltaTime;
                UpdateTimer(startTime + gameTime - currentTime);
            }
        }

        public void Initialize(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void CreateScores()
        {
            startTime = Time.time;
            currentTime = Time.time;
            isPlaying = true;

            playersScores = new Dictionary<PlayerType, int>();

            playersScores.Add(PlayerType.None, 0);
            playersScores.Add(PlayerType.PlayerOne, 0);
            playersScores.Add(PlayerType.PlayerTwo, 0);
            UpdateUI();
        }

        public void IncreaseScoreByOne(PlayerType player)
        {
            if (playersScores.ContainsKey(player))
            {
                int currentScore = playersScores[player];
                currentScore++;
                playersScores[player] = currentScore;

                Debug.Log($"Player {player} score is {currentScore}");
            }
            else
            {
                playersScores.Add(player, 0);
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            player1Score.text = playersScores[PlayerType.PlayerOne].ToString();
            player2Score.text = playersScores[PlayerType.PlayerTwo].ToString();
        }

        public void GameWon()
        {
            Debug.Log("won");
            isPlaying = false;
            gameManager.EndGame();
        }

        private void UpdateTimer(float seconds)
        {
            int minutes = (int)(seconds / 60);

            int secondsLeft = (int)(seconds - (minutes * 60));
            string minutesText = "0" + minutes;
            string secondsString = secondsLeft < 10 ? "0" + secondsLeft : secondsLeft.ToString();

            timer.text = $"{minutesText}:{secondsString}";
        }
    }

}