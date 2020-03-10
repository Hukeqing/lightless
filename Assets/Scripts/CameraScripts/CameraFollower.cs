using UnityEngine;

namespace CameraScripts
{
    public class CameraFollower : MonoBehaviour
    {
        public Transform player;
        public CameraControl cc;

        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, player.position, cc.HealthValue / 10);
        }
    }
}
