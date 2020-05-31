using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NetworkControl
{
    public class AccountManager : MonoBehaviour
    {
        public Animator loginRegister;
        public InputField nameField;
        public InputField emailField;
        public InputField pwdField;
        public InputField repeatField;

        public Text messageText;

        private bool _onRegister;
        private EventSystem _system;
        private WebConnector _webConnector;
        private GameManager.GameController _gameController;
        
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
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (_system.currentSelectedGameObject.transform.parent == nameField.transform.parent)
                {
                    _system.SetSelectedGameObject(emailField.gameObject, new BaseEventData(_system));
                }
                else if (_system.currentSelectedGameObject.transform.parent == emailField.transform.parent)
                {
                    _system.SetSelectedGameObject(pwdField.gameObject, new BaseEventData(_system));
                }
                else if (_system.currentSelectedGameObject.transform.parent == pwdField.transform.parent && _onRegister)
                {
                    _system.SetSelectedGameObject(repeatField.gameObject, new BaseEventData(_system));
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (_onRegister)
                {
                    Register();
                }
                else
                {
                    Login();
                }
            }
        }

        private void Login()
        {
            if (_webConnector.OnConnect) return;
            var email = emailField.text;
            var pwd = pwdField.text;
            const string expression =
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, expression))
            {
                Error("Email is not a email", 0);
                return;
            }

            _webConnector.GetAccount(email, pwd);
        }

        private void Register()
        {
            if (_webConnector.OnConnect) return;
            var accountName = nameField.text;
            var email = emailField.text;
            var pwd = pwdField.text;
            var repeat = repeatField.text;
            const string expression =
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            if (pwd != repeat)
            {
                Error("Password is not same", 2);
                return;
            }

            if (pwd.Length < 6)
            {
                Error("Password is too short", 1);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(email, expression))
            {
                Error("Email is not a email", 0);
                return;
            }

            _webConnector.CreateAccount(accountName, email, pwd);
        }

        public void OnInput(InputField inputField)
        {
            var colorBlock = inputField.colors;
            colorBlock.normalColor = Color.white;
            inputField.colors = colorBlock;
            messageText.text = "";
        }

        /// <summary>
        /// Error Show
        /// </summary>
        /// <param name="msg">error message</param>
        /// <param name="errorId">where error</param>
        public void Error(string msg, int errorId)
        {
            ColorBlock colorBlock;
            messageText.text = msg;
            switch (errorId)
            {
                case 0:
                    colorBlock = emailField.colors;
                    colorBlock.normalColor = Color.red;
                    emailField.colors = colorBlock;
                    break;
                case 1:
                    colorBlock = pwdField.colors;
                    colorBlock.normalColor = Color.red;
                    pwdField.colors = colorBlock;
                    break;
                case 2:
                    colorBlock = repeatField.colors;
                    colorBlock.normalColor = Color.red;
                    repeatField.colors = colorBlock;
                    break;
            }
        }
    }
}