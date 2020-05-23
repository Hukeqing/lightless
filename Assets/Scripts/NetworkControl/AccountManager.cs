using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NetworkControl
{
    public class AccountManager : MonoBehaviour
    {
        public Animator loginRegister;
        public InputField emailField;
        public InputField pwdField;
        public InputField repeatField;

        private bool _onRegister;
        private EventSystem _system;
        private WebConnector _webConnector;
        private static readonly int OnRegister = Animator.StringToHash("OnRegister");

        private void Start()
        {
            _onRegister = false;
            _system = EventSystem.current;
            _webConnector = GetComponent<WebConnector>();
            DontDestroyOnLoad(gameObject);
        }

        public void LoginButton()
        {
            if (_webConnector.onConnect) return;
            if (_onRegister)
            {
                _onRegister = false;
                loginRegister.SetBool(OnRegister, _onRegister);
            }
            else
            {
                Login();
            }
        }

        public void RegisterButton()
        {
            if (_webConnector.onConnect) return;
            if (_onRegister)
            {
                Register();
            }
            else
            {
                _onRegister = true;
                loginRegister.SetBool(OnRegister, _onRegister);
            }
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Tab)) return;
            if (_system.currentSelectedGameObject.transform.parent == emailField.transform.parent)
            {
                _system.SetSelectedGameObject(pwdField.gameObject, new BaseEventData(_system));
            }
            else if (_system.currentSelectedGameObject.transform.parent == pwdField.transform.parent && _onRegister)
            {
                _system.SetSelectedGameObject(repeatField.gameObject, new BaseEventData(_system));
            }
        }

        private void Login()
        {
            var email = emailField.text;
            var pwd = pwdField.text;
            _webConnector.GetAccount(email, pwd);
        }

        private void Register()
        {
            var email = emailField.text;
            var pwd = pwdField.text;
            var repeat = repeatField.text;
            const string expression =
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            if (pwd == repeat && pwd.Length >= 6 && System.Text.RegularExpressions.Regex.IsMatch(email, expression))
            {
                _webConnector.CreateAccount(email, pwd);
            }
            else
            {
                Debug.Log("password error");
            }
        }
    }
}