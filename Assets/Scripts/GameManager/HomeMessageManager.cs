using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class HomeMessageManager : MonoBehaviour
    {
        public GameObject messageGameObject;
        public Transform canvas;

        public GameObject messageOkGameObject;
        public GameObject messageNoButton;
        public Text messageOkText;

        private int _check;

        private static readonly int Back = Animator.StringToHash("Back");

        private void Start()
        {
            if (messageOkGameObject != null)
            {
                messageOkGameObject.SetActive(false);
            }

            _check = 1;
        }

        public void ShowMessage(string msg, float times)
        {
            var curMsg = Instantiate(messageGameObject, canvas);
            curMsg.GetComponentInChildren<Text>().text = msg;
            var animator = curMsg.GetComponent<Animator>();
            StartCoroutine(WaitForMessage(times, animator));
        }

        public void ShowImportantMessage(string msg, Action<bool> callback)
        {
            if (_check == 0)
            {
#if UNITY_EDITOR
                Debug.Log("Message miss");
#endif
                return;
            }

            messageOkGameObject.SetActive(true);
            messageNoButton.SetActive(false);
            messageOkText.text = msg;
            _check = 0;
            StartCoroutine(WaitForCheck(callback));
        }

        public void GetYesOrNoMessage(string msg, Action<bool> callback)
        {            if (_check == 0)
            {
#if UNITY_EDITOR
                Debug.Log("Message miss");
#endif
                return;
            }

            messageOkGameObject.SetActive(true);
            messageNoButton.SetActive(true);
            messageOkText.text = msg;
            _check = 0;
            StartCoroutine(WaitForCheck(callback));
        }

        public void SetCheck(bool flag)
        {
            _check = flag ? 1 : -1;
        }

        private IEnumerator WaitForCheck(Action<bool> callback)
        {
            yield return new WaitUntil(() => _check != 0);
            messageOkGameObject.SetActive(false);
            callback(_check == 1);
        }

        private static IEnumerator WaitForMessage(float times, Animator animator)
        {
            yield return new WaitForSeconds(times);
            if (animator != null)
            {
                animator.SetTrigger(Back);
            }

            yield return new WaitForSeconds(1);
            if (animator != null)
            {
                Destroy(animator.gameObject);
            }
        }
    }
}