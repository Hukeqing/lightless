using System.Collections.Generic;
using GameManager;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkControl
{
    public class FriendsManager : MonoBehaviour
    {
        public Transform content;
        public GameObject friendUi;
        public Friend curSelectFriend;

        public InputField addFriendInputField;

        private WebConnector _webConnector;
        private int _pos;
        private List<Friend> _friends;
        private HomeMessageManager _homeMessageManager;

        private void Start()
        {
            _friends = new List<Friend>();
            _webConnector = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<WebConnector>();
            _homeMessageManager = GetComponent<HomeMessageManager>();
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
            newFriend.friendsManager = this;
            _friends.Add(newFriend);
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
            newFriend.friendsManager = this;
            _pos -= 40;
        }

        public void SelectFriend(Friend friend)
        {
            if (curSelectFriend != null)
            {
                curSelectFriend.friendName.color = Color.black;
                curSelectFriend.SetStatus(FriendStatus.Normal);
            }

            curSelectFriend = friend;
            friend.friendName.color = Color.cyan;
            friend.SetStatus(FriendStatus.OnSelect);
        }

        public void UnSelectFriend()
        {
            if (curSelectFriend != null)
            {
                curSelectFriend.friendName.color = Color.black;
                curSelectFriend.SetStatus(FriendStatus.Normal);
            }

            curSelectFriend = null;
        }

        public void AcceptFriend(int friendId)
        {
            foreach (var friend in _friends)
            {
                Destroy(friend.gameObject);
            }

            _friends.Clear();
            _pos = -60;
            _webConnector.AcceptFriend(friendId);
            _webConnector.GetFriends(this);
        }

        public void AddFriend()
        {
            _webConnector.AddFriend(addFriendInputField.text, errorId =>
            {
                switch (errorId)
                {
                    case 0:
                        _homeMessageManager.ShowMessage("Friend request sent", 3.0f);
                        break;
                    case 404:
                        _homeMessageManager.ShowMessage("Email is not found", 3.0f);
                        break;
                    case 100:
                        _homeMessageManager.ShowMessage("You have this friend", 3.0f);
                        break;
                    case 200:
                        _homeMessageManager.ShowMessage("You are add yourself", 3.0f);
                        break;
                }
            });
        }
    }
}