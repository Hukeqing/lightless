using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Room
{
    public class RoomManager : MonoBehaviour
    {
        public List<GameObject> roomList;

        /**
         * 0 for left
         * 1 for right
         * 2 for up
         * 3 for bottom
         */
        public GameObject curRoom;

        public Transform player;

        private readonly Vector3[] _arr = new Vector3[4]
        {
            new Vector3(-50, 0, 0),
            new Vector3(50, 0, 0),
            new Vector3(0, 0, 50),
            new Vector3(0, 0, -50)
        };

        private readonly GameObject[] _existenceRoom = new GameObject[4];

        private void Start()
        {
            NewRoom();
        }

        private void Update()
        {
            var defDist = Vector3.Distance(player.position, transform.position);
            for (var i = 0; i < 4; i++)
            {
                if (!(defDist > Vector3.Distance(player.position, transform.position + _arr[i]))) continue;
                Destroy(_existenceRoom[i ^ 1]);
                Destroy(_existenceRoom[i ^ 2]);
                Destroy(_existenceRoom[i ^ 3]);
                _existenceRoom[i ^ 1] = curRoom;
                curRoom = _existenceRoom[i];
                _existenceRoom[i] = null;
                _existenceRoom[i ^ 2] = null;
                _existenceRoom[i ^ 3] = null;
                transform.position += _arr[i];
                NewRoom();
                return;
            }
        }


        private void NewRoom()
        {
            for (var i = 0; i < 4; i++)
            {
                if (_existenceRoom[i] == null)
                {
                    _existenceRoom[i] = Instantiate(roomList[Random.Range(0, roomList.Count)],
                        curRoom.transform.position + _arr[i], curRoom.transform.rotation);
                }
            }
        }
    }
}