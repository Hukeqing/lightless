using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class M40A3Weapon : Weapon
    {
        public int damage;
        public float maxRange;
        public float lineDisableInt;
        public LineRenderer attackLine;

        public Transform targetTransform;

        public float waitTime;

        private Ray _ray;
        private float _startTime;
        private Vector3 _cameraBasicPos;
        private Quaternion _cameraBasicQuaternion;

        private float _lineDisableTime;
        private GameObject _mainCameraObject;
        private Camera _mainCamera;
        private GameObject _message;

        private void Start()
        {
            attackLine.gameObject.SetActive(false);
            Init();
            _mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");
            _mainCamera = _mainCameraObject.GetComponent<Camera>();
            _message = GameObject.FindGameObjectWithTag("MouthHitMessage").transform.GetChild(0).gameObject;
        }

        public override void Attack()
        {
        }

        private void Update()
        {
            if (!attackLine.enabled || !(_lineDisableTime <= Time.time)) return;
            attackLine.gameObject.SetActive(false);
        }

        private IEnumerator AttackTime()
        {
            _startTime = 0;
            Time.timeScale = 0f;

            _message.SetActive(true);

            var transform1 = _mainCamera.transform;
            _cameraBasicPos = transform1.position;
            _cameraBasicQuaternion = transform1.rotation;
            while (_startTime <= 1)
            {
                transform1.position = Vector3.Lerp(_cameraBasicPos, targetTransform.position, _startTime);
                transform1.rotation = Quaternion.Lerp(_cameraBasicQuaternion, targetTransform.rotation, _startTime);
                _startTime += 0.01f;
                yield return new WaitForSecondsRealtime(0.01f);
            }

            yield return new WaitForSecondsRealtime(waitTime);

            Shot();

            yield return new WaitForSecondsRealtime(waitTime / 2.0f);

            while (_startTime >= 0)
            {
                transform1.position = Vector3.Lerp(_cameraBasicPos, targetTransform.position, _startTime);
                transform1.rotation = Quaternion.Lerp(_cameraBasicQuaternion, targetTransform.rotation, _startTime);
                _startTime -= 0.01f;
                yield return new WaitForSecondsRealtime(0.01f);
            }

            Time.timeScale = 1;
        }

        public override void AttackDown()
        {
            if (nextAttack > Time.time) return;
            nextAttack = Time.time + coolDown;

            if (curWeaponCost <= 0.001f)
            {
                PlayAudio();
                return;
            }

            StartCoroutine(AttackTime());
        }

        private void Shot()
        {
            _message.SetActive(false);
            attackLine.gameObject.SetActive(true);
            attackLine.SetPosition(0, firePoint.position);

            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, maxRange, hitLayerMask))
            {
                attackLine.SetPosition(1, hitInfo.point);
                if (hitInfo.collider.gameObject.layer == 13)
                {
                    var enemyUnit = hitInfo.collider.GetComponent<Enemy.Unit>();
                    enemyUnit.ApplyDamage(damage);
                }
            }
            else
            {
                attackLine.SetPosition(1, firePoint.position + firePoint.forward * maxRange);
            }

            PlayAudio();

            WeaponCost(weaponCost);
            _lineDisableTime = Time.time + lineDisableInt;
        }
    }
}