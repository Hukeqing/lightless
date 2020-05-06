using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class MessageManager : MonoBehaviour
    {
        #region Package

        public GameObject packageMessage;
        public Text packageMessageText;

        private Animator _packageAnimator;


        public void PackageMessage(ItemData itemData)
        {
            packageMessageText.text = itemData.itemName;
            packageMessageText.color = GameDataManager.GetColor(itemData.itemRarity);
        }

        public void ClearPackageMessage()
        {
            packageMessageText.text = "";
        }

        #endregion

        #region Room

        public GameObject roomMessage;
        public Text roomMessageText;
        public Text roomMessageTextDescribe;

        private Animator _roomAnimator;

        public void RoomMessage(RoomData roomData)
        {
            roomMessageText.text = roomData.roomName;
            roomMessageText.color = GameDataManager.GetColor(roomData.roomRarity);
            roomMessageTextDescribe.text = roomData.roomDescribe;
            StartCoroutine(OffShowRoomName());
        }

        private IEnumerator OffShowRoomName()
        {
            yield return new WaitForSeconds(2);
            roomMessageText.text = "";
            roomMessageTextDescribe.text = "";
        }

        #endregion
    }
}