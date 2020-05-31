using System.Collections.Generic;
using UnityEngine;

namespace NetworkControl
{
    public class FriendsManager : MonoBehaviour
    {
        public Transform content;
        public GameObject friendUi;

        private WebConnector _webConnector;
        private int _pos;
        private List<Friend> _friends;

        private void Start()
        {
            _webConnector = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<WebConnector>();
            _webConnector.GetFriends(this);
            _pos = -60;
        }

        public void AddFriend(string friendName, int score, int id)
        {
            var newFriend = Instantiate(friendUi, content).GetComponent<Friend>();
            newFriend.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _pos);
            newFriend.friendId = id;
            newFriend.friendName.text = friendName;
            newFriend.friendScore.text = score.ToString();
            newFriend.SetStatus(FriendStatus.Normal);
            _pos -= 40;
        }

        public void AddWaitFriend(string friendName, int id)
        {
            
            var newFriend = Instantiate(friendUi, content).GetComponent<Friend>();
            newFriend.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _pos);
            newFriend.friendId = id;
            newFriend.friendName.text = friendName;
            newFriend.friendScore.text = "";
            newFriend.SetStatus(FriendStatus.OnHold);
            _pos -= 40;
        }
    }
}