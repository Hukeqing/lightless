using CameraScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Room
{
    public class Planetarian : Room
    {
        public AudioSource planetarian;

        public override void Enter()
        {
            planetarian.Play();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().nextAudioTime =
                Mathf.Infinity;
            GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().Stop();
        }

        public override void Exit()
        {
            planetarian.Stop();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().nextAudioTime =
                Time.time + Random.Range(10, 30);
        }
    }
}