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

        public float curWeaponCost;

        private float _nextAttack;

        public virtual void Attack()
        {
            if (_nextAttack > Time.time) return;
            if (curWeaponCost <= 0) return;
            _nextAttack = Time.time + coolDown;
            ToAttack();
            WeaponCost();
        }

        protected virtual void ToAttack()
        {
            Debug.Log("No Weapon~");
        }

        protected virtual void WeaponCost()
        {
            curWeaponCost -= weaponCost;
            weaponImage.fillAmount = curWeaponCost;
        }
    }
}