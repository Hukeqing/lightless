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

    [Serializable]
    public struct RoomData
    {
        public string roomName;
        public string roomDescribe;
        public Rarity roomRarity;
        public GameObject roomPrefab;
    }

    public class GameDataManager : MonoBehaviour
    {
        public List<ItemData> itemDataList;
        public List<EnemyData> enemyDataList;
        public List<RoomData> roomDataList;
        [Range(0.1f, 0.9f)] public float rarityValue = 0.5f;

        private Room.RoomManager _rm;

        private void Start()
        {
            _rm = GetComponent<Room.RoomManager>();
            foreach (var itemData in itemDataList)
            {
                itemData.itemPrefab.GetComponent<Item.Item>().itemData = itemData;
            }

            foreach (var roomData in roomDataList)
            {
                roomData.roomPrefab.GetComponent<Room.Room>().roomData = roomData;
            }

            _rm.Init(this);
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

        public static Color GetColor(Rarity rarity)
        {
            var tmp = Color.white;
            switch (rarity)
            {
                case Rarity.White:
                    break;
                case Rarity.Green:
                    tmp = Color.green;
                    break;
                case Rarity.Blue:
                    tmp = Color.blue;
                    break;
                case Rarity.Purple:
                    tmp = Color.magenta;
                    break;
                case Rarity.Orange:
                    tmp = Color.yellow;
                    break;
                case Rarity.Red:
                    tmp = Color.red;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
            }

            return tmp;
        }

        public ItemData GetRandomItem(Rarity baseRarity)
        {
            var raritySum = itemDataList.Sum(itemData =>
                itemData.itemRarity >= baseRarity ? GetRarity(itemData.itemRarity) : 0);
            var cur = Random.Range(0, raritySum);
            var tmp = 0.0f;
            foreach (var itemData in itemDataList.Where(itemData => itemData.itemRarity >= baseRarity))
            {
                tmp += GetRarity(itemData.itemRarity);
                if (tmp >= cur)
                {
                    return itemData;
                }
            }

            return itemDataList[0];
        }

        public EnemyData GetRandomEnemy(Rarity baseRarity)
        {
            var raritySum = enemyDataList.Sum(enemyData =>
                enemyData.enemyRarity >= baseRarity ? GetRarity(enemyData.enemyRarity) : 0);
            var cur = Random.Range(0, raritySum);
            var tmp = 0.0f;
            foreach (var enemyData in enemyDataList.Where(enemyData => enemyData.enemyRarity >= baseRarity))
            {
                tmp += GetRarity(enemyData.enemyRarity);
                if (tmp >= cur) return enemyData;
            }

            return enemyDataList[0];
        }

        public RoomData GetRandomRoom(Rarity baseRarity = Rarity.White)
        {
            var raritySum = roomDataList.Sum(roomData =>
                roomData.roomRarity >= baseRarity ? GetRarity(roomData.roomRarity) : 0);
            var cur = Random.Range(0, raritySum);
            var tmp = 0.0f;
            foreach (var roomData in roomDataList.Where(roomData => roomData.roomRarity >= baseRarity))
            {
                tmp += GetRarity(roomData.roomRarity);
                if (tmp >= cur) return roomData;
            }

            return roomDataList[0];
        }
    }
}