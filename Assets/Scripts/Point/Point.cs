using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Point
{
    public class Point : MonoBehaviour
    {
        public GameManager.Rarity rarity;
        [Range(0, 1)] public float itemValue;

        private GameObject _item;

        private void Start()
        {
            var tmp = Random.Range(0.0f, 1.0f);
            if (tmp < itemValue)
            {
                InsItem();
            }
            else
            {
                InsEnemy();
            }
        }

        private void InsItem()
        {
            var gdm = GameObject.FindWithTag("GameManager").GetComponent<GameManager.GameDataManager>();
            var selfTransform = transform;
            var itemData = gdm.GetRandomItem(rarity);
            _item = Instantiate(itemData.itemPrefab, selfTransform.position, selfTransform.rotation);
            _item.transform.parent = selfTransform.parent;
            _item.GetComponent<Item.Item>().itemData = itemData;
        }

        private void InsEnemy()
        {
            var gdm = GameObject.FindWithTag("GameManager").GetComponent<GameManager.GameDataManager>();
            var selfTransform = transform;
            var enemyData = gdm.GetRandomEnemy(rarity);
            _item = Instantiate(enemyData.enemyPrefab, selfTransform.position, selfTransform.rotation);
            _item.transform.Rotate(Vector3.up * Random.Range(0, 360));
            _item.transform.parent = selfTransform.parent;
        }
    }
}