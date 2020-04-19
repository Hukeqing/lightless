using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Point
{
    public enum PointClass
    {
        Item,
        Enemy,
        ItemAndEnemy,
        MostItem,
        MostEnemy
    }

    public class Point : MonoBehaviour
    {
        public PointClass pointClass;
        private GameObject _item;

        private void Start()
        {
            var gdm = GameObject.FindWithTag("GameController").GetComponent<GameManager.GameDataManager>();
            var selfTransform = transform;
            var itemData = gdm.GetRandomItem();
            var enemyData = gdm.GetRandomEnemy();
            var tmp = Random.Range(0, 100);
            switch (pointClass)
            {
                case PointClass.Item:
                    _item = Instantiate(itemData.itemPrefab, selfTransform.position, selfTransform.rotation);
                    _item.transform.parent = selfTransform.parent;
                    _item.GetComponent<Item.Item>().itemData = itemData;
                    break;
                case PointClass.Enemy:
                    _item = Instantiate(enemyData.enemyPrefab, selfTransform.position, selfTransform.rotation);
                    _item.transform.parent = selfTransform.parent;
                    break;
                case PointClass.ItemAndEnemy:
                    if (tmp < 50)
                    {
                        _item = Instantiate(itemData.itemPrefab, selfTransform.position, selfTransform.rotation);
                        _item.transform.parent = selfTransform.parent;
                        _item.GetComponent<Item.Item>().itemData = itemData;
                    }
                    else
                    {
                        _item = Instantiate(enemyData.enemyPrefab, selfTransform.position, selfTransform.rotation);
                        _item.transform.parent = selfTransform.parent;
                    }

                    break;
                case PointClass.MostItem:
                    if (tmp < 80)
                    {
                        _item = Instantiate(itemData.itemPrefab, selfTransform.position, selfTransform.rotation);
                        _item.transform.parent = selfTransform.parent;
                        _item.GetComponent<Item.Item>().itemData = itemData;
                    }
                    else
                    {
                        _item = Instantiate(enemyData.enemyPrefab, selfTransform.position, selfTransform.rotation);
                        _item.transform.parent = selfTransform.parent;
                    }

                    break;
                case PointClass.MostEnemy:
                    if (tmp < 20)
                    {
                        _item = Instantiate(itemData.itemPrefab, selfTransform.position, selfTransform.rotation);
                        _item.transform.parent = selfTransform.parent;
                        _item.GetComponent<Item.Item>().itemData = itemData;
                    }
                    else
                    {
                        _item = Instantiate(enemyData.enemyPrefab, selfTransform.position, selfTransform.rotation);
                        _item.transform.parent = selfTransform.parent;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}