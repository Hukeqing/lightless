using GameManager;
using Item;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Point
{
    public class Point : MonoBehaviour
    {
        public Rarity rarity;
        [Range(0, 1)] public float itemValue;

        private GameObject _item;

        /// <summary>
        /// prd 算法，增加了游戏的平衡
        /// </summary>
        private static float _prdValue;

        private static readonly float[] PrdUp =
        {
            0.000000000000000000000000000f, 0.003801658303553139101756466f, 0.014745844781072675877050816f,
            0.032220914373087674975117359f, 0.055704042949781851858398652f, 0.084744091852316990275274806f,
            0.118949192725403987583755553f, 0.157983098125747077557540462f, 0.201547413607754017070679639f,
            0.249306998440163189714677100f, 0.302103025348741965169160432f, 0.360397850933168697104686803f,
            0.422649730810374235490851220f, 0.481125478337229174401911323f, 0.571428571428571428571428572f,
            0.666666666666666666666666667f, 0.750000000000000000000000000f, 0.823529411764705882352941177f,
            0.888888888888888888888888889f, 0.947368421052631578947368421f, 1.000000000000000000000000000f
        };

        private static int RandomPos(float value)
        {
            return Mathf.RoundToInt(value * 20);
        }

        private void Start()
        {
            var tmp = Random.Range(0.0f, 1.0f);
            if (tmp < _prdValue)
            {
                InsItem();
                _prdValue = PrdUp[RandomPos(itemValue)];
            }
            else
            {
                InsEnemy();
                _prdValue += PrdUp[RandomPos(itemValue)];
            }
        }

        // public void ReNew()
        // {
        //     if (_item != null)
        //     {
        //         Start();
        //     }
        // }

        private void InsItem()
        {
            var gdm = GameObject.FindWithTag("GameManager").GetComponent<GameDataManager>();
            var selfTransform = transform;
            var itemData = gdm.GetRandomItem(rarity);
            _item = Instantiate(itemData.dataPrefab, selfTransform.position, selfTransform.rotation);
            _item.transform.parent = selfTransform.parent;
            _item.GetComponent<Item.Item>().itemData = itemData;
            if (itemData.itemClass == ItemClass.Weapon)
            {
                _item.GetComponent<WeaponItem>().weaponCost = 1;
            }
        }

        private void InsEnemy()
        {
            var gdm = GameObject.FindWithTag("GameManager").GetComponent<GameDataManager>();
            var selfTransform = transform;
            var enemyData = gdm.GetRandomEnemy(rarity);
            _item = Instantiate(enemyData.dataPrefab, selfTransform.position, selfTransform.rotation);
            _item.transform.Rotate(Vector3.up * Random.Range(0, 360));
            _item.transform.parent = selfTransform.parent;
        }
    }
}