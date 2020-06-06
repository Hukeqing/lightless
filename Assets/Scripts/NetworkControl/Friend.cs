using System;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkControl
{
    public enum FriendStatus
    {
        Normal,
        OnFight,
        OnHold,
        OnSelect
    }

    public class Friend : MonoBehaviour
    {
        public int friendId;
        public int friendSc;
        public Text friendScore;
        public Text friendName;
        public Button button;
        
        [HideInInspector] public FriendsManager friendsManager;

        private FriendStatus _friendStatus;

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
                case FriendStatus.OnSelect:
                    button.GetComponentInChildren<Text>().text = "UnSelect";
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
                    friendsManager.SelectFriend(this);
                    break;
                case FriendStatus.OnFight:
                    break;
                case FriendStatus.OnHold:
                    friendsManager.AcceptFriend(friendId);
                    break;
                case FriendStatus.OnSelect:
                    friendsManager.UnSelectFriend();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}