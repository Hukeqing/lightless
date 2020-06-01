using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class HomeMessageManager : MonoBehaviour
    {
        public GameObject messageGameObject;
        public Transform canvas;

        private static readonly int Start = Animator.StringToHash("Start");
        private static readonly int Back = Animator.StringToHash("Back");

        public void ShowMessage(string msg, float times)
        {
            var curMsg = Instantiate(messageGameObject, canvas);
            curMsg.GetComponentInChildren<Text>().text = msg;
            var animator = curMsg.GetComponent<Animator>();
            StartCoroutine(WaitForMessage(times, animator));
        }

        private static IEnumerator WaitForMessage(float times, Animator animator)
        {
            yield return new WaitForSeconds(times);
            animator.SetTrigger(Back);
            yield return new WaitForSeconds(1);
            Destroy(animator.gameObject);
        }
    }
}