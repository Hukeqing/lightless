using UnityEngine;

namespace Item
{
    public class WeaponItem : Item
    {
        public GameObject weaponPrefab;
        public float weaponCost;

        public override void ApplyItem(Player.PlayerControl pc)
        {
            var transform1 = transform;
            var newWeapon = Instantiate(weaponPrefab, pc.transform);
            if (pc.weapon != null)
            {
                var newItem = Instantiate(pc.weapon.weaponItem, transform1.position, transform1.rotation);
                newItem.transform.parent = transform1.parent;
                newItem.GetComponent<WeaponItem>().weaponCost = pc.weapon.curWeaponCost;
                Destroy(pc.weapon.gameObject);
            }

            pc.weapon = newWeapon.GetComponent<Weapon.Weapon>();
            pc.weapon.curWeaponCost = weaponCost;
            pc.weapon.weaponImage = pc.weaponCostImage;
            pc.weaponCostImage.fillAmount = weaponCost;
        }
    }
}