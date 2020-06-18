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
        public Toggle rememberMeToggle;

        public Text messageText;

        private bool _onRegister;
        private EventSystem _system;
        private WebConnector _webConnector;
        private GameManager.GameController _gameController;

        private static readonly int OnRegister = Animator.StringToHash("OnRegister");

        private void Start()
        {
            _onRegister = false;
            messageText.text = "Login";
            _system = EventSystem.current;
            _webConnector = GetComponent<WebConnector>();
            GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);

            if (PlayerPrefs.HasKey("UserName"))
            {
                emailField.text = PlayerPrefs.GetString("UserName");
            }

            if (PlayerPrefs.HasKey("Password"))
            {
                pwdField.text = PlayerPrefs.GetString("Password");
            }
        }

        public void LoginButton()
        {
            if (_onRegister)
            {
                _onRegister = false;
                loginRegister.SetBool(OnRegister, _onRegister);
                messageText.text = "Login";
                pwdField.text = "";
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
                messageText.text = "Register";
                pwdField.text = "";
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

            if (!Input.GetKeyDown(KeyCode.Return)) return;
            if (_onRegister)
            {
                Register();
            }
            else
            {
                Login();
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
                Error("邮箱格式非法", 0);
                return;
            }

            PlayerPrefs.SetString("UserName", email);
            if (rememberMeToggle.isOn)
            {
                PlayerPrefs.SetString("Password", pwd);
            }
            else
            {
                PlayerPrefs.DeleteKey("Password");
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
            const string nameExpression = @"[A-Za-z0-9]{3,10}";
            if (!System.Text.RegularExpressions.Regex.IsMatch(accountName, nameExpression))
            {
                Error("名字不符合规范，请使用3-10个英文字符或数字组成", -1);
                return;
            }

            const string emailExpression =
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailExpression))
            {
                Error("邮箱错误", 0);
                return;
            }

            if (pwd.Length < 6)
            {
                Error("密码太短了", 1);
                return;
            }

            if (pwd != repeat)
            {
                Error("密码不相同", 2);
                return;
            }

            PlayerPrefs.SetString("UserName", email);
            if (rememberMeToggle.isOn)
            {
                PlayerPrefs.SetString("Password", pwd);
            }
            else
            {
                PlayerPrefs.DeleteKey("Password");
            }

            _webConnector.CreateAccount(accountName, email, pwd);
        }

        public void OnInput(InputField inputField)
        {
            var colorBlock = inputField.colors;
            colorBlock.normalColor = Color.white;
            inputField.colors = colorBlock;
            messageText.text = _onRegister ? "Register" : "Login";
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
                case -1:
                    colorBlock = nameField.colors;
                    colorBlock.normalColor = Color.red;
                    nameField.colors = colorBlock;
                    break;
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