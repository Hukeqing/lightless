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


            audioMixer.GetFloat("MasterVolume", out var mainAudioValue);
            mainAudio.value = mainAudioValue + 80;
            audioMixer.GetFloat("BGMVolume", out var bgmAudioValue);
            bgmAudio.value = bgmAudioValue + 80;
            audioMixer.GetFloat("EffectVolume", out var effectAudioValue);
            effectAudio.value = effectAudioValue + 80;
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
            Application.Quit();
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
    }
}