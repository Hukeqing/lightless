using UnityEngine;
using UnityEngine.UI;

namespace Weapon
{
    public class Weapon : MonoBehaviour
    {
        public float coolDown;
        public LayerMask hitLayerMask;
        public GameObject weaponItem;
        [HideInInspector] public Image weaponImage;
        [Range(0, 1)] public float weaponCost;
        [HideInInspector] public float curWeaponCost;
        public AudioClip weaponOut;
        [HideInInspector] public Transform firePoint;

        private AudioClip _weaponNormal;
        protected float nextAttack;
        protected AudioSource weaponAudioSource;

        protected void Init()
        {
            weaponAudioSource = GetComponent<AudioSource>();
            _weaponNormal = weaponAudioSource.clip;
        }

        public virtual void Attack()
        {
            if (nextAttack > Time.time) return;
            PlayAudio();
            if (curWeaponCost <= 0) return;
            nextAttack = Time.time + coolDown;
            ToAttack();
            WeaponCost(weaponCost);
        }

        protected virtual void ToAttack()
        {
        }

        public virtual void AttackDown()
        {
        }

        public virtual void AttackUp()
        {
        }

        protected void WeaponCost(float value)
        {
            curWeaponCost -= value;
            if (curWeaponCost < 0.001f)
            {
                curWeaponCost = 0;
            }

            weaponImage.fillAmount = curWeaponCost;
        }

        public void AddShell(float value)
        {
            curWeaponCost += value;
            if (curWeaponCost > 1) curWeaponCost = 1;
            weaponImage.fillAmount = curWeaponCost;
        }

        protected void PlayAudio()
        {
            if (curWeaponCost < 0.001f)
            {
                weaponAudioSource.clip = weaponOut;
                if (!weaponAudioSource.isPlaying) weaponAudioSource.Play();
            }
            else
            {
                weaponAudioSource.clip = _weaponNormal;
                weaponAudioSource.Play();
            }
        }
    }
}