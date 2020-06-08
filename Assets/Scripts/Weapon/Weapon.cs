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

        protected float nextAttack;

        public virtual void Attack()
        {
            if (nextAttack > Time.time) return;
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

        protected virtual void WeaponCost(float value)
        {
            curWeaponCost -= value;
            if (curWeaponCost < 0.001f) curWeaponCost = 0;
            weaponImage.fillAmount = curWeaponCost;
        }
    }
}
