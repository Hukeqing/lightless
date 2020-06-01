using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManager
{
    public enum GameStatus
    {
        LeisureGame,
        HighScoreGame,
        AcceptGame,
        FriendGame,
        AcceptFriendGame
    }

    public class GameController : MonoBehaviour
    {
        public float gameMode;

        private GameStatus _gameStatus;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Leisure()
        {
            gameMode = -1;
            _gameStatus = GameStatus.LeisureGame;
            SceneManager.LoadScene(2);
        }

        public void HighScore()
        {
            gameMode = -1;
            _gameStatus = GameStatus.HighScoreGame;
            SceneManager.LoadScene(2);
        }

        public void GameOver(float score)
        {
            SceneManager.LoadScene(1);
            switch (_gameStatus)
            {
                case GameStatus.LeisureGame:
                    break;
                case GameStatus.HighScoreGame:
#if UNITY_EDITOR
                    Debug.Log("Send score: " + score.ToString(CultureInfo.InvariantCulture));
#endif
                    break;
                case GameStatus.AcceptGame:
                    break;
                case GameStatus.FriendGame:
                    break;
                case GameStatus.AcceptFriendGame:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}