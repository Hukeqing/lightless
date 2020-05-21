using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NetworkControl
{
    public class AccountResponse
    {
        public int errorNo;
        public string errorMsg;
        public int accountId;
    }

    public class WebConnector : MonoBehaviour
    {
        private int _accountId;

        public bool GetAccounted => _accountId != -1;

        private void Start()
        {
            _accountId = -1;
        }

        public void GetAccount(string email, string password)
        {
            
        }

        private IEnumerator WebRequestGet<T>(string url, Dictionary<string, string> param, Action<T> callback)
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