using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace NetworkControl
{
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
        public int cnt;
        public List<int> accountId;
        public List<int> accountSc;
        public List<string> accountNa;
        public List<int> waitId;
        public List<string> waitNa;
    }

    public class WebConnector : MonoBehaviour
    {
        private AccountResponse _account;
        private FriendsResponse _friends;

        // ReSharper disable once ConvertToConstant.Local
        private readonly string _basicUrl = "http://" + Ip.Host + "/gameAPI/lightless/";
        private AccountManager _accountManager;

        public bool OnConnect { get; private set; }

        private void Start()
        {
            OnConnect = false;
            _accountManager = GetComponent<AccountManager>();
        }

        public void GetAccount(string email, string password)
        {
            var param = new Dictionary<string, string> {["account"] = email, ["pwd"] = password};
            OnConnect = true;
            StartCoroutine(WebRequestGet<AccountResponse>(_basicUrl + "login_in.php",
                param, response =>
                {
                    if (response.errorId == 0)
                    {
                        _account = response;
                        Debug.Log(_account.accountNa);
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

        public void CreateAccount(string accountName, string email, string password)
        {
            var param = new Dictionary<string, string>
                {["account"] = email, ["pwd"] = password, ["name"] = accountName};
            StartCoroutine(WebRequestGet<AccountResponse>(_basicUrl + "register.php",
                param, response =>
                {
                    if (response.errorId == 0)
                    {
                        _account = response;
                        Debug.Log(_account.accountNa);
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
                }));
        }

        public void GetFriends(FriendsManager friendsManager)
        {
            var param = new Dictionary<string, string> {["account"] = _account.accountId.ToString()};
            OnConnect = true;
            StartCoroutine(WebRequestGet<FriendsResponse>(_basicUrl + "get_friends.php", param,
                response =>
                {
                    _friends = response;
                    for (var i = 0; i < _friends.accountId.Count; i++)
                    {
                        friendsManager.AddFriend(_friends.accountNa[i], _friends.accountSc[i], _friends.accountId[i]);
                    }

                    for (var i = 0; i < _friends.waitId.Count; i++)
                    {
                        friendsManager.AddWaitFriend(_friends.waitNa[i], _friends.waitId[i]);
                    }
                }));
        }

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
    }
}