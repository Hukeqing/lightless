using System;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkControl
{
    public enum FriendStatus
    {
        Normal, OnFight, OnHold
    }
    
    public class Friend : MonoBehaviour
    {
        public int friendId;
        public Text friendScore;
        public Text friendName;
        private FriendStatus _friendStatus;
        public Button button;

        public void SetStatus(FriendStatus friendStatus)
        {
            _friendStatus = friendStatus;
            switch (friendStatus)
            {
                case FriendStatus.Normal:
                    button.GetComponentInChildren<Text>().text = "Select";
                    break;
                case FriendStatus.OnFight:
                    button.GetComponentInChildren<Text>().text = "Fight";
                    break;
                case FriendStatus.OnHold:
                    button.GetComponentInChildren<Text>().text = "Accept";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(friendStatus), friendStatus, null);
            }
        }

        public void PushButton()
        {
            switch (_friendStatus)
            {
                case FriendStatus.Normal:
                    break;
                case FriendStatus.OnFight:
                    break;
                case FriendStatus.OnHold:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
