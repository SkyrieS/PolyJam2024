using Game.Player;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace Game.Score
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int scoreToWin;
        [SerializeField] private TextMeshProUGUI player1Score;
        [SerializeField] private TextMeshProUGUI player2Score;

        private Dictionary<PlayerType, int> playersScores;

        private GameManager gameManager;

        public void Initialize(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void CreateScores()
        {
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

        public void CheckGameWon()
        {
            List<PlayerType> winningPlayers = new List<PlayerType>();
            foreach (var score in playersScores)
            {
                if (score.Value >= scoreToWin)
                {
                    winningPlayers.Add(score.Key);
                }
            }

            for(int i = 0; i < winningPlayers.Count; i++)
            {
                string wonText = i == 0 ? $"{winningPlayers[i]} won!" : $"{winningPlayers[i]} also won! What a coincidence!";
                Debug.Log(wonText);
            }

            if(winningPlayers.Count > 0)
            {
                gameManager.EndGame();
            }
        }
    }

}