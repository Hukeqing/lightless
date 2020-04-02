using UnityEngine;

namespace CameraScripts
{
    public class CameraFollower : MonoBehaviour
    {
        public Transform player;
        public CameraControl cc;
        public float cameraSpeed;

        private void LateUpdate()
        {
            transform.position =
                Vector3.Lerp(transform.position, player.position, cameraSpeed * Time.deltaTime * cc.HealthValue);
            // transform.position = player.position;
        }
    }
}
