using UnityEngine;

namespace HomeController
{
    public class HomeButton : MonoBehaviour
    {
        public void OnPushShow(GameObject show)
        {
            show.SetActive(true);
        }

        public void OnPushUnShow(GameObject unShow)
        {
            unShow.SetActive(false);
        }
    }
}