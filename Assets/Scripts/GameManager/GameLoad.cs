﻿using System.Collections;
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
            yield return new WaitForSeconds(0.3f);
            _async = SceneManager.LoadSceneAsync(3);
            _async.allowSceneActivation = false;
            _curImage = 0;
            while (_curImage != 9)
            {
                yield return new WaitForSeconds(0.1f);
                yield return new WaitUntil(() => _async.progress * 10 >= _curImage);
                loadImage[_curImage].sprite = loadOverSprite;
                _curImage++;
            }

            yield return new WaitForSeconds(1.0f);
            loadImage[_curImage].sprite = loadOverSprite;
            loadText.text = "Press any key to START";
            loadText.color = Color.red;
            _curImage++;
        }
    }
}