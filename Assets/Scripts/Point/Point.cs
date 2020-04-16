using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Point
{
    [Serializable]
    public struct Probably
    {
        public GameObject item;
        public int probability;
    }

    public class Point : MonoBehaviour
    {
        public Probably[] itemList;
        private GameObject _item;

        private void Start()
        {
            var cnt = itemList.Sum(probably => probably.probability);
            var cur = Random.Range(0, cnt);
            cnt = 0;
            for (var i = 0; i < itemList.Length; i++)
            {
                cnt += itemList[i].probability;
                if (cnt < cur) continue;
                _item = Instantiate(itemList[i].item, transform);
                _item.transform.localScale = Vector3.one * 0.2f;
                break;
            }
        }
    }
}