using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameManager
{
    public class GameManager : MonoBehaviour
    {
        public float gameScoreValue = 1.1f;
        public Text gameScoreText;

        public Animator gameOverImage;

        private float _gameStartTime;
        private float _gameEndTime;
        private bool _gameOver;

        private GameController _gameController;
        private float _gameMode;
        private float _extraScore;

        private static readonly int Over = Animator.StringToHash("GameOver");
        private static readonly int ScoreUp = Animator.StringToHash("ScoreUp");

        public float GameScore => _extraScore + Mathf.Pow(
                                      _gameOver ? _gameEndTime - _gameStartTime : Time.time - _gameStartTime,
                                      gameScoreValue);

        private void Start()
        {
            _gameOver = false;
            _extraScore = 0;
            _gameStartTime = Time.time;
#if UNITY_EDITOR
            try
            {
#endif
                _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
                _gameMode = _gameController.gameMode;
#if UNITY_EDITOR
            }
            catch (Exception)
            {
                // ignored
            }
#endif
        }

        private void Update()
        {
            gameScoreText.text = ((int) (GameScore * 10.0f) / 10.0f).ToString(CultureInfo.InvariantCulture);
            if (_gameMode > -0.5 && _gameMode < GameScore && !_gameOver)
            {
                gameScoreText.color = Color.green;
            }
        }

        public void GameOver()
        {
            _gameOver = true;
            _gameEndTime = Time.time;
#if UNITY_EDITOR
            try
            {
#endif
                _gameController.GameOver(GameScore);
#if UNITY_EDITOR
            }
            catch (Exception)
            {
                // ignored
            }
#endif
        }

        public void HomeLoad()
        {
            SceneManager.LoadScene(1);
        }

        public void ShowScore()
        {
            gameOverImage.SetTrigger(Over);
        }

        public void AddScore(float value)
        {
            _extraScore += value;
            gameOverImage.SetTrigger(ScoreUp);
        }
    }
}