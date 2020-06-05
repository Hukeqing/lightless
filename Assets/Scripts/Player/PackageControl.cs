using GameManager;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PackageControl : MonoBehaviour
    {
        public MessageManager messageManager;
        public Image weaponImage;

        private Item.Item _curItem;
        private PlayerControl _pc;

        private void Start()
        {
            _pc = GetComponent<PlayerControl>();
            _curItem = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 12) return;
            _curItem = other.gameObject.GetComponent<Item.Item>();
            UpdateLabel();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Item.Item>() != _curItem) return;
            _curItem = null;
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            if (_curItem != null)
            {
                messageManager.PackageMessage(_curItem.itemData);
            }
            else
            {
                messageManager.ClearPackageMessage();
            }
        }

        public void ApplyItem()
        {
            if (_curItem == null) return;
            if (_curItem.itemData.itemClass == ItemClass.Weapon)
            {
                weaponImage.sprite = _curItem.itemData.itemSprite;
            }

            _curItem.ApplyItem(_pc);
            _curItem.BeGet();
            messageManager.ClearPackageMessage();
        }
    }
}