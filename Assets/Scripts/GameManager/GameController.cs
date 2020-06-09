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

        private HomeMessageManager _homeMessageManager;
        private GameStatus _gameStatus;
        private WebConnector _webConnector;

        private void Start()
        {
            if (!_onScene)
            {
                DontDestroyOnLoad(gameObject);
                _onScene = true;
            }
#if UNITY_EDITOR
            try
            {
#endif
                _webConnector = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<WebConnector>();
#if UNITY_EDITOR
            }
            catch (Exception)
            {
                // ignored
            }
#endif
            _homeMessageManager = GetComponent<HomeMessageManager>();
        }

        public void Leisure()
        {
            gameMode = -1;
            _gameStatus = GameStatus.LeisureGame;
            SceneManager.LoadScene(2);
        }

        public void HighScore()
        {
            _homeMessageManager.GetYesOrNoMessage(
                "这场比赛将会被服务器记录\n并且影响你的Rank分\n请确保在网络畅通下进行", b =>
                {
                    if (!b) return;
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
#if UNITY_EDITOR
                            Debug.Log("Unknown error on High Score");
#endif
                        }
                    });
                });
        }

        public void AcceptGame()
        {
            _homeMessageManager.GetYesOrNoMessage(
                "这场比赛将会被服务器记录\n并且影响你的Rank分\n请确保在网络畅通下进行", b =>
                {
                    if (!b) return;
                    _gameStatus = GameStatus.AcceptGame;
                    _webConnector.GetGame(f =>
                    {
                        gameMode = f;
                        if (gameMode < 0)
                        {
#if UNITY_EDITOR
                            Debug.Log("Unknown error on accept game");
#endif
                            _homeMessageManager.ShowImportantMessage("抱歉，当前比赛池中\n未能找到匹配\n您当前Rank的比赛", b1 => { });
                        }
                        else
                        {
                            SceneManager.LoadScene(2);
                        }
                    });
                });
        }

        public void GameOver(float score)
        {
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
#if UNITY_EDITOR
                    Debug.Log("Send score: " + score.ToString(CultureInfo.InvariantCulture));
#endif
                    _webConnector.ReportGetGame(score);
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