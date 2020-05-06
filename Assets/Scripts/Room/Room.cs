using GameManager;
using UnityEngine;

namespace Room
{
    public class Room : MonoBehaviour
    {
        public RoomData roomData;

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }
    }
}