using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class MessageManager : MonoBehaviour
    {
        public GameObject packageMessage;
        public Text packageMessageText;

        private Animator _packageAnimator;

        public Animator roomMessage;
        public Text roomMessageText;
        public Text roomMessageTextDescribe;

        private bool _onRoomMessage;
        private float _roomMessageStartTime;
        private static readonly int StartTrigger = Animator.StringToHash("Start");
        private static readonly int EndTrigger = Animator.StringToHash("End");

        #region Package

        public void PackageMessage(ItemData itemData)
        {
            packageMessage.SetActive(true);
            packageMessageText.text = itemData.itemName;
            packageMessageText.color = GameDataManager.GetColor(itemData.itemRarity);
        }

        public void ClearPackageMessage()
        {
            packageMessage.SetActive(false);
        }

        #endregion

        #region Room

        public void RoomMessage(RoomData roomData)
        {
            _roomMessageStartTime = Time.time;
            roomMessageText.text = roomData.roomName;
            roomMessageText.color = GameDataManager.GetColor(roomData.roomRarity);
            roomMessageTextDescribe.text = roomData.roomDescribe;

            StartCoroutine(OffShowRoomName());
        }

        private IEnumerator OffShowRoomName()
        {
            if (!_onRoomMessage)
            {
                roomMessage.SetTrigger(StartTrigger);
                _onRoomMessage = true;
            }

            yield return new WaitUntil(() => Time.time >= _roomMessageStartTime + 2.0f);
            roomMessage.SetTrigger(EndTrigger);
            _onRoomMessage = false;
            yield return new WaitForSeconds(1);
        }

        #endregion

        private void Start()
        {
            packageMessage.SetActive(false);
            roomMessageText.text = "";
            roomMessageTextDescribe.text = "";
        }
    }
}