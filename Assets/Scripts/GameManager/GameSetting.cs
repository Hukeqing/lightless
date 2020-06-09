using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class GameSetting : MonoBehaviour
    {
        public Dropdown screenResolutions;
        public Dropdown screenFrame;
        public int[] frameList;
        public Toggle fullScreenToggle;

        public GameObject settingGameObject;

        private Resolution[] _resolutions;
        private bool _onSetting;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            _onSetting = false;

            if (!screenResolutions) return;
            _resolutions = Screen.resolutions;
            screenResolutions.ClearOptions();
            screenFrame.ClearOptions();
            for (var i = 0; i < _resolutions.Length; i++)
            {
                screenResolutions.AddOptions(new List<string>
                    {_resolutions[i].width + "×" + _resolutions[i].height});

                if (Screen.currentResolution.width != _resolutions[i].width ||
                    Screen.currentResolution.height != _resolutions[i].height) continue;
                screenResolutions.value = i;
            }

            foreach (var item in frameList)
            {
                screenFrame.AddOptions(item == -1 ? new List<string> {"无限制"} : new List<string> {item.ToString()});
            }

            screenFrame.value = frameList.Length - 1;
            fullScreenToggle.isOn = Screen.fullScreen;
        }

        public void SetScreen()
        {
            var value = screenResolutions.value;
            if (Screen.currentResolution.width == _resolutions[value].width &&
                Screen.currentResolution.height == _resolutions[value].height)
            {
                return;
            }

            Screen.SetResolution(_resolutions[value].width, _resolutions[value].height, Screen.fullScreen);
        }

        public void SetFrame()
        {
            Application.targetFrameRate = frameList[screenFrame.value];
        }

        public void Setting()
        {
            _onSetting = !_onSetting;
            settingGameObject.SetActive(_onSetting);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}