using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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

        public AudioMixer audioMixer;
        public Slider mainAudio;
        public Slider bgmAudio;
        public Slider effectAudio;

        private Resolution[] _resolutions;
        private bool _onSetting;

        private void Start()
        {
            // var byteArray = Encoding.Default.GetBytes("%E6%B5%8B%E8%AF%95");
            // var decoder = Encoding.GetEncoding("utf-8").GetDecoder();
            // var res = new char[50];
            // decoder.GetChars(byteArray, 0, byteArray.Length, res, 0);
            // Debug.Log(new string(res));

            DontDestroyOnLoad(gameObject);
            _onSetting = false;

            if (!screenResolutions) return;
            _resolutions = Screen.resolutions;
            screenResolutions.ClearOptions();
            screenFrame.ClearOptions();

            Resolution curResolution;
            var frame = -1;
            if (PlayerPrefs.HasKey("Frame"))
            {
                curResolution = new Resolution
                {
                    width = PlayerPrefs.GetInt("Resolution_W"), height = PlayerPrefs.GetInt("Resolution_H")
                };
                frame = PlayerPrefs.GetInt("Frame");
                fullScreenToggle.isOn = PlayerPrefs.GetInt("FullScreen") == 1;

                mainAudio.value = PlayerPrefs.GetFloat("MasterVolume");
                bgmAudio.value = PlayerPrefs.GetFloat("BGMVolume");
                effectAudio.value = PlayerPrefs.GetFloat("EffectVolume");
            }
            else
            {
                curResolution = Screen.currentResolution;
                fullScreenToggle.isOn = true;
                mainAudio.value = 80;
                bgmAudio.value = 80;
                effectAudio.value = 80;
            }

            for (var i = 0; i < _resolutions.Length; i++)
            {
                screenResolutions.AddOptions(new List<string>
                    {_resolutions[i].width + "×" + _resolutions[i].height});

                if (curResolution.width != _resolutions[i].width ||
                    curResolution.height != _resolutions[i].height) continue;
                screenResolutions.value = i;
            }

            for (var i = 0; i < frameList.Length; i++)
            {
                var item = frameList[i];
                screenFrame.AddOptions(item == -1 ? new List<string> {"无限制"} : new List<string> {item.ToString()});
                if (item == frame)
                    screenFrame.value = i;
            }
        }

        public void SetScreen()
        {
            var value = screenResolutions.value;
            Screen.SetResolution(_resolutions[value].width, _resolutions[value].height, fullScreenToggle.isOn);
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
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void ValueChange(int type)
        {
            switch (type)
            {
                case 0:
                    audioMixer.SetFloat("MasterVolume", mainAudio.value - 80);
                    break;
                case 1:
                    audioMixer.SetFloat("BGMVolume", bgmAudio.value - 80);
                    break;
                case 2:
                    audioMixer.SetFloat("EffectVolume", effectAudio.value - 80);
                    break;
            }
        }

        public void OnApplicationQuit()
        {
            PlayerPrefs.SetFloat("MasterVolume", mainAudio.value);
            PlayerPrefs.SetFloat("BGMVolume", bgmAudio.value);
            PlayerPrefs.SetFloat("EffectVolume", effectAudio.value);
            var value = screenResolutions.value;
            PlayerPrefs.SetInt("Resolution_W", _resolutions[value].width);
            PlayerPrefs.SetInt("Resolution_H", _resolutions[value].height);
            PlayerPrefs.SetInt("Frame", frameList[screenFrame.value]);
            PlayerPrefs.SetInt("FullScreen", fullScreenToggle.isOn ? 1 : 0);
        }
    }
}