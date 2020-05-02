using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameManager
{
    public enum ItemClass
    {
        Weapon,
        Medication
    }

    public enum Rarity
    {
        White,
        Green,
        Blue,
        Purple,
        Orange,
        Red
    }

    [Serializable]
    public struct ItemData
    {
        public string itemName;
        public string describe;
        public ItemClass itemClass;
        public Rarity itemRarity;
        public GameObject itemPrefab;
    }

    [Serializable]
    public struct EnemyData
    {
        public string enemyName;
        public Rarity enemyRarity;
        public GameObject enemyPrefab;
    }

    public class GameDataManager : MonoBehaviour
    {
        public List<ItemData> itemDataList;
        public List<EnemyData> enemyDataList;
        [Range(0.1f, 0.9f)] public float rarityValue = 0.8f;

        private float _itemRaritySum;
        private float _enemyRaritySum;

        private void Start()
        {
            _itemRaritySum = itemDataList.Sum(itemData => GetRarity(itemData.itemRarity));
            _enemyRaritySum = enemyDataList.Sum(enemyData => GetRarity(enemyData.enemyRarity));
        }

        private float GetRarity(Rarity rarity)
        {
            var tmp = Rarity.White;
            var res = 1.0f;
            while (tmp != rarity)
            {
                res *= rarityValue;
                tmp += 1;
            }

            return res;
        }

        public ItemData GetRandomItem()
        {
            var cur = Random.Range(0, _itemRaritySum);
            var tmp = 0.0f;
            foreach (var itemData in itemDataList)
            {
                tmp += GetRarity(itemData.itemRarity);
                if (tmp >= cur)
                {
                    return itemData;
                }
            }

            return itemDataList[0];
        }

        public EnemyData GetRandomEnemy()
        {
            var cur = Random.Range(0, _enemyRaritySum);
            var tmp = 0.0f;
            foreach (var enemyData in enemyDataList)
            {
                tmp += GetRarity(enemyData.enemyRarity);
                if (tmp >= cur)
                {
                    return enemyData;
                }
            }

            return enemyDataList[0];
        }
    }
}