using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PackageControl : MonoBehaviour
    {
        private GameObject _curItem;

        private void OnTriggerEnter(Collider other)
        {
            _curItem = other.gameObject;
        }
    }
}