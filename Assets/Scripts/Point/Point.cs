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
            var tmp = Random.Range(0, 100);
            switch (pointClass)
            {
                case PointClass.Item:
                    InsItem();
                    break;
                case PointClass.Enemy:
                    break;
                case PointClass.ItemAndEnemy:
                    if (tmp < 50)
                    {
                        InsItem();
                    }
                    else
                    {
                        InsEnemy();
                    }

                    break;
                case PointClass.MostItem:
                    if (tmp < 80)
                    {
                        InsItem();
                    }
                    else
                    {
                        InsEnemy();
                    }

                    break;
                case PointClass.MostEnemy:
                    if (tmp < 20)
                    {
                        InsItem();
                    }
                    else
                    {
                        InsEnemy();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InsItem()
        {
            var gdm = GameObject.FindWithTag("GameController").GetComponent<GameManager.GameDataManager>();
            var selfTransform = transform;
            var itemData = gdm.GetRandomItem();
            _item = Instantiate(itemData.itemPrefab, selfTransform.position, selfTransform.rotation);
            _item.transform.parent = selfTransform.parent;
            _item.GetComponent<Item.Item>().itemData = itemData;
        }

        private void InsEnemy()
        {
            var gdm = GameObject.FindWithTag("GameController").GetComponent<GameManager.GameDataManager>();
            var selfTransform = transform;
            var enemyData = gdm.GetRandomEnemy();
            _item = Instantiate(enemyData.enemyPrefab, selfTransform.position, selfTransform.rotation);
            _item.transform.parent = selfTransform.parent;
        }
    }
}