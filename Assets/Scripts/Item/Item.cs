using UnityEngine;

namespace Item
{
    public enum ItemClass
    {
        Weapon,
        Medication
    }

    public class Item : MonoBehaviour
    {
        public ItemClass itemClass;

        public virtual void ApplyItem(Player.PlayerControl pc)
        {
            Debug.Log("Item miss~");
        }
    }
}