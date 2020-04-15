using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PackageControl : MonoBehaviour
    {
        public Text curItemText;
        public Text describeText;

        private Item.Item _curItem;
        private PlayerControl _pc;

        private void Start()
        {
            _pc = GetComponent<PlayerControl>();
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
            curItemText.text = _curItem == null ? "" : _curItem.itemName;
            describeText.text = _curItem == null ? "" : _curItem.describe;
        }

        public void ApplyItem()
        {
            if (_curItem == null) return;
            _curItem.ApplyItem(_pc);
            _curItem.BeGet();
            curItemText.text = "";
            describeText.text = "";
        }
    }
}