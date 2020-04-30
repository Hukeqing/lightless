using System.Collections.Generic;
using UnityEngine;

namespace Room
{
    public class Room : MonoBehaviour
    {
        public string roomName;

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }
    }
}