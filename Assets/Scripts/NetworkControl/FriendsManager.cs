﻿using System;
using System.Collections.Generic;
using HomeController;
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

        public float flushTime;

        private WebConnector _webConnector;
        private int _pos;
        private List<Friend> _friends;
        private HomeMessageManager _homeMessageManager;
        private float _nextFlush;

        private void Start()
        {
            _friends = new List<Friend>();
            _nextFlush = Time.time + flushTime;
#if UNITY_EDITOR
            try
            {
#endif
                _webConnector = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<WebConnector>();
                _homeMessageManager = GetComponent<HomeMessageManager>();
                _webConnector.GetFriends(this);
                _webConnector.GetScore(() => { SetMine(_webConnector.Account); });
                _pos = -130;
#if UNITY_EDITOR
            }
            catch (Exception)
            {
                // ignored
            }
#endif
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
            newFriend.friendSc = score;
            newFriend.friendScore.text = score.ToString();
            newFriend.SetStatus(FriendStatus.Normal);
            newFriend.friendsManager = this;
            _friends.Add(newFriend);
            _pos -= 85;
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
            _pos -= 85;
        }

        public void SelectFriend(Friend friend)
        {
            if (curSelectFriend != null)
            {
                curSelectFriend.friendName.color = ScoreColor(curSelectFriend.friendSc);
                curSelectFriend.SetStatus(FriendStatus.Normal);
            }

            curSelectFriend = friend;
            friend.friendName.color = Color.white;
            friend.SetStatus(FriendStatus.OnSelect);
        }

        public void UnSelectFriend()
        {
            if (curSelectFriend != null)
            {
                curSelectFriend.friendName.color = ScoreColor(curSelectFriend.friendSc);
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
                        msg = "好友申请已发送";
                        break;
                    case 404:
                        msg = "此邮箱未被注册";
                        break;
                    case 100:
                        msg = "你已经拥有这个好友了";
                        break;
                    case 200:
                        msg = "你试图添加自己为好友";
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
                _homeMessageManager.ShowMessage("请先选择一个好友", 3);
                return;
            }

            ClearFriend();
            var friendId = curSelectFriend.friendId;
            _webConnector.DeleteFriend(friendId, this);
        }

        public void Flush()
        {
            if (Time.time < _nextFlush)
            {
                _homeMessageManager.ShowMessage("刷新太频繁啦！！！", 3);
                return;
            }

            _nextFlush = Time.time + flushTime;
            ClearFriend();
            _webConnector.GetFriends(this);
            _webConnector.GetScore(() => { SetMine(_webConnector.Account); });
        }

        private void ClearFriend()
        {
            foreach (var friend in _friends)
            {
                Destroy(friend.gameObject);
            }

            _friends.Clear();
            _pos = -130;
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
                return new Color(0.16f, 0.71f, 0.67f);
            }

            if (score < 1900)
            {
                return new Color(0.06f, 0.07f, 0.92f);
            }

            if (score < 2100)
            {
                return new Color(0.71f, 0.15f, 0.71f);
            }

            if (score < 2400)
            {
                return new Color(1f, 0.61f, 0.15f);
            }

            return score < 3000 ? new Color(1f, 0.15f, 0.15f) : new Color(0.67f, 0f, 0f);
        }
    }
}