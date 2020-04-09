using System;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PackageControl : MonoBehaviour
    {
        public Text curItemText;

        private GameObject _curItem;
        private PlayerControl _pc;

        private void Start()
        {
            _pc = GetComponent<PlayerControl>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 12) return;
            _curItem = other.gameObject;
            UpdateLabel();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject != _curItem) return;
            _curItem = null;
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            curItemText.text = _curItem == null ? "" : _curItem.name;
        }

        public void ApplyItem()
        {
            if (_curItem != null)
                _curItem.GetComponent<Item.Item>().ApplyItem(_pc);
        }
    }
}