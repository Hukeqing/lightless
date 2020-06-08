﻿using System.Collections;
using System.Collections.Generic;
using GameManager;
using UnityEngine;

namespace Room
{
    // public class RoomPoolData
    // {
    //     public GameObject room;
    //     public bool onUse;
    // }

    public class RoomManager : MonoBehaviour
    {
        public GameObject curRoom;
        public Transform player;
        public GameDataManager gdm;
        // [Range(5, 20)] public int maxPoolCount = 10;

        private MessageManager _messageManager;

        private Coroutine _coroutine;
        private Queue<Vector3> _newRoomList;

        // private List<RoomPoolData>[] _roomPool;
        // private int _poolCount;

        /**
         * 0 for left
         * 1 for right
         * 2 for up
         * 3 for bottom
         */
        private readonly Vector3[] _arr =
        {
            new Vector3(-50, 0, 0),
            new Vector3(50, 0, 0),
            new Vector3(0, 0, 50),
            new Vector3(0, 0, -50)
        };

        private readonly GameObject[] _existenceRoom = new GameObject[4];


        public void Init(GameDataManager gm)
        {
            gdm = gm;
            // _roomPool = new List<RoomPoolData>[gm.roomDataList.Count];
            // _poolCount = 0;
            // for (var index = 0; index < _roomPool.Length; index++)
            // {
                // _roomPool[index] = new List<RoomPoolData>();
            // }

            StartCoroutine(InitRoom());
            _messageManager = GetComponent<MessageManager>();
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
                curRoom.GetComponent<Room>().Exit();
                _existenceRoom[i].GetComponent<Room>().Enter();

                _messageManager.RoomMessage(_existenceRoom[i].GetComponent<Room>().roomData);

                transform.position += _arr[i];
                StartCoroutine(NewRoom(i));
                return;
            }
        }

        private IEnumerator InitRoom()
        {
            for (var i = 0; i < 4; i++)
            {
                _existenceRoom[i] = Instantiate(gdm.GetRandomRoom().dataPrefab,
                    curRoom.transform.position + _arr[i], curRoom.transform.rotation);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator NewRoom(int i)
        {
            var rotation = curRoom.transform.rotation;
            var position = curRoom.transform.position;

            yield return new WaitForSeconds(0.1f);
            _existenceRoom[i] = Instantiate(gdm.GetRandomRoom().dataPrefab,
                position + _arr[i], rotation);
            yield return new WaitForSeconds(0.1f);
            _existenceRoom[i ^ 2] = Instantiate(gdm.GetRandomRoom().dataPrefab,
                position + _arr[i ^ 2], rotation);
            yield return new WaitForSeconds(0.1f);
            _existenceRoom[i ^ 3] = Instantiate(gdm.GetRandomRoom().dataPrefab,
                position + _arr[i ^ 3], rotation);
        }

        // private IEnumerator InstantiateRoom(Vector3 pos)
        // {
        //     var roomData = gdm.GetRandomRoomIndex();
        //     var curList = _roomPool[roomData];
        //     var i = 0;
        //     for (; i < curList.Count; i++)
        //     {
        //         if (!curList[i].onUse) break;
        //     }
        //
        //     if (i == curList.Count)
        //     {
        //         curList.Add(new RoomPoolData()
        //         {
        //             room = Instantiate(gdm.roomDataList[roomData].dataPrefab, pos, Quaternion.identity), onUse = true
        //         });
        //         _poolCount++;
        //     }
        //     else
        //     {
        //         curList[i].onUse = true;
        //         curList[i].room.SetActive(true);
        //         curList[i].room.GetComponent<Room>().Flush();
        //         curList[i].room.transform.position = pos;
        //     }
        //
        //     yield return new WaitForSeconds(0.1f);
        // }
    }
}