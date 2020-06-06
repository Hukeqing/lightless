﻿using System.Collections.Generic;
using GameManager;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkControl
{
    public class FriendsManager : MonoBehaviour
    {
        public Text nameText;
        public Text scoreText;

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
            _webConnector.GetScore();
            _pos = -60;
            SetMine(_webConnector.Account);
        }

        private void SetMine(AccountResponse accountResponse)
        {
            nameText.text = accountResponse.accountNa;
            nameText.color = ScoreColor(accountResponse.accountSc);
            scoreText.text = "Rating: " + accountResponse.accountSc;
        }

        public void AddFriendUi(string friendName, int score, int id)
        {
            var newFriend = Instantiate(friendUi, content).GetComponent<Friend>();
            newFriend.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _pos);
            newFriend.friendId = id;
            newFriend.friendName.text = friendName;
            newFriend.friendName.color = ScoreColor(score);
            newFriend.friendScore.text = score.ToString();
            newFriend.SetStatus(FriendStatus.Normal);
            newFriend.friendsManager = this;
            _friends.Add(newFriend);
            _pos -= 40;
        }

        public void AddWaitFriendUi(string friendName, int id)
        {
            var newFriend = Instantiate(friendUi, content).GetComponent<Friend>();
            newFriend.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _pos);
            newFriend.friendId = id;
            newFriend.friendName.text = friendName;
            newFriend.friendName.color = Color.white;
            newFriend.friendScore.text = "";
            newFriend.SetStatus(FriendStatus.OnHold);
            newFriend.friendsManager = this;
            _friends.Add(newFriend);
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
            ClearFriend();
            _webConnector.AcceptFriend(friendId, this);
        }

        public void AddFriend()
        {
            _webConnector.AddFriend(addFriendInputField.text, errorId =>
            {
                var msg = "Unknown Error";
                switch (errorId)
                {
                    case 0:
                        msg = "Friend request sent";
                        break;
                    case 404:
                        msg = "Email is not found";
                        break;
                    case 100:
                        msg = "You have this friend";
                        break;
                    case 200:
                        msg = "You are add yourself";
                        break;
                }

                addFriendInputField.text = "";
                _homeMessageManager.ShowMessage(msg, 3.0f);
            });
        }

        public void DeleteFriend()
        {
            if (curSelectFriend == null)
            {
                _homeMessageManager.ShowMessage("Please select a friend", 3);
                return;
            }

            ClearFriend();
            var friendId = curSelectFriend.friendId;
            _webConnector.DeleteFriend(friendId, this);
        }

        private void ClearFriend()
        {
            foreach (var friend in _friends)
            {
                Destroy(friend.gameObject);
            }

            _friends.Clear();
            _pos = -60;
        }

        private static Color ScoreColor(int score)
        {
            if (score < 1200)
            {
                return Color.gray;
            }

            if (score < 1400)
            {
                return Color.green;
            }

            if (score < 1600)
            {
                return new Color(0.47f, 0.87f, 0.73f);
            }

            if (score < 1900)
            {
                return new Color(0.67f, 0.67f, 1f);
            }

            if (score < 2100)
            {
                return new Color(1f, 0.53f, 1f);
            }

            if (score < 2300)
            {
                return new Color(1f, 0.8f, 0.53f);
            }

            if (score < 2400)
            {
                return new Color(1f, 0.73f, 0.33f);
            }

            if (score < 2600)
            {
                return new Color(1f, 0.47f, 0.47f);
            }

            return score < 3000 ? new Color(1f, 0.2f, 0.2f) : new Color(0.67f, 0f, 0f);
        }
    }
}