using UnityEngine;

namespace Effect
{
    public class ButtonAudio : MonoBehaviour
    {
        private AudioSource _mouseAudio;

        private void Start()
        {
            _mouseAudio = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            _mouseAudio.Stop();
            _mouseAudio.Play();
        }
    }
}