using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace NetworkControl
{
    #region Response

    // ReSharper disable once ClassNeverInstantiated.Global
    public class AccountResponse
    {
        // ReSharper disable once UnassignedField.Global
        public int errorId;

        // ReSharper disable once UnassignedField.Global
        public string errorMsg;

        // ReSharper disable once UnassignedField.Global
        public int accountId;

        // ReSharper disable once UnassignedField.Global
        public int accountSc;

        // ReSharper disable once UnassignedField.Global
        public string accountNa;
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class FriendsResponse
    {
        // ReSharper disable once UnusedMember.Global
        public int cnt;

        // ReSharper disable once UnassignedField.Global
        // ReSharper disable once CollectionNeverUpdated.Global
        public List<int> accountId;

        // ReSharper disable once UnassignedField.Global
        // ReSharper disable once CollectionNeverUpdated.Global
        public List<int> accountSc;

        // ReSharper disable once UnassignedField.Global
        // ReSharper disable once CollectionNeverUpdated.Global
        public List<string> accountNa;

        // ReSharper disable once UnassignedField.Global
        // ReSharper disable once CollectionNeverUpdated.Global
        public List<int> waitId;

        // ReSharper disable once UnassignedField.Global
        // ReSharper disable once CollectionNeverUpdated.Global
        public List<string> waitNa;
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class GameResponse
    {
        // ReSharper disable once UnassignedField.Global
        public int errorId;

        // ReSharper disable once UnassignedField.Global
        public string errorMsg;

        // ReSharper disable once UnassignedField.Global
        public int gameId;
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoreResponse
    {
        // ReSharper disable once UnassignedField.Global
        public int errorId;

        // ReSharper disable once UnassignedField.Global
        public string errorMsg;

        // ReSharper disable once UnassignedField.Global
        public float score;
    }

    #endregion

    public class WebConnector : MonoBehaviour
    {
        public AccountResponse Account { get; private set; }

        private FriendsResponse _friends;
        private int _curGameId;
        private int _curQueueId;

        // ReSharper disable once ConvertToConstant.Local
        private readonly string _basicUrl = "http://" + Ip.Host + "/gameAPI/lightless/";
        private AccountManager _accountManager;

        public bool OnConnect { get; private set; }

        private void Start()
        {
            OnConnect = false;
            _accountManager = GetComponent<AccountManager>();
        }

        #region Account

        public void GetAccount(string email, string password)
        {
            var param = new Dictionary<string, string> {["account"] = email, ["pwd"] = password};
            OnConnect = true;
            StartCoroutine(WebRequestGet<AccountResponse>(_basicUrl + "login_in.php",
                param, response =>
                {
                    if (response.errorId == 0)
                    {
                        Account = response;
#if UNITY_EDITOR
                        Debug.Log(Account.accountNa);
#endif
                        param.Clear();
                        param["account"] = Account.accountId.ToString();
                        StartCoroutine(WebRequestGet<GameResponse>(_basicUrl + "clear_login.php", param,
                            gameResponse =>
                            {
                                if (gameResponse.gameId != -1)
                                {
                                    param.Clear();
                                    param["id"] = gameResponse.gameId.ToString();
                                    StartCoroutine(WebRequestGet(_basicUrl + "report_competition.php", param,
                                        () => { OnConnect = false; }));
                                }

                                SceneManager.LoadScene(1);
                            }));
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.Log(response.errorMsg);
#endif
                        switch (response.errorId)
                        {
                            case 404:
                                _accountManager.Error(response.errorMsg, 0);
                                break;
                            case 200:
                                _accountManager.Error(response.errorMsg, 1);
                                break;
                            default:
                                _accountManager.Error(response.errorMsg, 3);
                                break;
                        }
                    }
                }));
        }

        public void CreateAccount(string accountName, string email, string password)
        {
            var param = new Dictionary<string, string>
                {["account"] = email, ["pwd"] = password, ["name"] = accountName};
            OnConnect = true;
            StartCoroutine(WebRequestGet<AccountResponse>(_basicUrl + "register.php",
                param, response =>
                {
                    if (response.errorId == 0)
                    {
                        Account = response;
#if UNITY_EDITOR
                        Debug.Log(Account.accountNa);
#endif
                        SceneManager.LoadScene(1);
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.Log(response.errorMsg);
#endif
                        switch (response.errorId)
                        {
                            case 404:
                                _accountManager.Error(response.errorMsg, 0);
                                break;
                            case 200:
                                _accountManager.Error(response.errorMsg, 1);
                                break;
                            default:
                                _accountManager.Error(response.errorMsg, 3);
                                break;
                        }
                    }

                    OnConnect = false;
                }));
        }

        public void GetScore()
        {
            var param = new Dictionary<string, string>
                {["id"] = Account.accountId.ToString()};
            OnConnect = true;
            StartCoroutine(WebRequestGet<AccountResponse>(_basicUrl + "account_score.php",
                param, response =>
                {
                    if (response.errorId == 0)
                    {
                        Account = response;
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.Log(response.errorMsg);
#endif
                        switch (response.errorId)
                        {
                            case 404:
                                _accountManager.Error(response.errorMsg, 0);
                                break;
                            case 200:
                                _accountManager.Error(response.errorMsg, 1);
                                break;
                            default:
                                _accountManager.Error(response.errorMsg, 3);
                                break;
                        }
                    }

                    OnConnect = false;
                }));
        }

        #endregion

        #region Friend

        public void GetFriends(FriendsManager friendsManager)
        {
            var param = new Dictionary<string, string> {["account"] = Account.accountId.ToString()};
            OnConnect = true;
            StartCoroutine(WebRequestGet<FriendsResponse>(_basicUrl + "get_friends.php", param,
                response =>
                {
                    _friends = response;
                    for (var i = 0; i < _friends.accountId.Count; i++)
                    {
                        friendsManager.AddFriendUi(_friends.accountNa[i], _friends.accountSc[i], _friends.accountId[i]);
                    }

                    for (var i = 0; i < _friends.waitId.Count; i++)
                    {
                        friendsManager.AddWaitFriendUi(_friends.waitNa[i], _friends.waitId[i]);
                    }

                    OnConnect = false;
                }));
        }

        public void AcceptFriend(int friendId, FriendsManager friendsManager)
        {
            var param = new Dictionary<string, string>
                {["from"] = friendId.ToString(), ["to"] = Account.accountId.ToString()};
            OnConnect = true;
            StartCoroutine(WebRequestGet(_basicUrl + "accept_friend.php", param,
                () =>
                {
                    OnConnect = false;
                    GetFriends(friendsManager);
                }));
        }

        public void AddFriend(string email, Action<int> callBack)
        {
            var param = new Dictionary<string, string>
                {["from"] = Account.accountId.ToString(), ["account"] = email};
            OnConnect = true;
            StartCoroutine(WebRequestGet<AccountResponse>(_basicUrl + "add_friend.php", param,
                response =>
                {
                    OnConnect = false;
                    callBack(response.errorId);
                }));
        }

        public void DeleteFriend(int friendId, FriendsManager friendsManager)
        {
            var param = new Dictionary<string, string>
                {["from"] = Account.accountId.ToString(), ["to"] = friendId.ToString()};
            OnConnect = true;
            StartCoroutine(WebRequestGet(_basicUrl + "delete_friend.php", param,
                () =>
                {
                    OnConnect = false;
                    GetFriends(friendsManager);
                }));
        }

        #endregion

        #region Game

        public void StartGame(Action<bool> callback)
        {
            var param = new Dictionary<string, string>
                {["account"] = Account.accountId.ToString()};
            OnConnect = true;
            StartCoroutine(WebRequestGet<GameResponse>(_basicUrl + "start_game.php", param,
                response =>
                {
                    switch (response.errorId)
                    {
                        case 200:
#if UNITY_EDITOR
                            Debug.Log(response.errorMsg);
#endif
                            callback(false);
                            break;
                        case 0:
                            _curGameId = response.gameId;

                            param.Clear();
                            param["id"] = _curGameId.ToString();
                            param["player"] = Account.accountId.ToString();
                            StartCoroutine(WebRequestGet<GameResponse>(_basicUrl + "add_queue.php", param,
                                response2 =>
                                {
                                    OnConnect = false;
                                    switch (response2.errorId)
                                    {
                                        case 200:
#if UNITY_EDITOR
                                            Debug.Log(response2.errorMsg);
#endif
                                            callback(false);
                                            break;
                                        case 0:
                                            _curQueueId = response2.gameId;
                                            callback(true);
                                            break;
                                    }
                                }));
                            break;
                    }
                }));
        }

        public void ReportGame(float score)
        {
            var param = new Dictionary<string, string>
                {["id"] = _curGameId.ToString(), ["score"] = score.ToString(CultureInfo.InvariantCulture)};
            OnConnect = true;
            StartCoroutine(WebRequestGet(_basicUrl + "report_game.php", param,
                () =>
                {
                    param.Clear();
                    param["id"] = _curQueueId.ToString();

                    StartCoroutine(WebRequestGet(_basicUrl + "report_queue.php", param,
                        () => { OnConnect = false; }));
                }));
        }

        public void GetGame(Action<float> callback)
        {
            var param = new Dictionary<string, string>
                {["account"] = Account.accountId.ToString()};
            OnConnect = true;
            StartCoroutine(WebRequestGet<GameResponse>(_basicUrl + "start_game.php", param,
                response =>
                {
                    switch (response.errorId)
                    {
                        case 200:
#if UNITY_EDITOR
                            Debug.Log(response.errorMsg);
#endif
                            callback(-1);
                            break;
                        case 0:
                            _curGameId = response.gameId;

                            param.Clear();
                            param["account"] = Account.accountId.ToString();
                            param["re"] = _curGameId.ToString();
                            StartCoroutine(WebRequestGet<GameResponse>(_basicUrl + "get_queue.php", param,
                                response2 =>
                                {
                                    switch (response2.errorId)
                                    {
                                        case 200:
#if UNITY_EDITOR
                                            Debug.Log(response2.errorMsg);
#endif
                                            callback(-1);
                                            break;
                                        case 0:
                                            _curQueueId = response2.gameId;
                                            param.Clear();
                                            param["id"] = _curQueueId.ToString();
                                            StartCoroutine(WebRequestGet<ScoreResponse>(_basicUrl + "get_score.php",
                                                param,
                                                response3 =>
                                                {
                                                    OnConnect = false;
                                                    switch (response2.errorId)
                                                    {
                                                        case 200:
#if UNITY_EDITOR
                                                            Debug.Log(response3.errorMsg);
#endif
                                                            callback(-1);
                                                            break;
                                                        case 0:
                                                            callback(response3.score);
                                                            break;
                                                    }
                                                }));
                                            break;
                                    }
                                }));
                            break;
                    }
                }));
        }

        public void ReportGetGame(float score)
        {
            var param = new Dictionary<string, string>
                {["id"] = _curGameId.ToString(), ["score"] = score.ToString(CultureInfo.InvariantCulture)};
            OnConnect = true;
            StartCoroutine(WebRequestGet(_basicUrl + "report_game.php", param,
                () =>
                {
                    param.Clear();
                    param["id"] = _curQueueId.ToString();

                    StartCoroutine(WebRequestGet(_basicUrl + "report_competition.php", param,
                        () => { OnConnect = false; }));
                }));
        }

        #endregion

        private static IEnumerator WebRequestGet<T>(string url, Dictionary<string, string> param, Action<T> callback)
        {
            var webUrl = url;
            if (param != null)
            {
                var i = 0;
                foreach (var p in param)
                {
                    webUrl += i != 0 ? "&" : "?";
                    webUrl += p.Key + "=" + p.Value;
                    i++;
                }
            }

            var webRequest = UnityWebRequest.Get(webUrl);
            yield return webRequest.SendWebRequest();
            callback(JsonUtility.FromJson<T>(webRequest.downloadHandler.text));
        }

        private static IEnumerator WebRequestGet(string url, Dictionary<string, string> param, Action callback)
        {
            var webUrl = url;
            if (param != null)
            {
                var i = 0;
                foreach (var p in param)
                {
                    webUrl += i != 0 ? "&" : "?";
                    webUrl += p.Key + "=" + p.Value;
                    i++;
                }
            }

            var webRequest = UnityWebRequest.Get(webUrl);
            yield return webRequest.SendWebRequest();
            callback();
        }
    }
}