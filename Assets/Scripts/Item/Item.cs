using GameManager;
using UnityEngine;

namespace Item
{
    public class Item : MonoBehaviour
    {
        public ItemData itemData;

        public virtual void ApplyItem(Player.PlayerControl pc)
        {
            Debug.Log("Item miss~");
        }

        public void BeGet()
        {
            Destroy(gameObject);
        }
    }
}