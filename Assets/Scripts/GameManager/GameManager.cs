using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class GameManager : MonoBehaviour
    {
        public float gameScoreValue = 1.1f;

        public Text gameScoreText;

        private float _gameStartTime;
        private float _gameEndTime;
        private bool _gameOver;

        private float GameScore => Mathf.Pow(_gameOver ? _gameEndTime : Time.time - _gameStartTime, gameScoreValue);

        private void Start()
        {
            _gameOver = false;
            _gameStartTime = Time.time;
        }

        private void Update()
        {
            gameScoreText.text = ((int) (GameScore * 10.0f) / 10.0f).ToString(CultureInfo.InvariantCulture);
        }

        public void GameOver()
        {
            _gameOver = true;
            _gameEndTime = Time.time;
        }
    }
}