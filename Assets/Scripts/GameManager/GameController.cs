using System;
using System.Globalization;
using NetworkControl;
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

        private static bool _onScene;
        
        private GameStatus _gameStatus;
        private WebConnector _webConnector;

        private void Start()
        {
            if (!_onScene)
            {
                DontDestroyOnLoad(gameObject);
                _onScene = true;
            }
            _webConnector = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<WebConnector>();
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
            _webConnector.StartGame(response =>
            {
                if (response)
                {
                    SceneManager.LoadScene(2);
                }
                else
                {
                    // TODO unknown error
                }
            });
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
                    _webConnector.ReportGame(score);
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