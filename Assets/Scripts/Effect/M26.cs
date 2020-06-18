using UnityEngine;

namespace Effect
{
    public class M26 : MonoBehaviour
    {
        [HideInInspector] public Vector3 target;
        public int damage;
        public float moveSpeed;
        public ParticleSystem a, b;
        public float maxDistance = 3;

        private bool _hit;
        private AudioSource _audioSource;

        private void Start()
        {
            _hit = false;
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_hit) return;

            transform.LookAt(target);
            transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
            if (Vector3.Distance(target, transform.position) > 0.3f) return;

            _hit = true;

            var res = new Collider[5];
            var size = Physics.OverlapSphereNonAlloc(transform.position, maxDistance, res, 1 << 13);
            for (var i = 0; i < size; i++)
            {
                res[i].GetComponent<Enemy.Unit>().ApplyDamage(
                    Mathf.CeilToInt(damage * Mathf.Clamp01(
                        1 - Vector3.Distance(transform.position, res[i].transform.position) /
                        maxDistance
                    )));
            }

            a.Play();
            b.Play();
            _audioSource.Play();
            Destroy(gameObject, 2);
        }
    }
}