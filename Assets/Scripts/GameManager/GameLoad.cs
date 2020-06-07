using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameManager
{
    public class GameLoad : MonoBehaviour
    {
        public Text loadText;
        public Image[] loadImage;
        public Sprite loadOverSprite;

        private int _curImage;
        private static AsyncOperation _async;

        private void Start()
        {
            _async = SceneManager.LoadSceneAsync(3);
            _async.allowSceneActivation = false;
            _curImage = 0;
            StartCoroutine(ShowProgress());
        }

        private void Update()
        {
            if (Input.anyKey && _curImage >= 10)
            {
                _async.allowSceneActivation = true;
            }
        }

        private IEnumerator ShowProgress()
        {
            while (_curImage != 9)
            {
                yield return new WaitForSeconds(0.3f);
                yield return new WaitUntil(() => _async.progress * 10 >= _curImage);
                loadImage[_curImage].sprite = loadOverSprite;
                _curImage++;
            }
            yield return new WaitForSeconds(1.0f);
            loadImage[_curImage].sprite = loadOverSprite;
            loadText.text = "Press any key to START";
            _curImage++;
        }
    }
}