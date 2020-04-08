using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PackageControl : MonoBehaviour
    {
        public Text curItemText;
        
        private GameObject _curItem;

        private void OnTriggerEnter(Collider other)
        {
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
    }
}
