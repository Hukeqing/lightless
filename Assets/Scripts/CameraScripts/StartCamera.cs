using GameManager;
using UnityEngine;

namespace CameraScripts
{
    public class StartCamera : MonoBehaviour
    {
        public GameObject cameras;
        public GameObject[] uiList;

        private void Start()
        {
            Time.timeScale = 0;
            foreach (var o in uiList)
            {
                o.SetActive(false);
            }
        }

        // ReSharper disable once UnusedMember.Global
        public void EndMovie()
        {
            cameras.SetActive(true);
            foreach (var o in uiList)
            {
                o.SetActive(true);
            }

            var roomData = new RoomData {dataName = "ISLAND", dataRarity = Rarity.Red, roomDescribe = "一座因为核泄漏而被抛弃的城市"};

            GameObject.FindGameObjectWithTag("GameManager").GetComponent<MessageManager>().RoomMessage(roomData);
            Time.timeScale = 1;
            Destroy(gameObject);
        }
    }
}