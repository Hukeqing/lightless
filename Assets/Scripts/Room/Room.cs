using GameManager;
using UnityEngine;

namespace Room
{
    public class Room : MonoBehaviour
    {
        public Point.Point[] points;
        public RoomData roomData;

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        // public void Flush()
        // {
        //     foreach (var point in points)
        //     {
        //         point.ReNew();
        //     }
        // }
    }
}