using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
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

    public abstract class Data
    {
        // ReSharper disable once UnassignedField.Global
        public string dataName;

        // ReSharper disable once UnassignedField.Global
        public Rarity dataRarity;

        // ReSharper disable once UnassignedField.Global
        public GameObject dataPrefab;
    }

    [Serializable]
    public class ItemData : Data
    {
        public string describe;
        public ItemClass itemClass;
        public Sprite itemSprite;
    }

    [Serializable]
    public class EnemyData : Data
    {
    }

    [Serializable]
    public class RoomData : Data
    {
        public string roomDescribe;
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
            foreach (var itemData in itemDataList)
            {
                itemData.dataPrefab.GetComponent<Item.Item>().itemData = itemData;
            }

            foreach (var roomData in roomDataList)
            {
                roomData.dataPrefab.GetComponent<Room.Room>().roomData = roomData;
            }

            // itemDataList.Sort((a, b) => a.dataRarity.CompareTo(b.dataRarity));
            // enemyDataList.Sort((a, b) => a.dataRarity.CompareTo(b.dataRarity));
            // roomDataList.Sort((a, b) => a.dataRarity.CompareTo(b.dataRarity));
            _rm = GetComponent<Room.RoomManager>();
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
                itemData.dataRarity >= baseRarity ? GetRarity(itemData.dataRarity) : 0);
            var cur = Random.Range(0, raritySum);
            var tmp = 0.0f;
            foreach (var itemData in itemDataList.Where(itemData => itemData.dataRarity >= baseRarity))
            {
                tmp += GetRarity(itemData.dataRarity);
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
                enemyData.dataRarity >= baseRarity ? GetRarity(enemyData.dataRarity) : 0);
            var cur = Random.Range(0, raritySum);
            var tmp = 0.0f;
            foreach (var enemyData in enemyDataList.Where(enemyData => enemyData.dataRarity >= baseRarity))
            {
                tmp += GetRarity(enemyData.dataRarity);
                if (tmp >= cur) return enemyData;
            }

            return enemyDataList[0];
        }

        // public int GetRandomRoomIndex(Rarity baseRarity = Rarity.White)
        // {
        //     var raritySum = roomDataList.Sum(roomData =>
        //         roomData.dataRarity >= baseRarity ? GetRarity(roomData.dataRarity) : 0);
        //     var cur = Random.Range(0, raritySum);
        //     var tmp = 0.0f;
        //     for (var i = 0; i < roomDataList.Count; i++)
        //     {
        //         var roomData = roomDataList[i];
        //         if (roomData.dataRarity < baseRarity) continue;
        //         tmp += GetRarity(roomData.dataRarity);
        //         if (tmp >= cur) return i;
        //     }
        //     return 0;
        // }

        public RoomData GetRandomRoom(Rarity baseRarity = Rarity.White)
        {
            var raritySum = roomDataList.Sum(roomData =>
                roomData.dataRarity >= baseRarity ? GetRarity(roomData.dataRarity) : 0);
            var cur = Random.Range(0, raritySum);
            var tmp = 0.0f;
            foreach (var roomData in roomDataList.Where(roomData => roomData.dataRarity >= baseRarity))
            {
                tmp += GetRarity(roomData.dataRarity);
                if (tmp >= cur) return roomData;
            }

            return roomDataList[0];
        }
    }
}