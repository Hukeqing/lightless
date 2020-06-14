using UnityEngine;

namespace Item
{
    public class MedicationItem : Item
    {
        public int value;
        [Range(1.0f, 3f)] public float slowValue;

        public override void ApplyItem(Player.PlayerControl pc)
        {
            pc.AddHealth(value);
            pc.ApplySlowDamage(slowValue);
        }
    }
}