using System;
using UnityEngine;

namespace Item
{
    public class WeaponItem : Item
    {
        public GameObject weaponPrefab;
        
        public override void ApplyItem(Player.PlayerControl pc)
        {
            var newWeapon = Instantiate(weaponPrefab, pc.transform);
            pc.weapon = newWeapon.GetComponent<Weapon.Weapon>();
        }
    }
}