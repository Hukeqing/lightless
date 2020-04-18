using System.Collections.Generic;
using UnityEngine;

namespace Room
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private List<GameObject> point = new List<GameObject>();
        public GameObject pointPrefab;

        public void AddPoint()
        {
            var newPoint = Instantiate(pointPrefab, transform);
            point.Add(newPoint);
        }
    }
}