using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NetworkControl
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AccountResponse
    {
        public int errorId = 0;
        public string errorMsg = "";
        public int accountId = -1;
    }

    public class WebConnector : MonoBehaviour
    {
        private int _accountId;

        // ReSharper disable once ConvertToConstant.Local
        private readonly string _basicUrl = "http://" + Ip.Host + "/gameAPI/lightless/";
        private AccountManager _accountManager;

        public bool onConnect;

        private void Start()
        {
            _accountId = -1;
            onConnect = false;
            _accountManager = GetComponent<AccountManager>();
        }

        public void GetAccount(string email, string password)
        {
            var param = new Dictionary<string, string> {["account"] = email, ["pwd"] = password};
            onConnect = true;
            StartCoroutine(WebRequestGet<AccountResponse>(_basicUrl + "login_in.php",
                param, response =>
                {
                    if (response.errorId == 0)
                    {
                        _accountId = response.accountId;
                        Debug.Log(_accountId);
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

                    onConnect = false;
                }));
        }

        public void CreateAccount(string email, string password)
        {
            var param = new Dictionary<string, string> {["account"] = email, ["pwd"] = password};
            StartCoroutine(WebRequestGet<AccountResponse>(_basicUrl + "register.php",
                param, response =>
                {
                    if (response.errorId == 0)
                    {
                        _accountId = response.accountId;
                        Debug.Log(_accountId);
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