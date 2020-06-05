using UnityEngine;

namespace CameraScripts
{
    public class MapCameraFollower : MonoBehaviour
    {
        public Transform player;

        private Player.PlayerControl _pc;
        private Shader _curShader;
        private Material _material;
        private static readonly int Probability = Shader.PropertyToID("_Probability");

        private void Start()
        {
            _curShader = Shader.Find("Unlit/MapCameraShader");
            _material = new Material(_curShader) {hideFlags = HideFlags.HideAndDontSave};
            _pc = player.GetComponent<Player.PlayerControl>();
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            _material.SetFloat(Probability, 1 - _pc.HealthValue);
            Graphics.Blit(src, dest, _material);
        }

    }
}