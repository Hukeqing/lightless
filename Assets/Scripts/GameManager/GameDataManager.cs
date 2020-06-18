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

        // ReSharper disable once UnusedMember.Global
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
        // ReSharper disable once UnusedMember.Local
        [Range(0.1f, 0.9f)] private const float RarityValue = 0.5f;

        private Room.RoomManager _rm;

        private static readonly float[] BasicRarityValue =
        {
            1.000000f,
            0.302103f,
            0.084744f,
            0.022702f,
            0.005894f,
            0.001503f
        };

        private static readonly float[] RarityValueList =
        {
            1.000000f,
            0.302103f,
            0.084744f,
            0.022702f,
            0.005894f,
            0.001503f
        };

        private void Start()
        {
            _rm = GetComponent<Room.RoomManager>();
            _rm.Init(this);
            foreach (var itemData in itemDataList)
            {
                itemData.dataPrefab.GetComponent<Item.Item>().itemData = itemData;
            }

            foreach (var roomData in roomDataList)
            {
                roomData.dataPrefab.GetComponent<Room.Room>().roomData = roomData;
            }
        }

        public void Sort()
        {
            foreach (var itemData in itemDataList)
            {
                itemData.dataPrefab.GetComponent<Item.Item>().itemData = itemData;
            }

            foreach (var roomData in roomDataList)
            {
                roomData.dataPrefab.GetComponent<Room.Room>().roomData = roomData;
            }

            itemDataList.Sort((a, b) =>
            {
                if (a.itemClass != b.itemClass) return a.itemClass == ItemClass.Weapon ? 1 : -1;
                return a.dataRarity.CompareTo(b.dataRarity);
            });
            enemyDataList.Sort((a, b) => a.dataRarity.CompareTo(b.dataRarity));
            roomDataList.Sort((a, b) => a.dataRarity.CompareTo(b.dataRarity));
        }

        private static float GetRarity(Rarity rarity)
        {
            return RarityValueList[(int) rarity];
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
            // return itemDataList[11];
            var raritySum = itemDataList.Sum(itemData =>
                itemData.dataRarity >= baseRarity ? GetRarity(itemData.dataRarity) : 0);
            var cur = Random.Range(0, raritySum);
            var tmp = 0.0f;
            foreach (var itemData in itemDataList.Where(itemData => itemData.dataRarity >= baseRarity))
            {
                tmp += GetRarity(itemData.dataRarity);
                if (!(tmp >= cur)) continue;

                for (var i = 0; i < 6; i++)
                {
                    RarityValueList[i] += BasicRarityValue[i];
                }

                RarityValueList[(int) itemData.dataRarity] = BasicRarityValue[(int) itemData.dataRarity];
                return itemData;
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
                if (!(tmp >= cur)) continue;

                for (var i = 0; i < 6; i++)
                {
                    RarityValueList[i] += BasicRarityValue[i];
                }

                RarityValueList[(int) enemyData.dataRarity] = BasicRarityValue[(int) enemyData.dataRarity];
                return enemyData;
            }

            return enemyDataList[0];
        }

        public RoomData GetRandomRoom(Rarity baseRarity = Rarity.White)
        {
            var raritySum = roomDataList.Sum(roomData =>
                roomData.dataRarity >= baseRarity ? GetRarity(roomData.dataRarity) : 0);
            var cur = Random.Range(0, raritySum);
            var tmp = 0.0f;
            foreach (var roomData in roomDataList.Where(roomData => roomData.dataRarity >= baseRarity))
            {
                tmp += GetRarity(roomData.dataRarity);
                if (!(tmp >= cur)) continue;

                for (var i = 0; i < 6; i++)
                {
                    RarityValueList[i] += BasicRarityValue[i];
                }

                RarityValueList[(int) roomData.dataRarity] = BasicRarityValue[(int) roomData.dataRarity];
                return roomData;
            }

            return roomDataList[0];
        }
    }
}