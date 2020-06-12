using GameManager;
using UnityEngine;

namespace HomeController
{
    public class GameCube : MonoBehaviour
    {
        public Transform cube;
        public float rotateSpeed;
        [Range(0, 1)] public float speedDrag;

        private float _dragSpeed;

        private Vector3 _lastMousePos;
        private Vector3 _speed;
        private bool _onDrag;

        #region Game

        private GameController _gameController;

        #endregion

        private void Start()
        {
            _onDrag = false;
            _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        private void OnMouseDrag()
        {
            if (!_onDrag) return;
            var tmp = Input.mousePosition - _lastMousePos;
            cube.Rotate(Vector3.down * (rotateSpeed * tmp.x * Time.deltaTime), Space.World);
            _lastMousePos = Input.mousePosition;
            _dragSpeed = rotateSpeed * tmp.x;
        }

        private void Update()
        {
            if (_onDrag) return;
            cube.Rotate(Vector3.down * (_dragSpeed * Time.deltaTime), Space.World);
            _dragSpeed *= speedDrag;
        }

        private void OnMouseDown()
        {
            _onDrag = true;
            _lastMousePos = Input.mousePosition;
        }

        private void OnMouseUp()
        {
            _onDrag = false;
        }
        
        
        public void Leisure()
        {
            _gameController.Leisure();
        }

        public void HighScore()
        {
            _gameController.HighScore();
        }

        public void AcceptGame()
        {
            _gameController.AcceptGame();
        }

    }
}