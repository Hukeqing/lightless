using GameManager;
using UnityEngine;

namespace Item
{
    public class Item : MonoBehaviour
    {
        public ItemData itemData;

        public virtual void ApplyItem(Player.PlayerControl pc)
        {
        }

        public void BeGet()
        {
            Destroy(gameObject);
        }
    }
}