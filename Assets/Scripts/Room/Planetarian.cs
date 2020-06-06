using UnityEngine;

namespace Room
{
    public class Planetarian : Room
    {
        public AudioSource planetarian;

        public override void Enter()
        {
            planetarian.Play();
        }

        public override void Exit()
        {
            planetarian.Stop();
        }
    }
}