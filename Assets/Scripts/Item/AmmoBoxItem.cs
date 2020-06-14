using Player;
using UnityEngine;

namespace Item
{
    public class AmmoBoxItem : Item
    {
        [Range(0, 1.0f)] public float addValue;

        public override void ApplyItem(PlayerControl pc)
        {
            if (pc.weapon == null) return;
            pc.weapon.AddShell(addValue);
        }
    }
}