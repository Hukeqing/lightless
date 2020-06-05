﻿using System;
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

        private static readonly int Over = Animator.StringToHash("GameOver");

        private float GameScore => Mathf.Pow(_gameOver ? _gameEndTime - _gameStartTime : Time.time - _gameStartTime,
            gameScoreValue);

        private void Start()
        {
            _gameOver = false;
            _gameStartTime = Time.time;
#if UNITY_EDITOR
            try
            {
                _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
                _gameMode = _gameController.gameMode;
            }
            catch (Exception)
            {
                // ignored
            }
#else
            _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            _gameMode = _gameController.gameMode;
#endif
        }

        private void Update()
        {
            gameScoreText.text = ((int) (GameScore * 10.0f) / 10.0f).ToString(CultureInfo.InvariantCulture);
            if (_gameMode > 0 && _gameMode < GameScore && !_gameOver)
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
                _gameController.GameOver(GameScore);
            }
            catch (Exception)
            {
                // ignored
            }
#else
            _gameController.GameOver(GameScore);
#endif
        }

        public void SendScore()
        {
            SceneManager.LoadScene(1);
        }

        public void ShowScore()
        {
            gameOverImage.SetTrigger(Over);
        }
    }
}