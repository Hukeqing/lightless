using GameManager;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PackageControl : MonoBehaviour
    {
        public MessageManager messageManager;
        public Image weaponImage;
        public AudioClip pickUp, useMedication;
        public AudioSource audioSource;
        public Transform firePoint;

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
            _curItem.ApplyItem(_pc);

            if (_curItem.itemData.itemClass == ItemClass.Weapon)
            {
                weaponImage.sprite = _curItem.itemData.itemSprite;
                audioSource.clip = pickUp;
                _pc.weapon.firePoint = firePoint;
            }
            else
            {
                audioSource.clip = useMedication;
            }

            audioSource.Play();
            _curItem.BeGet();
            messageManager.ClearPackageMessage();
        }
    }
}