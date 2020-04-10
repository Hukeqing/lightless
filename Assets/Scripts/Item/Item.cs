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
        public string itemName;

        public virtual void ApplyItem(Player.PlayerControl pc)
        {
            Debug.Log("Item miss~");
        }

        public virtual void BeGet()
        {
            Destroy(gameObject);
        }
    }
}